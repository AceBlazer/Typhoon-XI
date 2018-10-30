using UnityEngine;
using System.Collections;

public class playerbasicMinion : MonoBehaviour {

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
	public Transform enemy;
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

			if (Physics.Raycast (ray, out hit) && (hit.transform.tag == "RedTeamTriggerTag"||hit.transform.tag == "CreepTriggerTag")) {
				
				enemy = hit.transform;
			//	enemy=GameObject.FindWithTag("RedTeamTriggerTag").transform;

				if (Vector3.Distance (transform.position, enemy.transform.position) <= range) {


					attack ();
				}



				//RecievedMove (hit.point);

				if (Vector3.Distance (transform.position, enemy.transform.position) <= range) {
					if (!instantiated) {
						instantiated = true;
						cp = Instantiate (sample, enemy.transform.position, enemy.transform.rotation) as GameObject;
					}
					transform.LookAt (enemy.transform);
			
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

		if (enemy&&basic) {
			basic.transform.LookAt (enemy.position);
			if (Vector3.Distance (basic.transform.position, enemy.transform.position) < 0.1f) 
				DestroyObject (basic);

		}




	}


	public void RecievedMove(Vector3 movePos){
		newposition = movePos;
		if (!instantiated ) {
			instantiated = true;
			cp = Instantiate (sample, enemy.transform.position, enemy.transform.rotation) as GameObject; 

			if (Vector3.Distance (transform.position, enemy.transform.position) <= range) {


				transform.LookAt (enemy.transform);

				attack (); 	
			}
			else {
				
				transform.LookAt (enemy.transform);
				rm.canmove = true;
				rm.newposition = enemy.transform.position;

			}

		}
	}

	public void attack() {
		if (!firing) {
			firing=true;
			StartCoroutine ("countdown");
			basic =Instantiate (bullet, gun.transform.position, gun.transform.rotation) as GameObject;
			bullet.GetComponent<basicprojectileMinionc>().target=enemy;
		}
	}

	IEnumerator countdown() {
		yield return new WaitForSeconds (1);
		firing=false;
	}

}
