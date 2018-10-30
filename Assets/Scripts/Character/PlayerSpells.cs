using UnityEngine;
using System.Collections;

//THIS SCRIPT IS ATTACHED TO THE PLAYER CREATOR AND PLAYER OWNER
//THIS SCRIPT IS USEND TO LAUNCH ALL PLAYER ABILITIES
//THIS SCRIPT IS ACCESED BY THE PLAYER CONTROLLER RTS SCRIPT

public class PlayerSpells : uLink.MonoBehaviour  {

	private string Bprefab;
	private string Wprefab;
	private string Qprefab;
	private string Eprefab;
	private string Rprefab;


	[HideInInspector] public GameObject target_obj;

	public Transform Blaunch;
	public Transform Qlaunch;
	public Transform Wlaunch;
	public Transform Elaunch;
	public Transform Rlaunch;
	
	private float finalAd;
	private float finalAp;


	private PlayerControllerRTS playerCScript;




	// Use this for initialization
	private PlayerStats playerStatsScript;

	[HideInInspector] public Vector3 AttackDestination;



	void Start () 
	{
		playerCScript = GetComponent<PlayerControllerRTS>();
		playerStatsScript = GetComponent<PlayerStats>();

	}



	public void Attacking(int type) 
	{
		if (uLink.Network.isServer)
		{


			if (type == 3)
			{

				if (playerCScript.playerStScript.stats.basic.ad >0)
				{
					 finalAd = playerCScript.playerStScript.stats.basic.ad + playerStatsScript._adValueAdd;
				}
				else
				{
					 finalAd = 0;
				}
				if (playerCScript.playerStScript.stats.basic.ap >0)
				{
					 finalAp = playerCScript.playerStScript.stats.basic.ap + playerStatsScript._apValueAdd;
				}
				else
				{
					 finalAp = 0;
				}

				Bprefab = "Abilities/" + playerCScript.playerStScript.stats.prefabPath + "/_B_mn";

				uLink.Network.Instantiate(uLink.NetworkPlayer.server, Bprefab,Bprefab,Bprefab, GetLaunchPos(playerCScript.playerStScript.stats.basic.LaunchPos), this.transform.rotation, 0, 
					                                                             playerStatsScript.playerTeam, transform.name, AttackDestination, target_obj.name, finalAd, finalAp);
				



			}

			if (type == 4)
			{
				Qprefab = "Abilities/" +playerCScript.playerStScript.stats.prefabPath + "/_Q_mn";

				string targetName = "";
				if (target_obj)
					targetName = target_obj.name;
				else
					targetName = "";
				if (playerCScript.playerStScript.stats.q.ad >0)
				{
					finalAd = playerCScript.playerStScript.stats.q.ad + playerStatsScript._adValueAdd;
				}
				else
				{
					finalAd = 0;
				}
				if (playerCScript.playerStScript.stats.q.ap >0)
				{
					finalAp = playerCScript.playerStScript.stats.q.ap + playerStatsScript._apValueAdd;
				}
				else
				{
					finalAp = 0;
				}
				uLink.Network.Instantiate(uLink.NetworkPlayer.server, Qprefab,Qprefab,Qprefab, GetLaunchPos(playerCScript.playerStScript.stats.q.LaunchPos), this.transform.rotation, 0,
			                                                             playerStatsScript.playerTeam, transform.name, AttackDestination, targetName, finalAd, finalAp);
			



			}

			if (type == 5)
			{
				Wprefab = "Abilities/" +playerCScript.playerStScript.stats.prefabPath + "/_W_mn";

				string targetName = "";
				if (target_obj)
					targetName = target_obj.name;
				else
					targetName = "";
				if (playerCScript.playerStScript.stats.w.ad >0)
				{
					finalAd = playerCScript.playerStScript.stats.w.ad + playerStatsScript._adValueAdd;
				}
				else
				{
					finalAd = 0;
				}
				if (playerCScript.playerStScript.stats.w.ap >0)
				{
					finalAp = playerCScript.playerStScript.stats.w.ap + playerStatsScript._apValueAdd;
				}
				else
				{
					finalAp = 0;
				}
				uLink.Network.Instantiate(uLink.NetworkPlayer.server, Wprefab,Wprefab,Wprefab, GetLaunchPos(playerCScript.playerStScript.stats.w.LaunchPos), this.transform.rotation, 0,
			                                                             playerStatsScript.playerTeam, transform.name, AttackDestination, targetName, finalAd, finalAp);
				
			}
			if (type == 6)
			{

				Eprefab = "Abilities/" +playerCScript.playerStScript.stats.prefabPath + "/_E_mn";

				string targetName = "";
				if (target_obj)
					targetName = target_obj.name;
				else
					targetName = "";
				if (playerCScript.playerStScript.stats.e.ad >0)
				{
					finalAd = playerCScript.playerStScript.stats.e.ad + playerStatsScript._adValueAdd;
				}
				else
				{
					finalAd = 0;
				}
				if (playerCScript.playerStScript.stats.e.ap >0)
				{
					finalAp = playerCScript.playerStScript.stats.e.ap + playerStatsScript._apValueAdd;
				}
				else
				{
					finalAp = 0;
				}
				uLink.Network.Instantiate(uLink.NetworkPlayer.server, Eprefab,Eprefab,Eprefab, GetLaunchPos(playerCScript.playerStScript.stats.e.LaunchPos), this.transform.rotation, 0,
			                                                             playerStatsScript.playerTeam, transform.name, AttackDestination, targetName, finalAd, finalAp);

					

			}
			if (type == 7)
			{
				Rprefab = "Abilities/" +playerCScript.playerStScript.stats.prefabPath + "/_R_mn";

				string targetName = "";
				if (target_obj)
					targetName = target_obj.name;
				else
					targetName = "";
				if (playerCScript.playerStScript.stats.r.ad >0)
				{
					finalAd = playerCScript.playerStScript.stats.r.ad + playerStatsScript._adValueAdd;
				}
				else
				{
					finalAd = 0;
				}
				if (playerCScript.playerStScript.stats.r.ap >0)
				{
					finalAp = playerCScript.playerStScript.stats.r.ap + playerStatsScript._apValueAdd;
				}
				else
				{
					finalAp = 0;
				}
				uLink.Network.Instantiate(uLink.NetworkPlayer.server, Rprefab,Rprefab,Rprefab, GetLaunchPos(playerCScript.playerStScript.stats.r.LaunchPos), this.transform.rotation, 0,
			                                                             playerStatsScript.playerTeam, transform.name, AttackDestination, targetName, finalAd, finalAp);

			}

		
		
	
	}
	}

	public Vector3 GetLaunchPos(DmgDataclass.launchPos LaunchPos)
	{
		Vector3 returnPoint = Vector3.zero;

		switch (LaunchPos)
		{
		case DmgDataclass.launchPos._floor:
			returnPoint = AttackDestination;
			break;

		case DmgDataclass.launchPos._b:
			returnPoint = Blaunch.transform.position;

			break;

		case DmgDataclass.launchPos._q:
			returnPoint = Qlaunch.transform.position;
			
			break;

		case DmgDataclass.launchPos._w:
			returnPoint = Wlaunch.transform.position;
			
			break;

		case DmgDataclass.launchPos._e:
			returnPoint = Elaunch.transform.position;
			
			break;

		case DmgDataclass.launchPos._r:
			returnPoint = Rlaunch.transform.position;
			
			break;

		case DmgDataclass.launchPos._sky:
			returnPoint = transform.position;
			returnPoint.y += 10;
			break;

		case DmgDataclass.launchPos.center:
			returnPoint = transform.position;
			returnPoint.y -= 2;
			break;

		}



		return returnPoint;
	}


}
