using UnityEngine;
using System.Collections;



public class MobaStormAbility : uLink.MonoBehaviour {

	//explotion particle effect attached
	public GameObject explotionObj;

	//DATA USED TO DEAL DMG ON THE CLIENT
	[HideInInspector]public string myOriginator;
	[HideInInspector]public CharacterClass.charName originatorCharName;
	[HideInInspector]public string team;
	public float ad;
	public float ap;
	protected float gg;
	[HideInInspector] public GameObject originatorGameObj;
	[HideInInspector] public GameObject destinationGameObj;
	[HideInInspector] public Vector3 initialPos;
	[HideInInspector] public Vector3 floorPos;
	// Use this for initialization

	//SCRIPT ACCESED TO UNLOCK ABILITIES
	[HideInInspector] public PlayerStats originatorPlayerScript;
	protected bool spellActive 		= false;
	protected bool expended 		= false;
	public float expireTime 		= 5;

	public  BuffDebuff buffDebuff;
	[HideInInspector] public LayerMask mask;


	
	[System.Serializable]
	public class BuffDebuff {

		public bool isStun = false;
		public GameObject stunObj;
		public float stunTime = 0;
		public bool isSlow = false;
		public GameObject slowObj;
		public float slowTime = 0;
		public float slowValue = 0;
		public bool isDot = false;
		public GameObject dotObj;
		public float dotTime = 0;
		public float dotValue = 0;
		public bool isHot = false;
		public GameObject hotObj;
		public float hotTime = 0;
		public float hotValue = 0;

	}








	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//METHOD USED TO DEAL DMG TO A OBJ DESTINATION
	public void MobaDealDmgDestinationObj () {
		if (uLink.Network.isServer)
		{
			if (destinationGameObj.tag == "CreepTag" || destinationGameObj.tag == "RedTeamTag" && team == "blue" || destinationGameObj.tag == "BlueTeamTag" && team == "red" || destinationGameObj.tag == "RedPlayerTag" && team == "blue" || destinationGameObj.tag == "BluePlayerTag" && team == "red")
			{
				
				expended = true;

				
				PlayerStats statsScript = destinationGameObj.transform.GetComponent<PlayerStats>();
				statsScript.DrainHealth(ad, ap, myOriginator, originatorGameObj, originatorGameObj.tag, originatorCharName);

				if (buffDebuff.isSlow)
				{
					uLink.Network.Instantiate(uLink.NetworkPlayer.server,buffDebuff.slowObj ,buffDebuff.slowObj ,buffDebuff.slowObj , transform.position,transform.rotation, 0, 
					                                                             "slow", myOriginator, transform.position, destinationGameObj.name, buffDebuff.slowValue, buffDebuff.slowTime);
				}
				if (buffDebuff.isStun)
				{
					uLink.Network.Instantiate(uLink.NetworkPlayer.server, buffDebuff.stunObj,buffDebuff.stunObj,buffDebuff.stunObj, transform.position,transform.rotation, 0, 
					                                                             "stun", myOriginator, transform.position, destinationGameObj.name, buffDebuff.stunTime, 0);
				}
				
			}


		}
	}
	//METHOD USED TO DEAL DMG TO A TRIGGER DESTINATION
	public void MobaDealDmgTriggerObj (GameObject obj) {
		if (uLink.Network.isServer)
		{
			if (obj.tag == "CreepTriggerTag" || obj.tag == "RedTeamTriggerTag" && team == "blue" || obj.tag == "BlueTeamTriggerTag" && team == "red" || obj.tag == "RedPlayerTriggerTag" && team == "blue" || obj.tag == "BluePlayerTriggerTag" && team == "red")
			{
				expended = true;
				GameObject enemy = (GameObject) obj.transform.gameObject;
				
				PlayerStats playerStatsScript = enemy.transform.parent.GetComponent<PlayerStats>();
				
				playerStatsScript.DrainHealth(ad, ap, myOriginator,originatorGameObj, originatorGameObj.tag, originatorCharName);
				
			

				if (buffDebuff.isSlow)
				{
					uLink.Network.Instantiate(uLink.NetworkPlayer.server, buffDebuff.slowObj,buffDebuff.slowObj,buffDebuff.slowObj, transform.position,transform.rotation, 0, 
					                                                             "slow", myOriginator, transform.position, enemy.transform.parent.name, buffDebuff.slowValue, buffDebuff.slowTime);
				}

				if (buffDebuff.isStun)
				{
					uLink.Network.Instantiate(uLink.NetworkPlayer.server, buffDebuff.stunObj,buffDebuff.stunObj,buffDebuff.stunObj, transform.position,transform.rotation, 0, 
					                                                             "stun", myOriginator, transform.position, enemy.transform.parent.name, buffDebuff.stunTime, 0);
				}

				
			}
			

		}
	}

	public void MobaHotDestinationObj () {


			if (destinationGameObj)
			{
				PlayerStats enemyStatsScript = destinationGameObj.transform.GetComponent<PlayerStats>();
				enemyStatsScript.myHealth += ad * Time.deltaTime;
				enemyStatsScript.mana += ad * Time.deltaTime;
			}
			else
			{
				uLink.Network.Destroy(this.gameObject);
			}


	}

	public void MobaSlowObj (GameObject obj, float value,float time) {
		if (uLink.Network.isServer)
		{
			StartCoroutine(MobaSlow(destinationGameObj,value,time));
		}

	}
	private IEnumerator MobaSlow (GameObject obj,float value,float time) {

		PlayerStats enemyStatsScript = obj.GetComponent<PlayerStats>();

		if (enemyStatsScript.slowObj)
		{
		
			uLink.Network.Destroy(enemyStatsScript.slowObj);
			enemyStatsScript.slowObj = this.gameObject;
			enemyStatsScript.agent.speed = enemyStatsScript.speed;
			
		}
		else
		{

			enemyStatsScript.slowObj = this.gameObject;
		}
		float lastSpeed = enemyStatsScript.speed;
		enemyStatsScript.speed = enemyStatsScript.speed * value / 100;

		yield return new WaitForSeconds (time);

		enemyStatsScript.speed = lastSpeed;
		enemyStatsScript.slowObj = null;

		uLink.Network.Destroy(this.gameObject);


	}

	public void MobaStunObj (GameObject obj,float time) {
		if (uLink.Network.isServer)
		{
			StartCoroutine(MobaStun(destinationGameObj,time));
		}
		
	}
	private IEnumerator MobaStun (GameObject obj, float time) {
		
		PlayerStats enemyStatsScript = obj.GetComponent<PlayerStats>();

		if (enemyStatsScript.stunObj)
		{

			uLink.Network.Destroy(enemyStatsScript.stunObj);
			enemyStatsScript.stunObj = this.gameObject;

			
		}
		else
		{

			enemyStatsScript.stunObj = this.gameObject;
		}

		//enemyStatsScript.abilityLocked = true;
		enemyStatsScript.UpdateAbilityLockedClients(true);

		yield return new WaitForSeconds (time);

		//enemyStatsScript.abilityLocked = false;
		enemyStatsScript.UpdateAbilityLockedClients(false);
		enemyStatsScript.stunObj = null;
		
		uLink.Network.Destroy(this.gameObject);
		
		
	}

	//INITIAL DATA SENT FROM THE PLAYERSPELL SCRIPT
	void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		string pTeam = info.networkView.initialData.ReadString();
		string pOriginator = info.networkView.initialData.ReadString();
		Vector3 pDestination = info.networkView.initialData.ReadVector3();
		string pObjNameDestination = info.networkView.initialData.ReadString();
		float pAd = info.networkView.initialData.ReadSingle();
		float pAp = info.networkView.initialData.ReadSingle();

		floorPos = pDestination;
		myOriginator = pOriginator;
		team = pTeam;
		ad = pAd;
		ap = pAp;

		initialPos = transform.position;
		originatorGameObj = GameObject.Find(pOriginator);
		originatorCharName = originatorGameObj.GetComponent<PlayerStats>().charName;
		destinationGameObj = GameObject.Find(pObjNameDestination);
		
		spellActive = true;

		if (team == "red")
		{
			mask = ((1 << 16) | (1 << 21));
		}
		else
		{
			mask = ((1 << 17) | (1 << 21));
		}

		OnStart();
		StartCoroutine(DestroyMyselfAfterSomeTime());
	}

	//CALLED FROM THE uLink_OnNetworkInstantiate METHOD WHEN ALL DATA IS RECIEVED
	protected virtual void OnStart()
	{

	}

	protected virtual IEnumerator DestroyMyselfAfterSomeTime()
	{

		yield return new WaitForSeconds (0);
	}
}


