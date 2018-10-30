using UnityEngine;
using System.Collections;

public class rtsCamerac : MonoBehaviour {

	public float targetZoomInMax  = 4.0f;
	public float targetZoomOutMax  = 10.0f;
	public GameObject target ;
	public GameObject gm;
	public bool cameraLocked = false;
	public string who;


	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Lycra(Clone)");
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if(Input.GetKeyDown("space")) {
			if(cameraLocked==false) {
				cameraLocked=true;
			}
			else {
				cameraLocked=false;
			}

		}
			

		if(cameraLocked == true){
			Vector3 pos = target.transform.position;
			Vector3 mypos = transform.position;
		

				mypos.x = pos.x;
				mypos.z = pos.z + 7;
				transform.position = mypos;
		


		}

	



		// Mouse wheel moving forward
		if(Input.GetAxis("Mouse ScrollWheel") < 0 )
		{
			if (transform.position.y < targetZoomInMax)
			transform.position+= new Vector3(0,1,0);
		}

		 //Mouse wheel moving backward
		if(Input.GetAxis("Mouse ScrollWheel") > 0 )
		{
			if (transform.position.y < targetZoomOutMax)
				transform.position +=new  Vector3(0,-1,0);
		}
      

	}
}
