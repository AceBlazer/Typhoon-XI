using UnityEngine;
using System.Collections;

public class announcer : MonoBehaviour {

	int interval = 1; 
	float nextTime = 0;
	string time;
	public GameObject gm;
	public AudioClip welcome;
	public AudioClip minionsSpawned;
	public AudioSource audio;
	// Use this for initialization
	void Start () {
		//InvokeRepeating("announc", 0, 1.0f);

	}
	
	// Update is called once per frame
	void Update () {
		time= gm.GetComponent<gametime> ().time;

		if (Time.time >= nextTime) {

			if (string.Equals (time, "0:05")) {
					audio.PlayOneShot (welcome);
				}
			
			if (string.Equals (time, "0:15")) {
					audio.PlayOneShot (minionsSpawned);
				}
			

			nextTime += interval; 
		}

	}

	/*public void announc(){
		
	}*/



}
