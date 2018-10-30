using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Char_Slot : MonoBehaviour {
	private Stack<Char_Pick> characters;
	public Text NameBox;
	public Text CharNameBox;
	public Text StatusBox;
	public RectTransform NameBoxRect; 
	public Button slotButton;
	public Text stackTxt;
	public int slotNumber;

	public Sprite slotEmpty;

	public uLink.NetworkPlayer networkOwner;
	public CharacterClass.charName charName;
	public bool isReady = false;
	public bool isEmpty = true;


	private GameObject playerPickObj;
	
	private PickSelection playerPickScript;
	// Use this for initialization
	void Start () {
		characters 				= new Stack<Char_Pick>();
		playerPickObj 			= GameObject.Find("Window_PickSelection_mn");
		playerPickScript		= playerPickObj.GetComponent<PickSelection>();


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangePortrait(string Pname,uLink.NetworkPlayer owner, Sprite neutral, bool empty)
	{
		if (empty)
		{

			networkOwner = owner;
			NameBox.text = "Free Slot";
			CharNameBox.text = "";
			StatusBox.text = "";
			ChangeSprite(slotEmpty);
			isEmpty = true;
		}
		else
		{
			CharNameBox.text = charName.ToString();	
			if (!isReady)
			{
			StatusBox.text = "Picking";
			}
			else
			{
			StatusBox.text = "Ready";
			}
			networkOwner = owner;
			NameBox.text = Pname;
			ChangeSprite(neutral);
			isEmpty= false;
		}

	}

	private void ChangeSprite(Sprite neutral)
	{
		GetComponent<Image>().sprite = neutral;

		//SpriteState st = new SpriteState();
		//st.highlightedSprite = highlight;
		//st.pressedSprite = neutral;

	}

	public void OnClick()
	{

		if (isEmpty)
		playerPickScript.ChangeSlot(slotNumber);
		
		
	}
}
