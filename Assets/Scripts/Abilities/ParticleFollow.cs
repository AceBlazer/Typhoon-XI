using UnityEngine;
using System.Collections;

public class ParticleFollow : MonoBehaviour {

	public GameObject originatorObj;
	public float offsetY;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (originatorObj)
		{
			Vector3 tmpTransform = new Vector3(originatorObj.transform.position.x, originatorObj.transform.position.y + offsetY, originatorObj.transform.position.z);
			transform.position = originatorObj.transform.position;
		}
	
	}
}
