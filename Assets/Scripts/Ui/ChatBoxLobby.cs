using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using uLobby;

public class ChatBoxLobby : MonoBehaviour {

	public string chatText;
	public Text chatBoxText;
	public InputField ChatInputField;

	// Use this for initialization
	void Start () {
	
	}
	public void addText(string text)
	{
	
		chatText = text;
	
	}

	public void SendToServer()
	{
	
		Lobby.RPC("SendChatInput", LobbyPeer.lobby, "<color=yellow>" + PlayerPrefs.GetString("playername") +" -</color> "  + chatText, Lobby.peer);
		ChatInputField.text = "";
		
	}

	// Update is called once per frame
	void Update () {
		if (ChatInputField.GetComponent<InputField>().isFocused == true) {

		}
	}


}
