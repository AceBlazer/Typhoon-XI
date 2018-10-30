using UnityEngine;
using System.Collections;

public class PlayerPick : uLink.MonoBehaviour {

	public int currentSlot;
	public PickSelection pickSelection;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//pickSelection.AddChar(gameObject.GetComponent<Char_Pick>(), currentSlot);
	}

	public void SelectChar(Char_Pick character)
	{
		if (uLink.Network.isClient)
		{
			//pickSelection.AddChar(character, currentSlot);
		//networkView.RPC("UpdateCharPick", uLink.RPCMode.Server, character);
		}
			//pickSelection.AddChar(character, currentSlot);
	}

	[PunRPC]
	void UpdateCharPick(Char_Pick character)
	{
		networkView.RPC("UpdateCharPick", uLink.RPCMode.Others, character);
		//pickSelection.AddChar(character, currentSlot);
	}
}
