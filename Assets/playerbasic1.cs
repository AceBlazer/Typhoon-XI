using UnityEngine;
using System.Collections;

public class playerbasic1 : MonoBehaviour {

	public int range ;
	public GameObject gun;
	public GameObject sample;
	public GameObject bullet;
	public int moveSpeed ;
	public RecievedMovement rm;
	public CharacterController controller;
	bool firing=false;
	bool canmove=true;
	Vector3 newposition;
	bool instantiated = false;
	GameObject cp;
	GameObject enemy;
	GameObject basic;

	// Use this for initialization
	void Start () {
		rm = GetComponent<RecievedMovement> ();
	}
	
	// Update is called once per frame
	void Update () {




		bool RMB = Input.GetMouseButtonDown(1);

		if (RMB) {

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit) && hit.transform.tag == "BluePlayerTag") {
				enemy = GameObject.FindWithTag ("BluePlayerTag");

				if (Vector3.Distance (transform.position, enemy.transform.position) <= range) {

					print ("here");
					attack ();
				}


				print ("enemy selected");
				//RecievedMove (hit.point);

				if (Vector3.Distance (transform.position, enemy.transform.position) <= range) {
					if (!instantiated) {
						instantiated = true;
						cp = Instantiate (sample, enemy.transform.position, enemy.transform.rotation) as GameObject;
					}
					transform.LookAt (enemy.transform);
					GetComponent<Animation> ().Play ("Alycra - Basic");
					attack (); 	
				}
				else {
					RecievedMove (hit.point); 

				}
			}


			if (Physics.Raycast (ray, out hit) && hit.transform.tag == "Ground" && cp) {
				Destroy (cp);
			//	canmove = false;
			    instantiated = false;
		    }

		}

	}


	public void RecievedMove(Vector3 movePos){
		newposition = movePos;
		if (!instantiated ) {
			instantiated = true;
			cp = Instantiate (sample, enemy.transform.position, enemy.transform.rotation) as GameObject; 

			if (Vector3.Distance (transform.position, enemy.transform.position) <= range) {

				print ("here");
				transform.LookAt (enemy.transform);
				GetComponent<Animation> ().Play ("Alycra - Basic");
				attack (); 	
			}
			else {
				print ("ici");
				transform.LookAt (enemy.transform);
				rm.canmove = true;
				rm.newposition = enemy.transform.position;

			}

		}
	}

	public void attack() {
		if (!firing) {
			firing=true;
			basic =Instantiate (bullet, gun.transform.position, gun.transform.rotation) as GameObject;
			StartCoroutine ("countdown");
			 }
	}

	IEnumerator countdown() {
		yield return new WaitForSeconds (1);
		firing=false;
	}

}
