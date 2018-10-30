using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// This script is attached to the enemies and it draws the 
/// healthbar above them.
/// This script accesses the PlayerStats script for 
/// determining the healthbar length.
/// 
/// </summary>


public class CreepLabel : uLink.MonoBehaviour {
	
	//Variables Start___________________________________
	
	//The health bar texture is attached to this in the inspector.
	
	public Image health;

	//Quick references.

	private PlayerStats playerStatsScript;

	private float healthBarLength;

	void Awake ()
	{
		//This script will only run on the client
		if(networkView.isMine == false)
		{
			playerStatsScript = GetComponent<PlayerStats>();			
		}
		else
		{
			enabled = false;	
		}
	}

	// Update is called once per frame
	void Update () 
	{	
		if (health)
		{
			healthBarLength = (playerStatsScript.myHealth / playerStatsScript.maxHealth);

			health.fillAmount = healthBarLength;

			if (playerStatsScript.myHealth <= 0)
			{
				health.fillAmount = 0;
			}
		}
	}

	

	

}
