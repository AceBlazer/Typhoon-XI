using UnityEngine;
using System.Collections;

//Heal over time script
//This heals the player while he stays on a friendly base
public class HotObjBase : MobaStormAbility {
	
	//Variables Start_________________________________________________________	
	

	public float HealthGain = 50;
	
	public float reSpawnTime = 20;
	
	private float rotateSpeed = 1;

	public bool thisRespawn = false;
	//Variables End___________________________________________________________
	
		
	protected override void OnStart ()
	{

		PlayerStats statsScript = destinationGameObj.transform.GetComponent<PlayerStats>();

		if (statsScript.hotObj)
		{
			uLink.Network.Destroy(statsScript.hotObj);
			statsScript.hotObj = this.gameObject;

		}
		else
		{
			statsScript.hotObj = this.gameObject;
		}

		StartCoroutine(RemoveHot());
	}


	// Update is called once per frame
	void Update () 
	{
		if (!spellActive)
			return;

		//Method used to add health to the current player
		MobaHotDestinationObj();

		transform.Rotate(Vector3.up * (rotateSpeed + Time.deltaTime));
		transform.position = destinationGameObj.transform.position;
	}

	IEnumerator RemoveHot()
	{
		
		
		yield return new WaitForSeconds (expireTime);
		PlayerStats statsScript = destinationGameObj.transform.GetComponent<PlayerStats>();
		statsScript.UpdateHealthClients();
		statsScript.hotObj = null;
		uLink.Network.Destroy(this.gameObject);
		
	}

}
