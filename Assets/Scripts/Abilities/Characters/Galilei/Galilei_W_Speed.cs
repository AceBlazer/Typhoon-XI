using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

//Custom character spell
public class Galilei_W_Speed : MobaStormAbility {
	
	//Called when script has recieved all network data
	protected override void OnStart ()
	{
		if (uLink.Network.isServer) {
			originatorPlayerScript = originatorGameObj.GetComponent<PlayerStats>();
			originatorPlayerScript._adValueAdd+= 50;
			originatorPlayerScript.UpdateAdds();
			PlayerControllerRTS originatorControllerScript = originatorGameObj.GetComponent<PlayerControllerRTS>();
			originatorControllerScript.playerStatus = PlayerControllerRTS.PlayerState.idle;
			originatorPlayerScript.abilityLocked = false;
		}
	}

	void Update () 
	{
		if (!spellActive)
			return;

		//This spell follows the position of the player originator
		Vector3 pos = originatorGameObj.transform.position;

		transform.position = pos;

	}
	
	public void OnDestroy () {
		if (uLink.Network.isServer)
		{
			originatorPlayerScript._adValueAdd-= 50;
			originatorPlayerScript.UpdateAdds();
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
