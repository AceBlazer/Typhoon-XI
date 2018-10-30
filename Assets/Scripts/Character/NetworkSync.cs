using UnityEngine;
using System.Collections;
using uLink;

public class NetworkSync : uLink.MonoBehaviour
{
	
	public double interpolationBackTime = 0.2;
	private Animator animator;
	private Vector3 toPosition;
	private Quaternion toRotation;
	public float Slerp = 0.5f;
	public float SlerpRotation = 0.5f;

	internal struct State
	{
		internal double timestamp;
		internal Vector3 pos;
		internal Quaternion rot;
		internal int anim;
	}
	
	// We store twenty states with "playback" information
	State[] m_BufferedState = new State[20];
	// Keep track of what slots are used
	int m_TimestampCount = 0;



	void Awake () {
		animator = GetComponent<Animator>();


	}

	void uLink_OnSerializeNetworkView (uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{

		// Always send transform (depending on reliability of the network view)
		if (stream.isWriting) {
			Vector3 pos = transform.localPosition;
			Quaternion rot = transform.localRotation;
			int anim = animator.GetInteger("AnimType");
			stream.Serialize (ref pos);
			stream.Serialize (ref rot);
			stream.Serialize (ref anim);
			// When receiving, buffer the information
		} else {
			// Receive latest state information
			Vector3 pos = Vector3.zero;
			Quaternion rot = Quaternion.identity;
			int anim = 0;
			stream.Serialize (ref pos);
			stream.Serialize (ref rot);
			stream.Serialize (ref anim);
			toPosition = pos;
			toRotation = rot;
			animator.SetInteger("AnimType", anim);
			// Shift buffer contents, oldest data erased, 18 becomes 19, ... , 0 becomes 1
			for (int i = m_BufferedState.Length - 1; i >= 1; i--) {
				m_BufferedState[i] = m_BufferedState[i - 1];
			}
			
			// Save currect received state as 0 in the buffer, safe to overwrite after shifting
			State state;
			state.timestamp = info.timestamp;
			state.pos = pos;
			state.rot = rot;
			state.anim = anim;
			m_BufferedState[0] = state;
			
			// Increment state count but never exceed buffer size
			m_TimestampCount = Mathf.Min (m_TimestampCount + 1, m_BufferedState.Length);
			
			// Check integrity, lowest numbered state in the buffer is newest and so on
			for (int i = 0; i < m_TimestampCount - 1; i++) {
				if (m_BufferedState[i].timestamp < m_BufferedState[i + 1].timestamp)
					Debug.Log ("State inconsistent");
			}
			
		}
	}
	
	// This only runs where the component is enabled, which is only on remote peers (server/clients)
	void Update ()
	{
		if (uLink.Network.isClient)
		{
			float distance = Vector3.Distance(transform.position, toPosition);
			if (distance > 1)
				transform.position = toPosition;
			else
				transform.localPosition = Vector3.Lerp (transform.position, toPosition, Slerp);
		
			transform.localRotation = Quaternion.Slerp (transform.rotation, toRotation, SlerpRotation);
		}

	}
}
