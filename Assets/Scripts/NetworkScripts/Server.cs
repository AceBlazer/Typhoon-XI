using UnityEngine;
using uLobby;
using uZone;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class Server : uLink.MonoBehaviour
{
	//ULOBBY VARIABLES
	public string serverName;
	public string lobbyIP = "127.0.0.1";
	public int lobbyPort = 45000;

	public string gameServerIP = "127.0.0.1";
	public int gameServerPort = 2000;

	public int numPlayers = 0;
	public int maxPlayers = 16;

	public bool started = false;
	public bool fullNotification = false;
	public bool ended = false;
	

	public GameObject windowPickSelection;
	public MenuManager mainMenuManager;

	private PlayerDatabase playerDatabaseScript;
	private PickSelection pickSelectionScript;
	//public bool uzoneconnected = false;

	public string playersBlue;
	public string playersRed;

	public GameObject cameraMain;
	public GameObject lightsObj;
	public Camera cameraParticle;

	void Awake()
	{
		uZone.InstanceManager.Server.Initialise();
	}

	public void Start()
	{
		playerDatabaseScript = GameObject.Find("GameManager_mn").GetComponent<PlayerDatabase>();
		GameObject PickSelectionObj = GameObject.Find("Window_PickSelection_mn");
		pickSelectionScript = PickSelectionObj.GetComponent<PickSelection>();

		uZone.InstanceManager.Server.AddListener(this.gameObject);
		//StartServer();
	}

	public void StartServer()
	{
		Lobby.AddListener(this);

		Lobby.ConnectAsServer(lobbyIP, lobbyPort);
	

		//mainMenuManager.ShowMenu(windowPickSelection.GetComponent<Menu>());

		
		//pickSelectionScript.CreateLayout();
		//pickSelectionScript.CreatePickLayout();

	}

	public void StartServerManual()
	{
		Lobby.AddListener(this);
		//gameServerIP = "127.0.0.1";
		//gameServerPort = 7000;
		Lobby.ConnectAsServer(lobbyIP, lobbyPort);
		uLink.Network.InitializeServer(32, 7000);


		mainMenuManager.ShowMenu(windowPickSelection.GetComponent<Menu>());


		pickSelectionScript.CreateLayout();
		pickSelectionScript.CreatePickLayout();
		cameraMain.SetActive(false);
		lightsObj.SetActive (false);
		cameraParticle.clearFlags = CameraClearFlags.SolidColor;
	}
	
	public void OnGUI()
	{
		/*

		if(uZone.InstanceManager.Server.cmdArgs != null)
		{
			GUI.Box(new Rect(10, 5, 230, 130), "");

		}
	
			GUI.Label(new Rect(10, 40, 230, 20), "ID: " + uZone.InstanceManager.Server.cmdArgs[1] + "  " + uZone.InstanceManager.Server.instanceId);
			GUI.Label(new Rect(10, 60, 230, 20), "Port: " + uZone.InstanceManager.Server.gameServerPort.ToString());
			if(uZone.InstanceManager.Server.cmdArgs != null && uZone.InstanceManager.Server.cmdArgs.Count() > 1)
			{
				string argString = "";
				for(int i = 1; i < uZone.InstanceManager.Server.cmdArgs.Count(); i++)
					argString += uZone.InstanceManager.Server.cmdArgs[i] + " ";
				GUI.Label(new Rect(10, 80, 230, 35), "Parameters:\n" + argString);
			}
	
			GUILayout.BeginArea(new Rect(10, 10, 300, 100));

			if (Lobby.peerType == LobbyPeerType.Connecting)
				GUILayout.Label("Connecting to lobby on " + lobbyIP + ":" + lobbyPort + "...");

			if (Lobby.isConnected)
			GUILayout.Label("Server connected to lobby."+ gameServerIP + ":" + gameServerPort + "...");

			GUILayout.EndArea();


		*/
	}

	void Update ()
	{

		numPlayers = playerDatabaseScript.PlayerList.Count();
		maxPlayers = pickSelectionScript.allslots.Count();

		if (numPlayers == maxPlayers && !fullNotification && maxPlayers != 0)
		{
			fullNotification = true;
			started = true;
			ChangeNumPlayers();
		}
	}

	private void uLink_OnPlayerConnected(uLink.NetworkPlayer player)
	{
		playersBlue = "NEW BLUE PLAYER";
		playersRed = "NEW RED PLAYER";
	}

	private void uLobby_OnConnected()
	{
		Debug.Log("Connected to lobby");

		// Add this server to the server registry. The game server IP and port is what will be listed in clients. The
		// last two arguments are extra data that can be extracted in the clients - we use this for storing extra
		// information about the server.
		ServerRegistry.AddServer(gameServerIP, gameServerPort, numPlayers, maxPlayers, serverName, started, playersBlue, playersRed);

		ChangeNumPlayers();
	}

	void uZone_OnDisconnected()
	{
		//Try and reconnect to uZone in case the connection is dropped.
		uZone.InstanceManager.Server.Reconnect();
	}
	
	void uZone_OnTerminate()
	{
		UnityEngine.Debug.Log("Terminating instance ...");	
	}
	
	void uZone_OnConnected(string id)
	{
		serverName = uZone.InstanceManager.Server.cmdArgs[1];
		maxPlayers = Convert.ToInt32(uZone.InstanceManager.Server.cmdArgs[2]);
		UnityEngine.Debug.Log("Instance connected.");
		gameServerPort = uZone.InstanceManager.Server.gameServerPort;
		uLink.Network.InitializeServer(32, uZone.InstanceManager.Server.gameServerPort);
		//ServerRegistry.AddServer(gameServerIP, uZone.InstanceManager.Server.gameServerPort, numPlayers, maxPlayers, serverName);

		Lobby.AddListener(this);
		Lobby.ConnectAsServer(lobbyIP, lobbyPort);

		GameObject PickSelectionObj = GameObject.Find("Window_PickSelection");
		mainMenuManager.ShowMenu(windowPickSelection.GetComponent<Menu>());
		PickSelection pickSelScript = PickSelectionObj.GetComponent<PickSelection>();
		
		pickSelScript.CreateLayout();
		pickSelScript.CreatePickLayout();


	}

	// Randomises the number of players playing on the server and updates this information in the server registry.
	private void ChangeNumPlayers()
	{
		playersBlue = "";
		playersRed = "";
		for (int i = 0; i < playerDatabaseScript.PlayerList.Count; i++) 
		{
			if (playerDatabaseScript.PlayerList[i].playerTeam == "blue")
			{
				playersBlue += playerDatabaseScript.PlayerList[i].playerName + "\r\n";

			}
			if (playerDatabaseScript.PlayerList[i].playerTeam == "red")
			{
				playersRed += playerDatabaseScript.PlayerList[i].playerName + "\r\n";
				
			}
			
			
		}
		//numPlayers += (random.NextDouble() > 0.5 ? 1 : -1);
		//numPlayers = System.Math.Max(0, System.Math.Min(numPlayers, maxPlayers));

		// Updates the data associated with this server in the server registry. The clients will immediately receive the
		// updated data.
		//ServerRegistry.UpdateServerData(numPlayers, maxPlayers, serverName);

		//serverName = uZone.InstanceManager.Server.cmdArgs[1];
		//maxPlayers = Convert.ToInt32(uZone.InstanceManager.Server.cmdArgs[2]);
		ServerRegistry.UpdateServerData(numPlayers, maxPlayers, serverName, started, playersBlue, playersRed);

		Invoke("ChangeNumPlayers", 5);
	}

	//[PunRPC]
	//public void SendGameChatInput(string text)
	//{
	//	networkView.RPC("SendGameChatInputAll", uLink.RPCMode.All, text);
	//}
}
