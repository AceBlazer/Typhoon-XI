using UnityEngine;

/// Implements a uLobby lobby instance. It listens for when accounts log in and sends a list of all logged in accounts
/// to all peers.
public class LobbySceneLoader : MonoBehaviour
{



	public void Start()
	{
		Application.LoadLevel("LobbyScene");

	}


}
