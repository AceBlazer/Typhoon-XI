using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameEvents : uLink.MonoBehaviour {

	public List<CharSlainPortrait> charPortraits = new List<CharSlainPortrait>();

	public List<SlainEvent> slainEventList = new List<SlainEvent>();

	[System.Serializable]
	public class CharSlainPortrait {

		public CharacterClass.charName charName;
		//public Sprite slainPortrait;
		public Sprite charPortrait;
	}

	[System.Serializable]
	public class SlainEvent {
		public string killerName;
		public string slainName;
		public Sprite killerSprite;
		public Sprite slainSprite;

		public SlainEvent Constructor ()
		{
			SlainEvent capture = new SlainEvent ();
			capture.killerName = killerName;
			capture.slainName = slainName;
			capture.killerSprite = killerSprite;
			capture.slainSprite = slainSprite;
			
			return capture;
		}

	}
	
	public CanvasGroup slainCanvas;
	public CanvasGroup spellUpCanvas;
	public Text killerText;
	public Text slainText;
	public AudioClip slainSound;
	public AudioClip greetingsClip;
	public Image killerImage;
	public Image slainImage;

	public bool showSlainUi = false;

	public bool showItemStore;
	public bool useItemSlot;
	public int slotNumberUsed;
	//Variables Assigned from the ItemManager Script attached to the players
	[HideInInspector] public CanvasGroup itemCanvas;
	[HideInInspector] public GameObject tooltipObj;
	[HideInInspector] public CanvasGroup tooltipCanvas;
	[HideInInspector] public RectTransform tooltipRect;


	//THIS IS THE REFERENCE TO THE PLAYERSTATS SCRIPT
	//THIS IS ACCESED FROM THE PLAYERSTATS SCRIPT OWNER
	[HideInInspector] public PlayerStats playerStScript;

	private Text tooltipText;
	private Text tooltipTextVisual;

	private AudioSource audioSource;
	public AudioClip buttonClick1;
	public AudioClip buttonClick2;
	public AudioClip windowSlide;
	public AudioClip readyButton;

	public void PlayGreetingsAudio () {
		//StartCoroutine(GreetingsAudio());
		
	}

	public IEnumerator GreetingsAudio () {
		yield return new WaitForSeconds(2);
		audioSource.PlayOneShot(greetingsClip);
		
	}
	public void ButtonClick1Audio () {
		audioSource.PlayOneShot(buttonClick1);
		                      
	}
	public void ButtonClick2Audio () {
		audioSource.PlayOneShot(buttonClick2);

	}
	public void WindowSlideAudio () {
		audioSource.PlayOneShot(windowSlide);
	}
	public void ReadyButtonAudio () {
		audioSource.PlayOneShot(readyButton);
	}

	public void ShowItemStore () {
		itemCanvas.alpha = 1;
		itemCanvas.interactable = true;
		showItemStore = false;
	}
	public void ShowSpellUpButtons () {
		spellUpCanvas.alpha = 1;
		spellUpCanvas.interactable = true;
	}

	public void UseItemSlot (int value) {
		useItemSlot = true;
		slotNumberUsed = value;
	}
	public void UpgradeSpell (string value) {
		if (value == "q")
			playerStScript.UpgradeAbility(PlayerControllerRTS.PlayerState.attackingQ, true);

		if (value == "w")
			playerStScript.UpgradeAbility(PlayerControllerRTS.PlayerState.attackingW, true);

		if (value == "e")
			playerStScript.UpgradeAbility(PlayerControllerRTS.PlayerState.attackingE, true);

		if (value == "r")
			playerStScript.UpgradeAbility(PlayerControllerRTS.PlayerState.attackingR, true);

			}

	public void ShowToolTip (string name, string desc, int cost, float x, float y) {
		tooltipCanvas.alpha = 1;

		string textfinal = string.Format("<color=white>" + name + "</color> \n <color=lightblue>"+ desc + " </color> \n \n <color=white>Cost: </color><color=yellow>  "+ cost + " </color>");
		textfinal = textfinal.Replace("NEWL","\n");
		tooltipText.text = textfinal;
		tooltipTextVisual.text = textfinal;
		tooltipRect.transform.localPosition = new Vector3(x, y-60, 0);

	}

	public void HideToolTip () {
		tooltipCanvas.alpha = 0;
	}


	void AddCaptureSlainEvent(CharacterClass.charName charKiller, string killerName, CharacterClass.charName charSlain, string slainName)
	{
		SlainEvent capture = new SlainEvent ();
		capture.killerName = killerName;
		capture.slainName = slainName;

		for (int i=0; i< charPortraits.Count; i++)
		{
			
			if (charPortraits[i].charName == charKiller)
			{
				capture.killerSprite = charPortraits[i].charPortrait;
			}
			
			if (charPortraits[i].charName == charSlain)
			{
				capture.slainSprite = charPortraits[i].charPortrait;
			}
			
			killerText.text = killerName;
			slainText.text = slainName;
			
		}

		slainEventList.Add(capture);
	}

	void Start () {
	
		tooltipObj = GameObject.Find("TooltipText");
		tooltipTextVisual = GameObject.Find("TooltipTextVisual").GetComponent<Text>();
		tooltipRect = GameObject.Find("TooltipText").GetComponent<RectTransform>();
		tooltipCanvas = tooltipObj.GetComponent<CanvasGroup>();
		tooltipText = tooltipObj.GetComponent<Text>();
		audioSource = GetComponent<AudioSource>();


	}
	
	// Update is called once per frame
	void Update () {



		if (showSlainUi == true)
		{
			if (slainCanvas.alpha<1)
			slainCanvas.alpha = slainCanvas.alpha + 0.1f;
		}
		else
		{
			if (slainCanvas.alpha >0)
			slainCanvas.alpha = slainCanvas.alpha - 0.1f;
		}

		if (slainEventList.Count >0 && showSlainUi == false && slainCanvas.alpha < 0.2f)
		{
			StartCoroutine(ShowPlayerSlain(slainEventList[0].killerSprite, slainEventList[0].killerName, slainEventList[0].slainSprite, slainEventList[0].slainName));

		}


	}

	public void SendSlainAlert(CharacterClass.charName charKiller, string killerName, CharacterClass.charName charSlain, string slainName)
	{
		networkView.RPC("SendSlainAlertToClient", uLink.RPCMode.Others, charKiller, killerName, charSlain, slainName);

	}

	[PunRPC]
	public void SendSlainAlertToClient(CharacterClass.charName charKiller, string killerName, CharacterClass.charName charSlain, string slainName)
	{
		AddCaptureSlainEvent(charKiller, killerName, charSlain, slainName);

	}



	public IEnumerator ShowPlayerSlain (Sprite killerSprite, string killerName, Sprite slainSprite, string slainName) {

		GetComponent<AudioSource>().PlayOneShot(slainSound);

		showSlainUi = true;

		killerImage.sprite = killerSprite;
		
		slainImage.sprite = slainSprite;
			
		killerText.text = killerName;
			
		slainText.text = slainName;

		slainEventList.RemoveAt(0);

		yield return new WaitForSeconds(8f);

		showSlainUi = false;
	}
}
