using UnityEngine;
using System.Collections;

public class RecievedMovement : MonoBehaviour {

	public GameObject blueSpawn;
	public GameObject redSpawn;
	int tm;
	public Vector3 newposition;
	public float speed;
	public float walkRange;
    public NavMeshAgent nav;
	public GameObject graphics;
    public GameObject sample;
	public bool canmove=true;
    public AnimationClip Run;
	public GameObject recallPart;



	void Start () {
        nav = GetComponent<NavMeshAgent>();
		tm = GetComponent<gamemanager1> ().teamm;
		newposition = this.transform.position;

	}
	

	void Update () {

        bool RMB = Input.GetMouseButtonDown(1);
		RaycastHit hit;

        if (RMB)
        {

            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Ground")
            {
                RecievedMove(hit.point);
            }


        }


		if (Input.GetKeyDown ("b")) {	
			Instantiate (recallPart, this.transform.position, Quaternion.Euler(new Vector3(-90,0,0)));
			StartCoroutine ("recallcountdown");
			DestroyObject (recallPart);


		}


        
		if(nav.destination != newposition && newposition!=null)
		{
			
			nav.destination = newposition;


		}

		if (Vector3.Distance (newposition, this.transform.position) <= walkRange) {
			GetComponent<Animation> ().Play ("Alycra - Idle");
		} else {
			transform.LookAt (newposition);
			GetComponent<Animation>().Play("Alycra - Run");
		}

    }


    public void RecievedMove(Vector3 movePos){
		if (canmove == true) {
			newposition = movePos;
			Instantiate (sample, newposition, Quaternion.identity);

		}
    }

	IEnumerator recallcountdown() {
		yield return new WaitForSeconds (4);

		if (tm == 0) {
			this.transform.position = blueSpawn.transform.position;
		} else {
			this.transform.position = redSpawn.transform.position;
		}
	}

}
