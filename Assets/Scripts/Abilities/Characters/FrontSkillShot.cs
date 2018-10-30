using UnityEngine;
using System.Collections;

//Custom character spell
public class FrontSkillShot : MobaStormAbility {

	public float spellSpeed = 10;
	private RaycastHit hit;
	public float range = 2f;

	//Called when script has recieved all network data
	protected override void OnStart ()
	{
		originatorPlayerScript = originatorGameObj.GetComponent<PlayerStats>();
		originatorPlayerScript.abilityLocked = false;
	}
	
	public void OnDestroy () {

	}

	void Update () 
	{
		

	}

	void FixedUpdate()
	{
		if (expended)
		{
			networkView.RPC("LaunchExplotion", uLink.RPCMode.Others);
			uLink.Network.Destroy(this.gameObject);

		}
		
		if (!spellActive)
			return;
		
		transform.Translate(Vector3.forward * spellSpeed * Time.deltaTime);
		//Raycast to the vector.forward to detect collisions with enemies using layermask mask
		if (Physics.Raycast(transform.position, transform.forward, out hit, range, mask) && expended == false && uLink.Network.isServer && !expended)
		{		
			//Used to deal dmg on the target
			MobaDealDmgTriggerObj(hit.transform.gameObject);
		}
	}

	protected override IEnumerator DestroyMyselfAfterSomeTime() 
	{
		yield return new WaitForSeconds(expireTime);	
		if (uLink.Network.isServer) {	
			uLink.Network.Destroy(gameObject);
		}
	}

	/// Updates the data on clients.
	[PunRPC]
	private void LaunchExplotion()
	{
		//if (uLink.Network.isServer)
		//networkView.RPC("LaunchExplotion", uLink.RPCMode.Others);
		//else
		Instantiate(explotionObj, transform.position, Quaternion.identity);
	}


}
