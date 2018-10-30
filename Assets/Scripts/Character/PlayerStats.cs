using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using uLink;

[System.Serializable]
public class PlayerStats : BaseStats {


	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager_mn");
		
		dataScript = gameManager.GetComponent<PlayerDatabase>();

		playerCScript = GetComponent<PlayerControllerRTS>();

		agent = GetComponentInChildren<NavMeshAgent>();

		trigger = transform.FindChild("Trigger").gameObject;

		gameEventScript = gameManager.GetComponent<GameEvents>();


		if(networkView.isOwner && isPlayer)
		{
			networkView.RPC("UpdateInitDataToPlayers", uLink.RPCMode.Server);
			networkView.RPC("UpgradeAbilityServer", uLink.RPCMode.Server, PlayerControllerRTS.PlayerState.attackingBasic, false);
			//networkView.RPC("UpgradeAbilityServer", uLink.RPCMode.Server, PlayerControllerRTS.PlayerState.attackingQ, false);
			//networkView.RPC("UpgradeAbilityServer", uLink.RPCMode.Server, PlayerControllerRTS.PlayerState.attackingW, false);
			//networkView.RPC("UpgradeAbilityServer", uLink.RPCMode.Server, PlayerControllerRTS.PlayerState.attackingE, false);
			//networkView.RPC("UpgradeAbilityServer", uLink.RPCMode.Server, PlayerControllerRTS.PlayerState.attackingR, false);
		}

		mana = baseMana;
		myHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
		UpdateRegeneration();

	}



	//ULINK CALL BACK WHEN PLAYER IS CONNECTED
	void uLink_OnPlayerConnected(uLink.NetworkPlayer nPlayer)
	{
		networkView.RPC("UpdateDataToClients", nPlayer, isPlayer, playerTeam, gameObject.name, myHealth, maxHealth,  healthRegenerate, mana, baseMana, destroyed, playerLvl, _bAdValue, _bApValue, adRes, apRes, _adValueAdd, _apValueAdd, adResAdd, apResAdd, playerExp, maxPlayerExp, gold, speed, speedAdd, attackRedAdd);
	}


	/// Updates the data on clients.
	public void UpdateStatsOnClients ()
	{
		networkView.RPC("UpdateDataToClients", uLink.RPCMode.Others, isPlayer, playerTeam, gameObject.name, myHealth, maxHealth,  healthRegenerate, mana, baseMana, destroyed, playerLvl, _bAdValue, _bApValue, adRes, apRes, _adValueAdd, _apValueAdd, adResAdd, apResAdd, playerExp, maxPlayerExp, gold, speed, speedAdd, attackRedAdd);
	
	}


	[PunRPC]
	void UpdateDataToClients (bool sIsPlayer ,string sTeam, string sName, float sMyhealth, float sMaxHealth, float sHealthRegenRate, float sMana, float sBaseMana, bool sDestroyed, int slvl, float sAd, float sAp, float sAdRes,float sApRes,
	                          float sAdAdd, float sApAdd, float sAdResAdd,float sApResAdd, float sPlayerExp,float sMaxPlayerExp,float sGold, float sSpeed, float sSpeedAdd, float sAttackRedAdd)
	{

		isPlayer = sIsPlayer;
		playerTeam = sTeam;
		gameObject.name = sName;
		myHealth = sMyhealth;
		maxHealth = sMaxHealth;
		mana = sMana;
		baseMana = sBaseMana;
		healthRegenerate = sHealthRegenRate;
		destroyed = sDestroyed;
		playerLvl = slvl;
		_bAdValue = sAd;
		_bApValue = sAp;
		adRes = sAdRes;
		apRes = sApRes;
		_adValueAdd = sAdAdd;
		_apValueAdd = sApAdd;
		adResAdd = sAdResAdd;
		apResAdd = sApResAdd;
		gold = sGold;
		playerExp = sPlayerExp;
		maxPlayerExp = sMaxPlayerExp;
		speed = sSpeed;
		speedAdd = sSpeedAdd;
		attackRedAdd = sAttackRedAdd;

		gameObject.name = sName;

		if (isPlayer)
		{
			if (sTeam == "blue")
			{
				gameObject.layer = 9;
				gameObject.tag = "BluePlayerTag";
				trigger = transform.FindChild("Trigger").gameObject;
				trigger.tag = "BluePlayerTriggerTag";
				trigger.layer = 16;
			}
			else
			{
				gameObject.layer = 10;
				gameObject.tag = "RedPlayerTag";
				trigger = transform.FindChild("Trigger").gameObject;
				trigger.tag = "RedPlayerTriggerTag";
				trigger.layer = 17;
			}
		}
	}

	/// Updates the data on clients.
	[PunRPC]
	private void UpdateInitDataToPlayers()
	{
		networkView.RPC("UpdateInitDataPlayers", uLink.RPCMode.Owner, stats.prefabPath, stats.basic.weaponType, stats.q.weaponType, stats.w.weaponType, stats.e.weaponType, stats.r.weaponType);
	}

	//RUNNING ON THE CLIENT SIDE OWNER ONLY
	[PunRPC]
	private void UpdateInitDataPlayers (string sPrefabPath, DmgDataclass.type bType, DmgDataclass.type qType, DmgDataclass.type wType, DmgDataclass.type eType, DmgDataclass.type rType)
	{
		stats.prefabPath = sPrefabPath;
		stats.basic.weaponType = bType;
		stats.q.weaponType = qType;
		stats.w.weaponType = wType;
		stats.e.weaponType = eType;
		stats.r.weaponType = rType;


	}
	//USED TO SEND A RPC TO THE CLIENTS WITH THE HEALTH
	public void UpdateAdds ()
	{
		if (uLink.Network.isServer)
		{
			networkView.RPC("UpdateAddsClient", uLink.RPCMode.Others, maxHealth, baseMana, _adValueAdd, _apValueAdd, adResAdd, apResAdd, speedAdd, attackRedAdd, cdRedAdd);
		}
		
	}
	[PunRPC]
	private void UpdateAddsClient (float SmaxHealth, float SbaseMana, float S_adValueAdd,float S_apValueAdd,float SadResAdd,float SapResAdd,float SspeedAdd,float SattackRedAdd,float ScdRedAdd)
	{
		maxHealth = SmaxHealth;
		baseMana = SbaseMana;
		_adValueAdd = S_adValueAdd;
		_apValueAdd = S_apValueAdd;
		adResAdd = SadResAdd;
		apResAdd = SapResAdd;
		speedAdd = SspeedAdd;
		attackRedAdd = SattackRedAdd;
		cdRedAdd = ScdRedAdd;
	}

	//RUNING ON THE CLIENT ONLY
	[PunRPC]
	void UpdateClientHealth (float sHealth, float sMaxHealth)
	{
		myHealth =  sHealth;
		maxHealth = sMaxHealth;
	}


	//USED TO SEND A RPC TO THE CLIENTS WITH THE MANA
	public void UpdateManaToClients ()
	{
		networkView.RPC("UpdateClientMana", uLink.RPCMode.Others, mana, baseMana, rechargeRateMana);
		
	}
	[PunRPC]
	void UpdateClientMana (float playerMana, float playerBaseMana, float playerRechargeRate) {
		mana = playerMana;
		baseMana = playerBaseMana;
		rechargeRateMana = playerRechargeRate;
	}
	//USED TO SEND A RPC TO THE CLIENTS WITH THE NEW GOLD ADDED
	public void UpdateGoldToClients (float goldToAdd)
	{
		gold += goldToAdd;
		networkView.RPC("UpdateClientGold", uLink.RPCMode.Others, gold);
		
	}
	[PunRPC]
	private void UpdateClientGold (float value) {
		gold = value;
	}
	
	//USED TO SEND A RPC TO THE CLIENTS WITH THE NEW EXP ADDED
	public void UpdateExpToClients (float expToAdd)
	{
		playerExp += expToAdd;
		networkView.RPC("UpdateClientExp", uLink.RPCMode.Others, playerExp);
		
	}
	[PunRPC]
	void UpdateClientExp (float value) {
		playerExp = value;
	}

	//USED TO SEND A RPC TO THE CLIENTS WITH THE ABILITYLOCKED VALUE
	public void UpdateAbilityLockedClients (bool value) 
	{
		if (uLink.Network.isServer) {
			abilityLocked = value;
			networkView.RPC("UpdateClientAbilityLocked", uLink.RPCMode.Others, value);
		}
	}
	[PunRPC]
	void UpdateClientAbilityLocked (bool sAbilityLocked) 
	{
		abilityLocked =  sAbilityLocked;	
	}


	//SEND A RPC WHEN THE PLAYER LEVEL UP TO SHOW THE SPELL LEVEL UP BUTTONS ON THE OWNER PLAYER
	[PunRPC]
	public void ShowAbilityUpUi()
	{
		if (uLink.Network.isServer)
		{
			networkView.RPC("ShowAbilityUpUi", networkOwner);
			networkView.RPC("ShowLvlUpParticle", uLink.RPCMode.Others);
		}
		else
		{

			gameEventScript.ShowSpellUpButtons();
		}
	}

	//SEND A RPC TO LEVEL UP ANY ABILITY
	public void UpgradeAbility(PlayerControllerRTS.PlayerState type, bool levelUpAbility)
	{

		networkView.RPC("UpgradeAbilityServer", uLink.RPCMode.Server, type, levelUpAbility);

		
	}
	//ADD A LEVEL TO THE PLAYER SPELL AND SEND A RPC TO THE CLIENTS WITH THE NEW VALUE
	[PunRPC]
	private void UpgradeAbilityServer(PlayerControllerRTS.PlayerState type, bool levelUpAbility)
	{

		switch (type)
		{
		case PlayerControllerRTS.PlayerState.attackingBasic:
			if (playerCScript.playerStScript.stats.basic.weaponLvl < 1 && levelUpAbility)
				playerCScript.playerStScript.stats.basic.weaponLvl ++;

			networkView.RPC("UpgradeAbilityClient", uLink.RPCMode.Owner, PlayerControllerRTS.PlayerState.attackingBasic, playerCScript.playerStScript.stats.basic.cdr, 
			                playerCScript.playerStScript.stats.basic.ad, playerCScript.playerStScript.stats.basic.ap, playerCScript.playerStScript.stats.basic.weaponLvl);


			break;
		case PlayerControllerRTS.PlayerState.attackingQ:
			if (playerCScript.playerStScript.stats.q.weaponLvl<1 && levelUpAbility)
				playerCScript.playerStScript.stats.q.weaponLvl ++;

			networkView.RPC("UpgradeAbilityClient", uLink.RPCMode.Owner, PlayerControllerRTS.PlayerState.attackingQ, playerCScript.playerStScript.stats.q.cdr, 
			                playerCScript.playerStScript.stats.q.ad, playerCScript.playerStScript.stats.q.ap, playerCScript.playerStScript.stats.q.weaponLvl);
			break;
		case PlayerControllerRTS.PlayerState.attackingW:
			if (playerCScript.playerStScript.stats.w.weaponLvl<1 && levelUpAbility)
				playerCScript.playerStScript.stats.w.weaponLvl ++;

			networkView.RPC("UpgradeAbilityClient", uLink.RPCMode.Owner, PlayerControllerRTS.PlayerState.attackingW, playerCScript.playerStScript.stats.w.cdr, 
			                playerCScript.playerStScript.stats.w.ad, playerCScript.playerStScript.stats.w.ap, playerCScript.playerStScript.stats.w.weaponLvl);
			break;
		case PlayerControllerRTS.PlayerState.attackingE:
			if (playerCScript.playerStScript.stats.e.weaponLvl<1 && levelUpAbility)
				playerCScript.playerStScript.stats.e.weaponLvl ++;
		
			networkView.RPC("UpgradeAbilityClient", uLink.RPCMode.Owner, PlayerControllerRTS.PlayerState.attackingE, playerCScript.playerStScript.stats.e.cdr, 
			                playerCScript.playerStScript.stats.e.ad, playerCScript.playerStScript.stats.e.ap, playerCScript.playerStScript.stats.e.weaponLvl);
			break;
		case PlayerControllerRTS.PlayerState.attackingR:
			if (playerCScript.playerStScript.stats.r.weaponLvl <1 && levelUpAbility)
				playerCScript.playerStScript.stats.r.weaponLvl ++;
		
			networkView.RPC("UpgradeAbilityClient", uLink.RPCMode.Owner, PlayerControllerRTS.PlayerState.attackingR, playerCScript.playerStScript.stats.r.cdr, 
			                playerCScript.playerStScript.stats.r.ad, playerCScript.playerStScript.stats.r.ap, playerCScript.playerStScript.stats.r.weaponLvl);
			break;
		}



		
		
	}

	[PunRPC]
	private void UpgradeAbilityClient(PlayerControllerRTS.PlayerState type, float cdr, float dmgAd, float dmgAp, int sWeaponLevel)
	{
		switch (type)
		{
		case PlayerControllerRTS.PlayerState.attackingBasic:
			playerCScript.playerStScript.stats.basic.weaponLvl = sWeaponLevel;
			playerCScript.playerStScript.stats.basic.ad = dmgAd;
			playerCScript.playerStScript.stats.basic.ap = dmgAp;
			playerCScript.playerStScript.stats.basic.cdr = cdr;
			playerCScript.playerStScript.stats.basic.cdrTStamp = Time.time;
			break;
		case PlayerControllerRTS.PlayerState.attackingQ:
			playerCScript.playerStScript.stats.q.weaponLvl = sWeaponLevel;
			if (sWeaponLevel >0)
			playerCScript.cdrUiQScript.active = true;
			playerCScript.playerStScript.stats.q.ad = dmgAd;
			playerCScript.playerStScript.stats.q.ap = dmgAp;
			playerCScript.playerStScript.stats.q.cdr = cdr;
			playerCScript.playerStScript.stats.q.cdrTStamp = Time.time;
			break;
		case PlayerControllerRTS.PlayerState.attackingW:
			playerCScript.playerStScript.stats.w.weaponLvl  = sWeaponLevel;
			if (sWeaponLevel >0)
			playerCScript.cdrUiWScript.active = true;
			playerCScript.playerStScript.stats.w.ad = dmgAd;
			playerCScript.playerStScript.stats.w.ap = dmgAp;
			playerCScript.playerStScript.stats.w.cdr = cdr;
			playerCScript.playerStScript.stats.w.cdrTStamp = Time.time;
			break;
		case PlayerControllerRTS.PlayerState.attackingE:
			playerCScript.playerStScript.stats.e.weaponLvl  = sWeaponLevel;
			if (sWeaponLevel >0)
			playerCScript.cdrUiEScript.active = true;
			playerCScript.playerStScript.stats.e.ad = dmgAd;
			playerCScript.playerStScript.stats.e.ap = dmgAp;
			playerCScript.playerStScript.stats.e.cdr = cdr;
			playerCScript.playerStScript.stats.e.cdrTStamp = Time.time;
			break;
		case PlayerControllerRTS.PlayerState.attackingR:
			playerCScript.playerStScript.stats.r.weaponLvl  = sWeaponLevel;
			if (sWeaponLevel >0)
			playerCScript.cdrUiRScript.active = true;
			playerCScript.playerStScript.stats.r.ad = dmgAd;
			playerCScript.playerStScript.stats.r.ap = dmgAp;
			playerCScript.playerStScript.stats.r.cdr = cdr;
			playerCScript.playerStScript.stats.r.cdrTStamp = Time.time;
			break;
		}
		
	}







	//SPAWN THE uiDamageDealt OBJ TO SHOW THE DAMAGE POP UP ON THE CLIENT
	[PunRPC]
	void ShowDmgUi (string reciever, string attacker, float totalDmg, bool dealing)
	{

		GameObject uiDamage = Instantiate(uiDamageDealt, new Vector3 (0,0,0), Quaternion.identity) as GameObject; 
		DamageDealtUI label = uiDamage.GetComponent<DamageDealtUI>();
		label.totalDmg = totalDmg;
		label.reciever = reciever;
		label.dealingDamage = dealing;

	}

	[PunRPC]
	void ShowGoldUi (string reciever, string attacker, float totalGold, bool dealing)
	{
		
		GameObject uiDamage = Instantiate(uiDamageDealt, new Vector3 (0,0,0), Quaternion.identity) as GameObject; 
		DamageDealtUI label = uiDamage.GetComponent<DamageDealtUI>();
		label.totalDmg = totalGold;
		label.reciever = reciever;
		label.dealingDamage = dealing;
		
	}


}
