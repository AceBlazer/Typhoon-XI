using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using uLobby;

public class LogInManager : MonoBehaviour {
	private string user;
	private string pass;

	public string regUser;
	public string regPass;
	public string regPassConfirm;

	public string email;
	private int win;
	private int lose;

	//public Button connectButton;
	private bool connected = false;

	public GameObject clientObj;
	//public GameObject mainMenu;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void LogUserTxt (string userInput)
	{
		user = userInput;
		//sliderText.text = sliderTextString;
	}
	public void LogPassrTxt (string passInput)
	{
		pass = passInput;
		//sliderText.text = sliderTextString;
	}
	public void regUserTxt (string regUserInput)
	{
		regUser = regUserInput;
		//sliderText.text = sliderTextString;
	}
	public void RegPassrTxt (string regPassInput)
	{
		regPass = regPassInput;
		//sliderText.text = sliderTextString;
	}
	public void RegConfirmPassrTxt (string regPassInput)
	{
		regPassConfirm = regPassInput;
		//sliderText.text = sliderTextString;
	}
	public void RegEmail (string regEmailInput)
	{
		email = regEmailInput;
		//sliderText.text = sliderTextString;
	}
	public void DoLogin()
	{
		StartCoroutine(TimeOut());
		Debug.Log("dologin");
		//clientObj.GetComponent<Client>().ConnectToLobby();
		Lobby.RPC("AccountInfoToServer", LobbyPeer.lobby, user, pass, Lobby.peer);
		this.gameObject.GetComponent<MenuManager>().LoadingScreens(true);
	}

	public void DoRegister()
	{
		
		//clientObj.GetComponent<Client>().ConnectToLobby();
		Lobby.RPC("RegInfoToServer", LobbyPeer.lobby, regUser, regPass, regPassConfirm, email, Lobby.peer);
		this.gameObject.GetComponent<MenuManager>().LoadingScreens(true);
	}

	IEnumerator TimeOut()
	{
		yield return new WaitForSeconds(5f);
		if (!connected)
		{
			this.gameObject.GetComponent<MenuManager>().LoadingScreens(false);

			//clientObj.GetComponent<Client>().windowLogin.GetComponent<Menu>().errorDialogText.color = Color.red;
			//clientObj.GetComponent<Client>().windowLogin.GetComponent<Menu>().errorDialogText.text = "Cant connect to the lobby. Try again";
		}

	} 

	private void uLobby_OnConnected()
	{
	
		connected = true;
	}


}
