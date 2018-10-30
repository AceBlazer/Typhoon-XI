using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

//Custom character spell
public class AoeExplotion : MobaStormAbility {
	private RaycastHit rocketHit;
	private float range = 0.5f;
	private float blastRadius = 3;
	//Called when script has recieved all network data
	protected override void OnStart ()
	{
		if (uLink.Network.isServer) {
			originatorPlayerScript = originatorGameObj.GetComponent<PlayerStats>();
			originatorPlayerScript.abilityLocked = false;
		}
		if (explotionObj) {
			Instantiate(explotionObj, transform.position, Quaternion.identity);
		}
	}
	
	void Update () 
	{
		if (!spellActive)
			return;
		
		//This spell follows the position of the player originator
		Vector3 pos = originatorGameObj.transform.position;
		
		transform.position = pos;

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
	
	public void OnDestroy () {

	}
	
	protected override IEnumerator DestroyMyselfAfterSomeTime() 
	{
		yield return new WaitForSeconds(expireTime);	
		if (uLink.Network.isServer) {
			
			uLink.Network.Destroy(gameObject);
		}
	}
}
