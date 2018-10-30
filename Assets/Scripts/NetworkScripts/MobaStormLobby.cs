using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using uLobby;
using uZone;
using System;
using System.IO;

/// Implements a uLobby lobby instance. It listens for when accounts log in and sends a list of all logged in accounts
/// to all peers.
public class MobaStormLobby : MonoBehaviour
{
	private int win = 0;
	private int lose = 0;

	public int listenPort = 45000;

	public string databaseIP;
	public int databasePort;

	//UZONE VARIABLES
	public string host = "127.0.0.1";
	public string port = "12345";
	public List<string> cmdArg = new List<string>();

	public List<string> playersConnected = new List<string>();

	public void StartLobby()
	{

	}
	void Awake()
	{
		uZone.InstanceManager.Initialise();	
	}

	public void Start()
	{
		Lobby.AddListener(this);
		
		// Initialize a lobby that can accept a maximum of 1024 connections.
		Lobby.InitializeLobby(1024, listenPort);
		
		// Add ourselves as listeners for when accounts log in or out.
		AccountManager.OnAccountLoggedIn += OnAccountLoggedIn;
		AccountManager.OnAccountLoggedOut += OnAccountLoggedOut;

		/*
		 * This is called to make the object subscribe to events
		 * and have its named callbacks executed. These are defined
		 * at the end of this script file.
		 */
		uZone.InstanceManager.AddListener(this.gameObject);
		
		//One way of defining a callback. This is equivalent to connectedCallbackTwo.
		EventCallback connectedCallbackOne =
			arg =>
		{
			string id = (string) arg;
			UnityEngine.Debug.Log("Registered with ID: !" + id);
		};
		
		//This sets the callback to be executed on the event where the application
		//successfuly connects to uZone to be connectedCallbackOne.
		uZone.InstanceManager.SetCallback(EventCode.UzoneConnected, connectedCallbackOne);
		
		//A second call will overwrite the previously assigned callback.
		uZone.InstanceManager.SetCallback(EventCode.UzoneConnected, connectedCallbackTwo);
		
		//Calling this method will remove whatever callback is currently set for this event.
		//Also note that this is global for the application, and not just for this object.
		uZone.InstanceManager.DeleteCallback(EventCode.UzoneConnected);

		uZone.InstanceManager.Connect(host, Int32.Parse(port));
	}

	void connectedCallbackTwo(object arg)
	{
		string id = (string) arg;
		UnityEngine.Debug.Log("Registered with ID: !" + id);	
	}

	void OnGUI()
	{

		//ULOBBY
		GUILayout.BeginArea(new Rect(Screen.width/2, 10, 300, 100));
		
		if (Lobby.isConnected)
			GUILayout.Label("Lobby initialized on port " + listenPort + ".");
		
		GUILayout.EndArea();

		//UZONE

		string status;
		string status2;
		
		if(uZone.InstanceManager.isConnected)
			status = "Connected";
		else
			status = "Disconnected";
		if(uZone.InstanceManager.isListening)
			status2 = "Listening for data ...";
		else
			status2 = "Idle";
		
		GUI.Box(new Rect(Screen.width/2, 60, 150, 80), "uZone Status");
		GUI.Label(new Rect(Screen.width/2, 80, 140, 20), status); 
		GUI.Label(new Rect(Screen.width/2, 100, 140, 25), status2);


	}

	public void DoLogin(string user, string pass, LobbyPeer peer)
	{

		Debug.Log("LOGGINN");
		WWWForm www = new WWWForm();
		www.AddField("user", user);
		www.AddField("pass", pass);
		
		WWW w = new WWW("www.mobastorm.dx.am/login.php", www);
		StartCoroutine(Login(w, peer, user));
	}
	
	IEnumerator Login(WWW w, LobbyPeer peer, string user)
	{
		Debug.Log("connecting");
		yield return w;
		Debug.Log("connected");
		if (w.error == null)
		{
			if (w.text == "login-SUCCESS")
			{
				Debug.Log("It worked ");
				Lobby.RPC("AccountInfoResponse", peer, true, user, "Login Success");
			}
			else
			{
				Debug.Log("Error: " + w.text.ToString());
				Lobby.RPC("AccountInfoResponse", peer, false, user, "Error: " + w.text.ToString());
			}
		}
	}
	
	public void DoRegister(string user, string password, string passConfirm, string email, LobbyPeer peer)
	{
		if (password != passConfirm)
		{
			Lobby.RPC("RegInfoResponse", peer, false, user, "Error: Password Confirmation Error");
			return;
		}

		if (!email.Contains("@"))
		{
			Lobby.RPC("RegInfoResponse", peer, false, user, "Error: Email format invalid");
			return;
		}

		Debug.Log("user" + user + "password" + password);

		WWWForm www = new WWWForm();
		www.AddField("user", user);
		www.AddField("pass", password);
		www.AddField("email", email);
		www.AddField("win", win);
		www.AddField("lose", lose);
		
		WWW w = new WWW("www.mobastorm.dx.am/register.php", www);
		StartCoroutine(Reg(w, peer, user));
	}
	
	IEnumerator Reg(WWW w, LobbyPeer peer, string user)
	{
		yield return w;
		if (w.error == null)
		{
			Lobby.RPC("RegInfoResponse", peer, true, user,w.text.ToString());
			Debug.Log( w.text.ToString());
		}
		else
		{
			Lobby.RPC("RegInfoResponse", peer, false, user, "Error: " + w.text.ToString());
			Debug.Log("Error: " + w.error.ToString());
		}
	}




	private void uLobby_OnLobbyInitialized()
	{
		Debug.Log("Lobby initialized");
	}

	private void OnAccountLoggedIn(Account account)
	{
		OnAccountListUpdated();
	}

	private void OnAccountLoggedOut(Account account)
	{
		OnAccountListUpdated();
	}

	private void OnAccountListUpdated()
	{
		// An account has logged in or out, send the list of all logged in accounts to all players. To simplify the
		// example the whole list is sent each time, although this could be optimised by only sending differences.
		Lobby.RPC("OnAccountListUpdated", Lobby.GetPeers(), AccountManager.Master.GetLoggedInAccounts().ToArray(), false);
	}


	[PunRPC]
	public void CreateServer(string name, string players, LobbyPeer peer)
	{
		Debug.Log("RPC recieved NAME" + name + "PLayers !" + players);

		cmdArg.Add(name);
		cmdArg.Add(players);
		uZone.InstanceManager.StartGameInstance("gameMobaStormInstance", cmdArg);
		cmdArg.Clear();
	}

	[PunRPC]
	public void AccountInfoToServer(string name, string password, LobbyPeer peer)
	{
		DoLogin(name, password, peer);
	}

	[PunRPC]
	public void RegInfoToServer(string user, string password, string passConfirm, string email, LobbyPeer peer)
	{
		DoRegister(user, password, passConfirm, email, peer);
	}

	[PunRPC]
	public void SendChatInput(string text, LobbyPeer peer)
	{
		Lobby.RPC("SendChatInputAll", Lobby.GetPeers(), text, Lobby.peer);
	}
}
