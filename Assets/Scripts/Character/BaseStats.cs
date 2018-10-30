using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using uLink;

[System.Serializable]
public class BaseStats : uLink.MonoBehaviour {

	public CharacterClass stats = new CharacterClass ();
	//HEALTH VARIABLES
	public float myHealth 			= 100;
	public float maxHealth			= 100;
	public float healthRegenerate 	= 1.3f;

	//MANA VARIABLES
	public float mana;
	public float baseMana 			= 100;
	public float rechargeRateMana 	= 5f;


	//PLAYER VARIABLES
	public CharacterClass.charName charName;
	public uLink.NetworkPlayer networkOwner;
	public bool isPlayer = false;
	public bool isCreator = false;	
	public PlayerControllerRTS playerCScript;
	public string playerTeam;
	public int playerScore;
	public int playerLvl 			= 1;
	public float playerExp;
	public float maxPlayerExp 		= 250;
	public int kills;
	public int deaths;
	public int assist;
	public float gold;
	public float rechargeRateGold 	= 1f;
	public float adResAdd;
	public float apResAdd;
	public float speedAdd;
	public float attackRedAdd;
	public float cdRedAdd;
	public float _adValueAdd;
	public float _apValueAdd;
	public GameObject hotObj;
	public GameObject dotObj;
	public GameObject stunObj;
	public GameObject slowObj;
	public GameObject recallObj;
	public bool recalling;
	//how much time players will stay on the attackers list of the damaged enemy
	public List<AttackersDataClass> attackersList = new List<AttackersDataClass>();
	
	//GENERAL VARIABLES
	public float _bAdValue;
	public float _bApValue;
	public bool destroyed;
	public float expToGive 		= 40;
	public float goldToGive 	= 150;
	public bool respawns 		= false;
	public float respawnTime 	= 10;
	public float adRes;
	public float apRes;
	public float speed 			= 1.4f;
	public NavMeshAgent agent;

	//USED WHEN AN ABILITY OR SOMETHING OVERRIDE THE MOVEMENTS OR ANIMATION OF THE PLAYER
	public bool charLocked = false;
	public bool abilityLocked = false;

	//REFERENCE GAMEOBJ
	public GameObject gameManager;
	public PlayerDatabase dataScript;
	public GameObject trigger;

	//THIS SCRIPT IS USED UPDATE EVENTS FOR THE PLAYER LEVELING
	public GameEvents gameEventScript;

	//GAMEOBJ THAT SHOWS THE DAMAGE POP UP
	public	GameObject uiDamageDealt;

	public void UpdateRegeneration()
	{
		if (!destroyed)
		{
			//REGENERATING MANA
			if(mana < baseMana){
				mana = mana + rechargeRateMana * Time.deltaTime;	
			}
			if(mana > baseMana) {
				mana = baseMana;	
			}
			if(mana < 0) {
				mana = 0;	
			}
			
			//REGENERATING HEALTH
			if (maxHealth > myHealth && myHealth > 0) {
				myHealth = myHealth + healthRegenerate * Time.deltaTime;
			}
			//IF THE PLAYER HEALTH EXCEEDS THE MAXHEALTH THEN SET IT BACK TO THE MAX HEALTH
			if (myHealth > maxHealth) {
				myHealth = maxHealth;
			}
			//INCREASE THE GOLD OVERTIME
			gold = gold + rechargeRateGold  * Time.deltaTime;
		}
	}

	//USED TO DRAIN HEALTH ON ANY PLAYER OR ENEMYS
	public void DrainHealth (float _ad, float _ap, string attacker, GameObject originatorObj, string originatorTag, CharacterClass.charName originatorCharName)
	{
		if (!destroyed)
		{
			
			myHealth = myHealth - GetFinalDmg(_ad, _ap, adRes + adResAdd, apRes + apResAdd);
			
			if (originatorObj.tag == "RedPlayerTag" || originatorObj.tag == "BluePlayerTag")
			{
				AddCaptureAttackers(attacker,originatorObj, Time.time);
				
				
			}
			
			UpdateHealthClients();
			
			
			
		}
		
		if (myHealth <= 0 && destroyed == false && uLink.Network.isServer == true)
		{
			myHealth = 0;
			destroyed = true;
			
			for(int i = 0; i < dataScript.PlayerList.Count; i++)
			{
				
				if(dataScript.PlayerList[i].playerName == attacker)
				{
					
					networkView.RPC("ShowGoldUi", dataScript.PlayerList[i].networkPlayer, this.transform.name, attacker, goldToGive, true);
					
				}
				
				
			}
			
			for (int i = 0; i < attackersList.Count; i++) // Loop with for.
			{
				attackersList[i]._Obj.GetComponent<PlayerStats>().UpdateExpToClients(expToGive / attackersList.Count);
				attackersList[i]._Obj.GetComponent<PlayerStats>().UpdateGoldToClients(goldToGive / attackersList.Count);
			}
			
			if (gameObject.tag == "RedPlayerTag" || gameObject.tag == "BluePlayerTag")
			{
				//call the method SendSlainAlert(); on GAME MANAGER
				CharacterClass.charName thisCharName = GetComponent<PlayerStats>().charName;
				
				gameManager.GetComponent<GameEvents>().SendSlainAlert(originatorCharName, attacker, thisCharName, this.gameObject.name);
				
			}
			
			if (originatorTag == "RedPlayerTag" || originatorTag == "BluePlayerTag")
			{
				
				//If the reciever  is a player 
				if (gameObject.tag == "RedPlayerTag" || gameObject.tag == "BluePlayerTag")
				{
					
					dataScript.EditPlayerListWithScore(attacker);
					
					PlayerStats originatorPlayerScript = originatorObj.GetComponent<PlayerStats>();
					originatorPlayerScript.kills ++;
					if (originatorPlayerScript.playerExp >= originatorPlayerScript.maxPlayerExp)
					{
						originatorPlayerScript.ShowAbilityUpUi();
						originatorPlayerScript.playerLvl++;
						originatorPlayerScript._bAdValue += 5;
						originatorPlayerScript._bApValue += 5;
						originatorPlayerScript.maxHealth += 25;
						originatorPlayerScript.playerExp = 0;
						originatorPlayerScript.UpdateStatsOnClients();
						
					}
					
					
					
				}
				else
					//If the  reciever is not a player
				{
					//dataScript.EditPlayerListWithScore(attacker, 1);
					PlayerStats originatorPlayerScript = originatorObj.GetComponent<PlayerStats>();
					
					if (originatorPlayerScript.playerExp >= originatorPlayerScript.maxPlayerExp)
					{
						originatorPlayerScript.ShowAbilityUpUi();
						originatorPlayerScript.playerLvl++;
						originatorPlayerScript._bAdValue += 8;
						originatorPlayerScript._bApValue += 8;
						originatorPlayerScript.maxHealth += 25;
						originatorPlayerScript.playerExp = 0;
						originatorPlayerScript.UpdateStatsOnClients();
						
					}
				}
				
			}
			
			
			
			networkView.RPC("ImDead", uLink.RPCMode.All);
			
		}
	}


	//ADD THE CURRENT ATTACKER NAME TO THE LIST attackersList
	void AddCaptureAttackers(string attackerTag, GameObject originatorObj, float time)
	{
		for (int i = 0; i < attackersList.Count; i++) // Loop with for.
		{
			if (attackersList[i]._Name == attackerTag)
			{
				RemoveCaptureAttackers(i);
			}

		}
		AttackersDataClass capture = new AttackersDataClass ();
		capture._Name = attackerTag;
		capture._Obj = originatorObj;
		capture._time = time;
		attackersList.Add(capture);
		
	}
	//REMOVES THE CURRENT ATTACKER INDEX ON THE LIST attackersList
	void RemoveCaptureAttackers(int index)
	{
		attackersList.RemoveAt(index);
	}



	//ONLY RUNNING ON THE SERVER
	//USED TO SEND A RPC TO THE CLIENTS WITH THE HEALTH
	public void UpdateHealthClients ()
	{
		if (uLink.Network.isServer)
		{
			networkView.RPC("UpdateClientHealth", uLink.RPCMode.Others, myHealth, maxHealth);
		}
		
	}

	public void UpdateHealthClients (float healthToAdd)
	{
		if (uLink.Network.isServer)
		{
			networkView.RPC("UpdateClientHealth", uLink.RPCMode.Others, myHealth, maxHealth);
		}
		
	}



	//CALCULATE THE TOTAL DMG
	private float GetFinalDmg (float adDmg, float apDmg, float adRes, float apRes)
	{
		float dmgTotal = 0f;
		float dmgTotalAd = 0f;
		float dmgTotalAp = 0f;

		
		dmgTotalAd = adDmg - adRes;
		dmgTotalAp= apDmg - apRes;
		if (dmgTotalAd<0)
			dmgTotalAd = 0;
		if (dmgTotalAp<0)
			dmgTotalAp = 0;
		
		dmgTotal = dmgTotalAd + dmgTotalAp;
		
		
		return dmgTotal;
	}

}

	