using UnityEngine;
using System.Collections;

public class basicprojectileMinionc : MonoBehaviour {



    public Transform target;
	public int speed = 2;
	public playerbasicMinion basic;
	public GameObject impact;

	void Start () {
		//target = basic.GetComponent<playerbasicMinion> ().enemy as Transform ;

	}
	
	void Update () {
		

		//transform.LookAt (target);
		transform.Translate (Vector3.forward * 4 * Time.deltaTime);
		if (target) {
			if (Vector3.Distance (this.transform.position, target.position) < 0.1f) {
				Instantiate (impact, target.transform.position, target.transform.rotation);
				Destroy (gameObject);
			}
		}
	}
}



