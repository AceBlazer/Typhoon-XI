using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// This script is attached to the player and it draws the healthbar of players above them.
/// This script accesses the PlayerStats script for 
/// determining the healthbar length.

public class PlayerLabel : uLink.MonoBehaviour {
	
	//Variables Start___________________________________
	
	//The health bar texture is attached to this in the inspector.
	
	public Image health;
	public Image mana;
	public Text lvlText;
	public Text nameText;
	private int lvl;
	private PlayerStats playerStatsScript;
	private float healthBarLength;
	private float manaBarLength;
	[HideInInspector] public string playerName;
	private PlayerDatabase playerDataScript;
	void Awake ()
	{
		//FIND GAMEMAGER AND ACCESS THE DMGDATABASE SCRIPT
		GameObject GameManager = GameObject.Find("GameManager_mn");
		playerDataScript = GameManager.GetComponent<PlayerDatabase>();

		playerStatsScript = GetComponent<PlayerStats>();			
	}

	// Update is called once per frame
	void Update () 
	{	
		lvl = playerStatsScript.playerLvl;
	
		
		if(playerStatsScript.myHealth <= 0)
		{
			healthBarLength = 0;	
		}
		else
		{
			healthBarLength = (playerStatsScript.myHealth / playerStatsScript.maxHealth);	


		}

		if(playerStatsScript.mana <= 0)
		{
			manaBarLength = 0;	
		}
		else
		{
			manaBarLength = (playerStatsScript.mana / playerStatsScript.baseMana);	

		}

		if (playerDataScript.playerTeam == playerStatsScript.playerTeam)
		{
			nameText.color = Color.white;
		}
		else
		{
			nameText.color = Color.red;
		}
		nameText.text = gameObject.name;
		//update the text lvlText with the actual player level
		lvlText.text = lvl.ToString();
		//update the health image fillamount with the actual player health
		health.fillAmount = healthBarLength;
		//update the mana image fillamount with the actual player mana
		mana.fillAmount = manaBarLength;

	}

}
