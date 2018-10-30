using UnityEngine;
using System.Collections;

public class RecievedMovementGalilei : MonoBehaviour {


	public Vector3 newposition;
	public float speed;
	public float walkRange;
	public NavMeshAgent nav;
	public GameObject graphics;
	public GameObject sample;
	public bool canmove=true;
	public AnimationClip Run;

	void Start () {
		nav = GetComponent<NavMeshAgent>();
		newposition = this.transform.position;
	}


	void Update () {

		bool RMB = Input.GetMouseButtonDown(1);

		if (RMB)
		{

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast (ray, out hit) && hit.transform.tag == "Ground") {
				RecievedMove (hit.point);
			}


		}

		if (nav.destination != newposition && newposition != null) {
			
			nav.destination = newposition;
			GetComponent<Animation> ().CrossFade ("RUN00_F");
				

		} 
		if (Vector3.Distance (nav.destination, transform.position) <= 0.1f) {
			GetComponent<Animation> ().CrossFade ("WAIT02");
		} else {
			transform.LookAt (newposition);
			GetComponent<Animation>().CrossFade("RUN00_F");
		}



	

	}


	public void RecievedMove(Vector3 movePos){
		if (canmove == true) {
			newposition = movePos;
			Instantiate (sample, newposition, Quaternion.identity);

		}
	}




}
