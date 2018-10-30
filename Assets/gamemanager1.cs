using UnityEngine;
using System.Collections;

public class gamemanager1 : Photon.MonoBehaviour {
	public GameObject[] blueSpawn;
	public GameObject[] redSpawn;
	public GameObject lobbyCam;
	public GameObject playerCam;
	public GameObject myPlayer;
	public int state = 0;
	public int teamm=0;

	void Connect () {
		//PhotonNetwork.ConnectToBestCloudServer ("1.0");
		PhotonNetwork.ConnectUsingSettings("1.0");
	}

	void OnJoinedLobby() {
		state = 1;
	}

	void OnPhotonRandomJoinFailed() {
		PhotonNetwork.CreateRoom (null);
	}

	void OnJoinedRoom() {
		state = 2;
	}

	void Start () {
		
	}
	

	void Update () {
		
	}

	void OnGUI() {
		switch (state) {
		case 0: 
			if (GUI.Button (new Rect (10, 10, 100, 30), "Connect")) {
				Connect ();
			}
			break;
		case 1:
			GUI.Label(new Rect(160,30,100,30),"Connected");
			if (GUI.Button (new Rect (10, 90, 200, 30), "Search Active Game")) {
				PhotonNetwork.JoinRandomRoom ();
			}
			break;
		case 2:
			//champ selelct
			GUI.Label(new Rect(160,130,200,30),"Select your Team");
			if (GUI.Button (new Rect (200, 190, 100, 30), "Red Team")) {
				teamm = 1;
			}
			if (GUI.Button (new Rect (70, 190, 100, 30), "Blue Team")) {
				teamm = 0;
			}
			GUI.Label(new Rect(160,220,200,30),"Select your champion");
			if (GUI.Button (new Rect (70, 250, 100, 30), "Lycra")) {
				spawn (teamm,"Lycra");
			}
			if (GUI.Button (new Rect (200, 250, 100, 30), "Liley")) {
				spawn (teamm,"Liley");
			}
			break;

		case 3:
			//ingame
			GetComponent<gametime> ().enabled = true;

			break;
		}
	}
	void spawn(int team,string don) {
		GameObject myspawn = blueSpawn [Random.Range (0, blueSpawn.Length)];
		state = 3;
		lobbyCam.SetActive (false);
		playerCam.SetActive (true);

		if (team == 0) {
			 myspawn = blueSpawn [Random.Range (0, blueSpawn.Length)];
		} else {
			 myspawn = redSpawn [Random.Range (0, redSpawn.Length)];
		}

		 myPlayer = PhotonNetwork.Instantiate (don,myspawn.transform.position,myspawn.transform.rotation,0);


		if (team == 0) {
			myPlayer.GetComponent<playerbasicMinion> ().enabled = true;
		}
		if (team == 1) {
			myPlayer.GetComponent<PlayerbasicMinionRed> ().enabled = true;
		}
		if(don == "Lycra") {
			myPlayer.GetComponent<RecievedMovement> ().enabled = true;
			playerCam.GetComponent<rtsCamerac>().target=GameObject.Find("Lycra(Clone)");
		}
		if(don == "Liley") {
			myPlayer.GetComponent<RecievedMovementGalilei> ().enabled = true;
			playerCam.GetComponent<rtsCamerac>().target=GameObject.Find("Liley(Clone)");
		}
		myPlayer.GetComponent<CharacterController> ().enabled = true;

		
	}
}
