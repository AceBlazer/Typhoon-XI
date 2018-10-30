using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Char_Pick : MonoBehaviour {
	public Button charButton;
	public AudioClip pickSound;
	public int maxSize;
	public CharacterClass.charName charName;
	private GameObject playerPickObj;
	private PickSelection playerPickScript;
	// Use this for initialization
	
	void Start ()
	{
		playerPickObj = GameObject.Find("Window_PickSelection_mn");
		playerPickScript	= playerPickObj.GetComponent<PickSelection>();
	}
	// Update is called once per frame
	void Update () {
	
	}
	public void OnClick()
	{
	
		if (!uLink.Network.isServer)
		{
		GetComponent<AudioSource>().PlayOneShot(pickSound, 0.5F);
		playerPickScript.AddChar((int)charName);
		}

		
	}

}
