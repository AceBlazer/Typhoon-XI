using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//Contains a list of all characters
//This script is accesed by PlayerControllerRTS script to get all the data of characters available
//All the charlist parameters must be filled on the inspector
public class DmgDatabase : uLink.MonoBehaviour {

	//public List<CharacterClass> charList = new List<CharacterClass>();

	public	GameObject uiDamageDealt;
	
	void Start () {
	
	}

	public void ShowDmgLabelToAttacker(string attacker, string reciever, float totalDmg)
	{

		GameObject gameManager = GameObject.Find("GameManager_mn");
		
		PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
		
		for(int i = 0; i < dataScript.PlayerList.Count; i++)
		{

			
			if(dataScript.PlayerList[i].networkPlayer == uLink.Network.player)
			{
				
				if(dataScript.PlayerList[i].playerName == attacker)
				{
					Debug.Log("attacker");
					GameObject uiDamage = Instantiate(uiDamageDealt, new Vector3 (0,0,0), Quaternion.identity) as GameObject; 
					DamageDealtUI label = uiDamage.GetComponent<DamageDealtUI>();
					label.totalDmg = totalDmg;
					label.reciever = reciever;
					label.dealingDamage = true;
					
				}
				
				if(dataScript.PlayerList[i].playerName == reciever)
				{
					Debug.Log("reciever");
					GameObject uiDamage = Instantiate(uiDamageDealt, new Vector3 (0,0,0), Quaternion.identity) as GameObject; 
					DamageDealtUI label = uiDamage.GetComponent<DamageDealtUI>();
					label.totalDmg = totalDmg;
					label.reciever = reciever;
					label.dealingDamage = false;
					
				}
				
				
			}
		}

	}

	// Update is called once per frame
	void Update () {

	}


	//Returns all the data of the actual character name
	/*
	public CharacterClass GetCharacterData (CharacterClass.charName CharName)
	{
		CharacterClass data = new CharacterClass ();

		for(int i = 0; i < charList.Count; i++)
		{
			if (charList[i].characterName == CharName)
				data = charList[i];
		}
		

		
		return data;
	}
	*/

}
