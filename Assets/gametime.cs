using UnityEngine;
using System.Collections;

public class gametime : MonoBehaviour {

	public float sec = 0;
	public int intsec;
	public int minutes=0;
	public string seconds;
	public string time;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {		
		sec = sec + Time.deltaTime;
	}

	void OnGUI() {
		intsec = (int)sec;
		if (intsec < 10) {
			seconds = "0" + intsec.ToString();
		} else {
			seconds = intsec.ToString();
		}

		if (intsec >= 60) {
			minutes++;
			sec = 0;
		}

		time = minutes + ":" + seconds;

		GUI.Label (new Rect (Screen.width-50,10, 100, 20), time);

	}

}
