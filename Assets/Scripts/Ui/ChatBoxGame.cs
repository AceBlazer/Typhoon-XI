using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using uLobby;

public class ChatBoxGame : uLink.MonoBehaviour {

	public string chatText;
	public Text chatBoxText;
	public InputField ChatInputField;
	private string[] colorNames = new string[] {"cyan", "green", "lightblue","olive", "orange", "purple","red", "silver", "teal","yellow"};
	private int index = 0;
	private string userNameColor;
	//private Color 
	// Use this for initialization
	void Start () {
	
	}
	public void addText(string text)
	{

		chatText = text;

	}


	// Update is called once per frame
	void Update () {
		if (ChatInputField.GetComponent<InputField>().isFocused == true) {
			if (Input.GetKeyDown(KeyCode.Return))
			{

				ChatInputField.text = ChatInputField.text.Remove(ChatInputField.text.Length - 1);
				networkView.RPC("SendGameChatInput", uLink.RPCMode.Server, "<color="+ userNameColor+ ">" + PlayerPrefs.GetString("playername") +" -</color> "  + ChatInputField.text);
				ChatInputField.text = "";
			}
		}
	}

	void uLink_OnPlayerConnected(uLink.NetworkPlayer networkPlayer) 
	{
		string colorName = colorNames[index];
		index++;

		networkView.RPC("SetPlayerColor", networkPlayer, colorName);
	}

	[PunRPC]
	public void SendGameChatInput(string text)
	{
		if (uLink.Network.isServer)
		{
			networkView.RPC("SendGameChatInput", uLink.RPCMode.Others, text);
		}
		else
		{
			chatBoxText.text += text + "\r\n";
		}
	}
	[PunRPC]
	public void SetPlayerColor(string text)
	{
		userNameColor = text;
	}



}
