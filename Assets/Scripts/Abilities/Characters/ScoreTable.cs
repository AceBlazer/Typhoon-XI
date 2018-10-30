using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
/// This script is attached to the GameManager and it is responsible
/// for tracking the player score
/// This script is accessed by the PlayerStats script to update the team score.
/// </summary>


public class ScoreTable : uLink.MonoBehaviour {
	
	//Variables Start___________________________________
	
	//These variables are used in displaying the scoreboard.
	

	
	public int redTeamScore;
	
	public int blueTeamScore;




	//THESE ARE USED FOR THE WINNING SCORE


	public bool redTeamHasWon = false;

	public bool blueTeamHasWon = false;


	public GameObject blueBaseTower;
	public GameObject redBaseTower;
	public PlayerStats blueBaseStats;
	public PlayerStats redBaseStats;

	private LeftPanelUi leftPanelScript;

	public GameObject slainInfoObj;


	//Variables End_____________________________________
	
	
	// Use this for initialization
	void Start () 
	{

		leftPanelScript = GameObject.Find("LeftPanel").GetComponent<LeftPanelUi>();

	}
	
	// Update is called once per frame
	void Update () 
	{

		leftPanelScript.blueKillsText.text = blueTeamScore.ToString();
		leftPanelScript.redKillsText.text = redTeamScore.ToString();

		if (blueTeamHasWon)
		{
			redBaseTower.GetComponent<Animator>().SetBool("destroyed", true);
			StartCoroutine(EndMatch());
		}

		if (redTeamHasWon)
		{
			blueBaseTower.GetComponent<Animator>().SetBool("destroyed", true);
			StartCoroutine(EndMatch());
		}





	}

	//When a player connects to the game, the server sends an RPC with the team scores
	void uLink_OnPlayerConnected(uLink.NetworkPlayer nPlayer)
	{
		if (uLink.Network.isServer)
		{
		networkView.RPC("ServerRefreshScore", nPlayer, redTeamScore, blueTeamScore);
		}
	}

	

	
	//Whenever the player's score increases
	//The PlayerDatabase script will send a signal to this script to increment the
	//overall team score.

	public void UpdateRedTeamScore ()
	{
		redTeamScore++;

	}
	

	public void UpdateBlueTeamScore ()
	{
		blueTeamScore++;
		
	}
	
	[PunRPC]
	void ServerRefreshScore (int redScore, int blueScore)
	{
		redTeamScore = redScore;
		
		blueTeamScore = blueScore;
	}
	

	
	IEnumerator EndMatch ()
	{
		yield return new WaitForSeconds(5);
		//blueTeamHasWon = false;
		//redTeamHasWon = false;
		Application.Quit();
	}

	
	
	
	
	
	
	
	
	
	
	
}
