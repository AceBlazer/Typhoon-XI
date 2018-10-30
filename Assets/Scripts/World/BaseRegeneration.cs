using UnityEngine;
using System.Collections;
//SCRIPT ATTACHED TO THE BASE GAMEOBJ
public class BaseRegeneration : uLink.MonoBehaviour
{
	//HEAL OBJ
	public GameObject HealOverTimeObj;
	//BASE TEAM
	public string myTeam = "blue";
	[HideInInspector] public GameObject targetObj;
	
	void OnTriggerStay(Collider other)
	{

		if (uLink.Network.isServer)
		{
			if (myTeam == "blue")
			{

				if(other.tag == "BluePlayerTag")
				{			
					targetObj = other.gameObject;
					if (!targetObj.GetComponent<PlayerStats>().hotObj)
					{
						uLink.Network.Instantiate(uLink.NetworkPlayer.server, HealOverTimeObj,HealOverTimeObj,HealOverTimeObj, targetObj.transform.position, Quaternion.identity, 0,
						                                                             myTeam, gameObject.name, targetObj.transform.position, targetObj.name, 20f, 20f);
					}
				}
			}
			else
			{
				if(other.tag == "RedPlayerTag")
				{			
					targetObj = other.gameObject;
					if (!targetObj.GetComponent<PlayerStats>().hotObj)
					{
						uLink.Network.Instantiate(uLink.NetworkPlayer.server, HealOverTimeObj,HealOverTimeObj,HealOverTimeObj, targetObj.transform.position, Quaternion.identity, 0,
						                                                             myTeam, gameObject.name, targetObj.transform.position, targetObj.name, 20f, 20f);
					}
					
				}
			}
		}
		
	}








}
