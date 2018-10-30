using UnityEngine;
using System.Collections.Generic;
using uLobby;
using UnityEngine.UI;


public class Client : uLink.MonoBehaviour
{
	public string lobbyIP = "127.0.0.1";
	public int lobbyPort = 45000;
	public bool clientActive= false;

	public string serverPublicIp = "127.0.0.1";

	public List<Account> loggedInAccounts = new List<Account>();

	public GameObject windowInit;
	public GameObject windowLobby;
	public GameObject windowLogin;
	public GameObject windowReg;
	public GameObject windowServerCreation;
	public GameObject windowPickSelection;
	public GameObject windowGame;
	public MenuManager mainMenuManager;
	public Menu lobbyMenu;

	public GameObject layoutServers;
	public GameObject ServerRect;
	RectTransform serverRectTransform;
	public GameObject ServerBoxPrefab;

	public Text chatBoxText;



	private int serverRow;
	public List<GameObject> allServerslots;

	private string serverName;
	private string serverPlayers;

	private int attempts = 0;

	private GameObject particleCamObj;
	public AudioSource introSource;
	public void Start()
	{
		particleCamObj = GameObject.Find("CameraParticle");
		allServerslots = new List<GameObject>();
		serverRectTransform = ServerRect.GetComponent<RectTransform>();

	}
	//function called when the user connects to the lobby
	public void ConnectToLobby()
	{
		clientActive = true;
		Lobby.AddListener(this);
		Lobby.ConnectAsClient(lobbyIP, lobbyPort);
		introSource.Play();
	}

	public void UpdateServerName (string name)
	{
		serverName = name;
	}
	public void UpdatePlayers (string players)
	{
		serverPlayers = players;
	}
	public void CreateServer()
	{
		Lobby.RPC("CreateServer", LobbyPeer.lobby, serverName, serverPlayers, Lobby.peer);
		Invoke("ConnectToServer", 5);
		windowServerCreation.GetComponent<Menu>().errorDialogText.color = Color.green;
		windowServerCreation.GetComponent<Menu>().errorDialogText.text = "Creating a custom match...";
	}



	public void ConnectToServer()
	{

		IEnumerable<ServerInfo> servers = ServerRegistry.GetServers();

		foreach (ServerInfo server in servers)
		{
		
			uLink.BitStream dataCopy = server.data;
			//int numPlayers = dataCopy.ReadInt32();
			//int maxPlayers = dataCopy.ReadInt32();
			string name = dataCopy.ReadString();
		
			if (serverName == name)
			{
				attempts = 0;
				Debug.Log("CONNECTING TO SERVER" + server.host + " " + server.port);
				ConnectToIp(server.host, server.port);

			}


		}

		if (!uLink.Network.isServer && !uLink.Network.isClient)
		{
			if (attempts < 3)
			{
				attempts++;
				Invoke("ConnectToServer", 3);
			}
			else
			{
				attempts = 0;
				windowServerCreation.GetComponent<Menu>().errorDialogText.color = Color.red;
				windowServerCreation.GetComponent<Menu>().errorDialogText.text = "Error creating a custom match";
				mainMenuManager.loadingScreenObj.SetActive(false);
			}
		}

	}


	public void ConnectToIp(string ip, int port)
	{

		Debug.Log(ip);
		Debug.Log(port);
		uLink.Network.Connect(serverPublicIp, port);
		
		
	}

	public void uLink_OnConnectedToServer()
	{
		introSource.Stop();
		mainMenuManager.ShowMenu(windowPickSelection.GetComponent<Menu>());
		mainMenuManager.loadingScreenObj.SetActive(false);
		
		// GIVE THE NAME OF THE PLAYER TO THE PICK SELECTION WINDOW
		GameObject PickSelectionObj = GameObject.Find("Window_PickSelection_mn");
		
		PickSelection pickSelScript = PickSelectionObj.GetComponent<PickSelection>();
		
		pickSelScript.PlayerName = PlayerPrefs.GetString("playername");
		pickSelScript.CreateLayout();
		pickSelScript.CreatePickLayout();
	}

	public void uLink_OnDisconnectedFromServer()
	{
		Lobby.DisconnectImmediate();

		particleCamObj.SetActive(true);
		Debug.Log("disconected");
		mainMenuManager.ShowMenu(windowInit.GetComponent<Menu>());

		Lobby.RemoveListener(this);
		Application.LoadLevel("MainScene");
	}
	
	private void uLobby_OnConnected()
	{
		Debug.Log("Connected to lobby");



	}

	private void uLobby_OnDisconnected()
	{
		Debug.Log("Disconnected from lobby");
	}

	public void Update()
	{
		if (uLink.Network.isClient && introSource.volume > 0.20f)
		{
			introSource.volume -= 0.01f;
		}




		if (uLink.Network.isServer)
		{
			introSource.Stop();
		}

	}



	public void ShowServers()
	{
	
		for(int i = 0; i < allServerslots.Count; i++)
		{
		Destroy(allServerslots[i]);
		allServerslots.Remove(allServerslots[i]);
		}
			
			
		IEnumerable<ServerInfo> servers = ServerRegistry.GetServers();
	

		foreach (ServerInfo server in servers)
		{

			GameObject newServerBox = Instantiate(ServerBoxPrefab) as GameObject;
			RectTransform newServerBoxRect = newServerBox.GetComponent<RectTransform>();
			ServerListObj serverListScript = newServerBox.GetComponent<ServerListObj>();
			serverListScript.server = server;
			
			
			newServerBox.transform.SetParent(ServerRect.transform);
			
			newServerBoxRect.localPosition =  new Vector3(0, (-serverRow * 50), 0);
			newServerBoxRect.localScale = Vector3.one;
			
			allServerslots.Add(newServerBox);
			serverRow++;
			serverRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50 * serverRow);
			

		}
		serverRow = 0;
		Invoke("ShowServers", 15);
		
	}
	public void UpdateServers()
	{
		IEnumerable<ServerInfo> servers = ServerRegistry.GetServers();

		foreach (ServerInfo server in servers)
		{

			int index = allServerslots.FindIndex(item => item.GetComponent<ServerListObj>().server.host == server.host);
			if (index > 0) 
			{
				Debug.Log("Server exist");
				// element exists, do what you need
			}
			else
			{
				Debug.Log("Server dont exist");
				GameObject newServerBox = Instantiate(ServerBoxPrefab) as GameObject;
				RectTransform newServerBoxRect = newServerBox.GetComponent<RectTransform>();
				ServerListObj serverListScript = newServerBox.GetComponent<ServerListObj>();
				serverListScript.server = server;
				newServerBox.transform.SetParent(ServerRect.transform);
				newServerBoxRect.localPosition =  new Vector3(0, (-serverRow * 50), 0);
				newServerBoxRect.localScale = Vector3.one;
				allServerslots.Add(newServerBox);
				serverRow++;
				serverRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50 * serverRow);
			}
			
		
			
		}
		Invoke("UpdateServers", 10);
		
	}
	public void ShowGameMenu()
	{
		
		mainMenuManager.ShowMenu(windowGame.GetComponent<Menu>());
		
	}

	// Sent from the lobby whenever the list of connected accounts has changed. The "dummy" bool is a workaround hack
	// for a bug in uLink when sending lists of user-defined classes - this bug will be fixed shortly!
	[PunRPC]
	public void OnAccountListUpdated(Account[] loggedInAccounts, bool dummy)
	{
		this.loggedInAccounts = new List<Account>(loggedInAccounts);
	}

	[PunRPC]
	public void AccountInfoResponse(bool response, string playerName, string msj)
	{
		if (response == true) {
			PlayerPrefs.SetString("playername", playerName);
			Debug.Log(playerName);
			mainMenuManager.ShowMenu(windowLobby.GetComponent<Menu>());

			ShowServers();
			mainMenuManager.LoadingScreens(false);


		}
		else
		{


			mainMenuManager.LoadingScreens(false);
			windowLogin.GetComponent<Menu>().errorDialogText.color = Color.red;
			windowLogin.GetComponent<Menu>().errorDialogText.text = msj;

		}
	}
	[PunRPC]
	public void RegInfoResponse(bool response, string playerName, string msj)
	{
		Debug.Log(response);
		if (response == true) {
			PlayerPrefs.SetString("playername", playerName);
			mainMenuManager.ShowMenu(windowLogin.GetComponent<Menu>());
			windowLogin.GetComponent<Menu>().errorDialogText.text = msj;
			windowLogin.GetComponent<Menu>().errorDialogText.color = Color.green;

			mainMenuManager.LoadingScreens(false);
			
		}
		else
		{
		
			windowReg.GetComponent<Menu>().errorDialogText.text = msj;
			windowLogin.GetComponent<Menu>().errorDialogText.color = Color.red;
			mainMenuManager.LoadingScreens(false);

		}
	}

	[PunRPC]
	public void SendChatInputAll(string text, LobbyPeer peer)
	{
		Debug.Log(text);
		chatBoxText.text += text + "\r\n";
	}


}
