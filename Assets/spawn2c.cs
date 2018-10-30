using UnityEngine;
using System.Collections;

public class spawn2c : MonoBehaviour {

	public Transform node1;
	public GameObject minionPrefab;
	public bool spawn=true;
	public bool did = false;
	string time;
	public GameObject gm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		time= gm.GetComponent<gametime> ().time;
		if (string.Equals (time, "0:45")) {
			if (!did) {
				spawnMinion ();
				did = true;
			}
		}
		}

		
	public void spawnMinion() {
		int i = 0;
		do {
			StartCoroutine ("spawncountdown2");
			i++;
			Instantiate (minionPrefab, node1.position, node1.rotation);
		} while(i <= 7);
		StartCoroutine ("spawncountdown3");
	}
	IEnumerator spawncountdown2() {
		yield return new WaitForSeconds (0.5f);
	}
	IEnumerator spawncountdown3() {
		yield return new WaitForSeconds (30);
		spawnMinion ();
	}


}
