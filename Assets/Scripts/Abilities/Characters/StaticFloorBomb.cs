using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class StaticFloorBomb : MobaStormAbility {
	
	private float blastRadius = 5;

	//CALLED WHEN THE OBJECT HAS RECIEVED ALL THE NETWORK DATA
	protected override void OnStart ()
	{
		originatorPlayerScript = originatorGameObj.GetComponent<PlayerStats>();
		originatorPlayerScript.abilityLocked = false;
	}

	//WHEN A PLAYER OR A CREEP ENTER THE BOMB RADIUS
	void OnTriggerStay(Collider other)
	{
		if (uLink.Network.isServer)
		{
			if (other.transform.tag == "CreepTrigger"|| other.transform.tag== "BlueTeamTriggerTag" && team == "red"|| other.transform.tag == "RedTeamTriggerTag" && team == "blue" || other.transform.tag == "RedPlayerTriggerTag" && team == "blue" || other.transform.tag == "BluePlayerTriggerTag" && team == "red")
			{
				
				//DEAL DMG TO A LIST OF COLLIDERS IN THE RANGE
				List<Collider> struckObjects = new List<Collider>(Physics.OverlapSphere(this.transform.position, blastRadius));
				
				foreach(Collider objectsHit in struckObjects)
				{
					//USED TO DEAL DMG TO THE TRIGGER TARGET
					MobaDealDmgTriggerObj(objectsHit.gameObject);
				
				}
				
				uLink.Network.Destroy(this.gameObject);


			}
		}
	}

	protected override IEnumerator DestroyMyselfAfterSomeTime() 
	{
		yield return new WaitForSeconds(expireTime);	
		
		if (explotionObj) {
			Instantiate(explotionObj, transform.position, Quaternion.identity);
		}
		
		uLink.Network.Destroy(gameObject);
	}


}
