using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Utility;

public class MinionSpawnManager : uLink.MonoBehaviour {

	public GameObject BlueMinionCreator;
	public GameObject BlueMinionProxy;
	public GameObject RedMinionCreator;
	public GameObject RedMinionProxy;
	public string team;
	public WaypointCircuit circuitScript;
	private PlayerStats playerStatScript;
	private MinionsScript minionScript;
	private WaypointProgressTracker waypointScript;

	private int waveNumber;
	public float health;
	public float healthRegen;
	public int lvl;
	public float expToGive;
	public float goldToGive;
	public float ad;
	public float ap;
	public float adRes;
	public float apRes;

	public float adFinal;
	public float apFinal;
	GameObject gameManager;
	ScoreTable scoreScript;

	void Start () {
		StartCoroutine(MinionSpawner());
		gameManager = GameObject.Find("GameManager_mn");
		
		scoreScript = gameManager.GetComponent<ScoreTable>();
	}

	IEnumerator MinionSpawner () {
		if (uLink.Network.isServer)
		{
		
			SpawnMinion();
		
			yield return new WaitForSeconds(2);

			SpawnMinion();

			yield return new WaitForSeconds(2);

			SpawnMinion();
		


			//ADD A LEVEL AFTER EACH WAVE SPAWN
			lvl ++;

		}
		yield return new WaitForSeconds(30);
		StartCoroutine(MinionSpawner());
	}
	// This method spawns each enemy wave for the teams
	// Sends data to each minion playerstats script
	void SpawnMinion () {



		if (team == "blue")
		{
			adFinal = scoreScript.blueTeamScore;
			adFinal *= 2;
			GameObject minionObj = (GameObject)uLink.Network.Instantiate(uLink.NetworkPlayer.server, BlueMinionProxy,BlueMinionProxy,BlueMinionCreator, transform.position, transform.rotation, 0);
			waypointScript = minionObj.GetComponent<WaypointProgressTracker>();
			playerStatScript = minionObj.GetComponent<PlayerStats>();
			minionScript = minionObj.GetComponent<MinionsScript>();
			waypointScript.circuit = circuitScript;
			minionObj.name = "BlueWave" + waveNumber.ToString();
		}
		if (team == "red")
		{
			adFinal = scoreScript.redTeamScore;
			adFinal *= 2;
			GameObject minionObj = (GameObject)uLink.Network.Instantiate(uLink.NetworkPlayer.server, RedMinionProxy,RedMinionProxy,RedMinionCreator, transform.position, transform.rotation, 0);
			waypointScript = minionObj.GetComponent<WaypointProgressTracker>();
			playerStatScript = minionObj.GetComponent<PlayerStats>();
			minionScript = minionObj.GetComponent<MinionsScript>();
			waypointScript.circuit = circuitScript;
			minionObj.name = "RedWave" + waveNumber.ToString();
		}
		playerStatScript.playerTeam = team;
		playerStatScript.maxHealth = health + adFinal;
		playerStatScript.myHealth = health	+ adFinal;
		playerStatScript.healthRegenerate = healthRegen;
		playerStatScript.playerLvl = lvl;
		playerStatScript.expToGive = expToGive;
		playerStatScript.goldToGive = goldToGive;
		playerStatScript._bAdValue = ad + adFinal;
		playerStatScript._bApValue = ap;
		playerStatScript.adRes = adRes;
		playerStatScript.apRes = apRes;
		playerStatScript.UpdateStatsOnClients();
		minionScript.attackSpeed = 3;

		waveNumber++;
	}

	void Update () {
		
	}
}
