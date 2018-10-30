using UnityEngine;
using uLink;
[System.Serializable]
//This class is used to store all the info about each connected player
public class PlayerDataClass {

	public uLink.NetworkPlayer networkPlayer;
	public string playerName;
	public GameObject playerObj;
	public int playerScore;
	public string playerTeam;
	public int playerLvl;
	public float playerExp;
	public int kills;
	public int deaths;
	public int assist;
	public int gold;

	public float ad;
	public float ap;
	public float armor;
	public float mr;
	public float speed;




	public PlayerDataClass Constructor ()
	{
		PlayerDataClass capture = new PlayerDataClass ();
		capture.networkPlayer = networkPlayer;
		capture.playerName = playerName;
		capture.playerObj = playerObj;
		capture.playerScore = playerScore;
		capture.playerTeam = playerTeam;
		capture.playerLvl = playerLvl;
		capture.playerExp = playerExp;
		capture.kills = kills;
		capture.deaths = deaths;
		capture.assist = assist;
		capture.gold = gold;

		capture.ad = ad;
		capture.ap = ap;
		capture.armor = armor;
		capture.mr = mr;
		capture.speed = speed;


		return capture;
	}


}