using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uLobby;
using UnityEngine.UI;

public class ServerListObj : MonoBehaviour {
	public ServerInfo server; 
	//public string Ip;
	public int players;
	public Text textBoxName;
	public Text playersBLueText;
	public Text playersRedText;
	public GameObject clientObj;
	// Use this for initialization
	void Start () {
		UpdateData();
		clientObj = GameObject.Find("ClientGameObj");
	}

 	public void ConnectToIp () {

		clientObj.GetComponent<Client>().ConnectToIp(server.host, server.port);
	}
	// Update is called once per frame
	void UpdateData () {

		uLink.BitStream dataCopy = server.data;
		
		int numPlayers = dataCopy.ReadInt32();
		int maxPlayers = dataCopy.ReadInt32();
		string serverName = dataCopy.ReadString();
		bool started = dataCopy.ReadBoolean();
		string playersBlue = dataCopy.ReadString();
		string playersRed = dataCopy.ReadString();
		playersBLueText.text = playersBlue;
		playersRedText.text = playersRed;
		players = numPlayers;
		//Ip = server.endpoint.ToString();

		textBoxName.text = server.host + "\r\n" + numPlayers + " / " + maxPlayers;

		Invoke("UpdateData", 5);
	}

}
