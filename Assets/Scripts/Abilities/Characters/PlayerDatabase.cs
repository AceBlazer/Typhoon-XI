using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class PlayerDatabase : uLink.MonoBehaviour
	{
	public List<PlayerDataClass> PlayerList = new List<PlayerDataClass>();
	public string playerName;
	public int playerScore;
	public string playerTeam;
	private ScoreTable scoreScript;

	// Use this for initialization

	void Start () {
		scoreScript = GetComponent<ScoreTable>();
	}
	
	// Update is called once per frame

	void Update () 
	{
	
	}
	//Accesed by PickSelection script to add the current joined player to the list playerlist
	public void AddPlayerNameToList (string pName, uLink.NetworkPlayer nPlayer)
	{
		if (PlayerList.Exists(x => x.networkPlayer == nPlayer))
		{
		
		}
		else
		{

			networkView.RPC("AddPlayerOnClients", uLink.RPCMode.OthersBuffered, nPlayer, pName);
			PlayerDataClass capture = new PlayerDataClass ();
			capture.networkPlayer = nPlayer;
			capture.playerName = pName;
			
			PlayerList.Add(capture);
		}
	}
	//Accesed by SpawnScript script to add the current team and Obj to the list playerlist
	public void AddPlayerDataToList (uLink.NetworkPlayer nPlayer, string team, GameObject obj)
	{
		for (int i = 0; i < PlayerList.Count; i++) 
		{
			if (PlayerList[i].networkPlayer == nPlayer)
			{
				PlayerList[i].playerTeam = team;
				PlayerList[i].playerObj = obj;
				networkView.RPC("AddTeamOnClients", uLink.RPCMode.OthersBuffered, PlayerList[i].playerName, team);
			}
			

		}
	}
	//Returns the name of the player assigned to the current networkplayer nPlayer
	public string GetPlayerName (uLink.NetworkPlayer nPlayer)
	{
		string pName = "";
		for (int i = 0; i < PlayerList.Count; i++) 
		{

			if (PlayerList[i].networkPlayer == nPlayer)
			{
				pName = PlayerList[i].playerName;
			}
			

		}
		return pName;
	}

	void uLink_OnPlayerConnected(uLink.NetworkPlayer networkPlayer) 
	{
		//networkView.RPC("AddPlayerToList", uLink.RPCMode.AllBuffered, networkPlayer);
	}



	void uLink_OnPlayerDisconnected(uLink.NetworkPlayer networkPlayer) 
	{
		//networkView.RPC("RemovePlayerFromList", uLink.RPCMode.AllBuffered, networkPlayer);
	}

	//Add the current joined player to the list playerlist on clients
	[PunRPC]
	void AddPlayerOnClients ( uLink.NetworkPlayer nPlayer, string pName)
	{
		if (PlayerList.Exists(x => x.networkPlayer == nPlayer))
		{
		}
		else
		{
			PlayerDataClass capture = new PlayerDataClass ();
			capture.networkPlayer = nPlayer;
			capture.playerName = pName;
			PlayerList.Add(capture);
		}
	}

	//Add the current joined player team to the list playerlist on clients
	[PunRPC]
	void AddTeamOnClients (string sPname, string team)
	{
		for (int i = 0; i < PlayerList.Count; i++) 
		{
			if (PlayerList[i].playerName == sPname)
			{
				PlayerList[i].playerTeam = team;

			}
		}
	}



	[PunRPC]
	void RemovePlayerFromList (uLink.NetworkPlayer nPlayer)
	{

		for (int i = 0; i < PlayerList.Count; i++) 
		{
			if (PlayerList[i].networkPlayer == nPlayer)
			{
				PlayerList.RemoveAt(i);
			}

		}
	}


	/*
	[PunRPC]
	void EditPlayerListWithName (uLink.NetworkPlayer nPlayer, string pName)
	{
		if (uLink.Network.isServer)
		networkView.RPC("EditPlayerListWithName", uLink.RPCMode.OthersBuffered, nPlayer, pName);
		//FIND THE PLAYER IN THE PLAYER LIST

		for (int i = 0; i < PlayerList.Count; i++) 
		{
			if (PlayerList[i].networkPlayer == nPlayer)
			{
				PlayerList[i].playerName = pName;

			}

		}
	}
	*/


	//Accesed by PlayerStats script to increment the player score when a player kills an enemy
	//An RPC is sent out across the network so that everyone gets the latest team score.
	[PunRPC]
	public void EditPlayerListWithScore (string nPlayer)
	{

		if (uLink.Network.isServer)
		{
		networkView.RPC("EditPlayerListWithScore", uLink.RPCMode.Others, nPlayer);
		}
		for (int i = 0; i < PlayerList.Count; i++) 
		{
			if (PlayerList[i].playerName == nPlayer)
			{
				PlayerList[i].playerScore ++;

				if (PlayerList[i].playerTeam == "blue")
				{
					scoreScript.UpdateBlueTeamScore();
				}
				else
				{
					scoreScript.UpdateRedTeamScore();
				}
				
			}
			
		}
	}

}
