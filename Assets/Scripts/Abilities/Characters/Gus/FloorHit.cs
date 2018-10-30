using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class FloorHit : MobaStormAbility {
	
	private float blastRadius = 2;
	private RaycastHit rocketHit;
	
	//CALLED WHEN THE OBJECT HAS RECIEVED ALL THE NETWORK DATA
	protected override void OnStart ()
	{
		originatorPlayerScript = originatorGameObj.GetComponent<PlayerStats>();
		originatorPlayerScript.abilityLocked = false;
		if (explotionObj) {
			Instantiate(explotionObj, transform.position, Quaternion.identity);
		}
	}

	void Update ()
	{
		if (uLink.Network.isServer && !expended)
		{
			expended = true;
			
			List<Collider> struckObjects = new List<Collider>(Physics.OverlapSphere(rocketHit.point, blastRadius));
			
			foreach(Collider objectsHit in struckObjects)
			{
				//Used to deal dmg on the target
				MobaDealDmgTriggerObj(objectsHit.gameObject);

			}
		}
	}

	
	protected override IEnumerator DestroyMyselfAfterSomeTime() 
	{
		yield return new WaitForSeconds(expireTime);	
		if (uLink.Network.isServer) {
			
			uLink.Network.Destroy(gameObject);
		}
	}
	
	
}