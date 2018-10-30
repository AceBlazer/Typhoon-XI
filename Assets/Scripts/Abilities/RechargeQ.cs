using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the RechargeHealthPickup. When a player
/// walks into this object they are given some health and the pickup 
/// dissappears for some duration.
/// 
/// This script accesses the player's HealthAndDamage script to 
/// increase their health.
/// </summary>

public class RechargeQ : uLink.MonoBehaviour {
	
	//Variables Start_________________________________________________________	
	
	private bool taken = false;
	
	public float reSpawnTime = 20;
	
	private float rotateSpeed = 1;

	public bool thisRespawn = false;

	public PlayerControllerRTS.PlayerState abilityToLearn = PlayerControllerRTS.PlayerState.attackingQ;
	//Variables End___________________________________________________________
	
		
	
	// Update is called once per frame
	void Update () 
	{
		//Make the RechargeHealthPickup constantly rotate.
		
		transform.Rotate(Vector3.up * (rotateSpeed + Time.deltaTime));
	}
	
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "BluePlayerTriggerTag" && taken == false || other.tag == "RedPlayerTriggerTag" && taken == false)
		{			
			//HealthAndDamage HDScript = other.GetComponent<HealthAndDamage>();

			//PlayerSpells playerSpellScript = other.transform.parent.GetComponent<PlayerSpells>();

			PlayerStats playerStatsScript = other.transform.parent.GetComponent<PlayerStats>();

			
			//If health jumps up above max health then set it
			//back to the max health level.
			
		
			
			//Only the server deactivates and reactivates the pickup. It does this
			//across the network and uses buffered RPCs so players just joining can't
			//use the pickups.
			
			if(uLink.Network.isServer)
			{	
				//Debug.Log("colliotionb");
				//playerStatsScript.UpdateAbilityServer(abilityToLearn, true);
					
					//uLink.Network.Destroy(this.gameObject);
					networkView.RPC("DeactivateHealthPickup", uLink.RPCMode.All);
					
					StartCoroutine(ReSpawn());

					taken = true;
					
			
			}

		}
		
	}
	
	IEnumerator ReSpawn() 
	{	
		//After a certain duration make the cube visible again
		//and turn its light back on.
		if (!thisRespawn)
			uLink.Network.Destroy(this.gameObject);

        yield return new WaitForSeconds(reSpawnTime);
		
		networkView.RPC("ReactivateHealthPickup", uLink.RPCMode.All);
    }

	
	[PunRPC]
	void DeactivateHealthPickup ()
	{			
	//	Debug.Log("DEACTIVATE");
		//gameObject.GetComponent<ParticleSystem>().enableEmission = false;
		transform.GetComponent<Renderer>().enabled = false;
		transform.GetComponent<Collider>().enabled = false;
		transform.GetComponent<ParticleRenderer>().enabled = false;
		transform.GetComponent<Light>().enabled = false;
		
		taken = true;
	}
	
	
	[PunRPC]
	void ReactivateHealthPickup ()
	{
		taken = false;
		
		transform.GetComponent<Renderer>().enabled = true;
		transform.GetComponent<Collider>().enabled = true;
		transform.GetComponent<ParticleRenderer>().enabled = true;
		transform.GetComponent<Light>().enabled = true;
	}
}
