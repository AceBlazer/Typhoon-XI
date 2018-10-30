using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Utility;


	[RequireComponent(typeof (NavMeshAgent))]
	public class PlayerControllerRTS : uLink.MonoBehaviour {
	public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding

	
	public enum PlayerState
		{
			idle,			
			running,
			chasing,			
			attackingBasic,			
			attackingQ, 
			attackingW, 
			attackingE, 
			attackingR, 
			dead,
			recall,
			emote,
			retreat
			
		}
		
	public PlayerState playerStatus = PlayerState.idle;

		public enum MouseState
		{
			idle,			
			attackingBasic,			
			attackingQ, 			
			attackingW, 	
			attackingE, 
			attackingR, 
			server
			
			
		}

	private MouseState mouseState = MouseState.server;
	private InputField inputFieldGameChat;
	//public 	GameObject recallEffect;
	//USED TO SEND THE TARGET NAME TO THE SERVER
	private GameObject targetObj;



	public 	GameObject mouseMovePointerPrefab;
	private GameObject mouseMovePointer;
	private GameObject mouseProjector;

	
	private Animator animator;

	//SCRIPTS ACCESED ON THIS CLASS
	private PlayerSpells playerSpellScript;

	[HideInInspector] public ShowCDR cdrUiQScript;
	[HideInInspector] public ShowCDR cdrUiWScript;
	[HideInInspector] public ShowCDR cdrUiEScript;
	[HideInInspector] public ShowCDR cdrUiRScript;

	private Image healthCdImage;
	private Image manaCdImage;


	//THIS SCRIPT IS USED TO SHOW THE LEVEL OF SPELLS TO THE USER
	private SpellLvlScript spellLvlScript;

	//ACCES GAME MANAGER OBJECT AND DMGDATABASE SCRIPT
	private GameObject GameManager;

	private DmgDatabase dmgScript;

	private Vector3 destinationPos;


	public 	LayerMask redMask;
	public 	LayerMask blueMask;
	public 	LayerMask floorMask;

	private LayerMask mask;

	private CDRUi cdrUiScript;
	private LeftPanelUi leftPanelScript;
	[HideInInspector] public PlayerStats playerStScript;

	//STORE THE INITIAL POSITION AND ROTATION OF THE PLAYER TO WARP THE AGENT WHEN THE PLAYER IS DEAD
	private Vector3 initialPos;
	private Quaternion initialRotation;

	//USED TO SEND LOCK THE CAMERA TO THE CURRENT OWNER NETWORK PLAYER
	private GameObject cameraObj;
	Camera cam;
	private CustomCamera cameraScript;

	//Handle particle creation
	[HideInInspector]  public ParticleManager particleScript;

	//private GameObject teleportObj;

	public AudioClip basicClip;
	public AudioClip qClip;
	public AudioClip wClip;
	public AudioClip eClip;
	public AudioClip rClip;
	public AudioClip deadClip;
	public AudioClip TeleportClip;
	private GameObject teleportParticle;
	private GameObject teleportObj;
	private Image mainPortrait;
	//VOID START
	void Start () {
		animator = GetComponent<Animator>();
		playerStScript = GetComponent<PlayerStats>();

		//FIND THE InputFieldGameChat Object and get the InputField COMPONENT
		inputFieldGameChat = GameObject.Find("InputFieldGameChat").GetComponent<InputField>();

		//FIND GAMEMAGER AND ACCESS THE DMGDATABASE SCRIPT
		GameManager = GameObject.Find("GameManager_mn");
		dmgScript = GameManager.GetComponent<DmgDatabase>();
		particleScript = GameManager.GetComponent<ParticleManager>();

		particleScript.CreateSpawnParticle(this.transform);
		teleportParticle = particleScript.teleportParticle;

		if (uLink.Network.isServer)
		{
			playerSpellScript = transform.GetComponent<PlayerSpells>();
			agent = GetComponentInChildren<NavMeshAgent>();

			agent.updateRotation = false;
			agent.updatePosition = true;
			
			initialRotation = transform.rotation;
			initialPos = transform.position;
		

		}

		if (networkView.isOwner == true)
		{
			mainPortrait = GameObject.Find("CharPortrait").GetComponent<Image>();

			mainPortrait.sprite = playerStScript.stats.charPortrait;

			//StartCoroutine(RecallBaseRPC());
			GameManager.GetComponent<GameEvents>().PlayGreetingsAudio();

			//SEND A REFERENCE OF THE PLAYERSTATS SCRIPT TO THE GAMEEVENTS SCRIPT ATTACHED TO GAME MANAGER OBJ
			GameManager.GetComponent<GameEvents>().playerStScript = playerStScript;

			GameObject.Find("LeftPanel").GetComponent<LeftPanelUi>();
			playerSpellScript = transform.GetComponent<PlayerSpells>();

			//FIND LeftPanel GAMEOBJECT AND GET LeftPanelUi COMPONENT
			leftPanelScript = GameObject.Find("LeftPanel").GetComponent<LeftPanelUi>();

			//FIND QcdUi GAMEOBJECT AND GET CDRUi COMPONENT
			cdrUiScript = GameObject.Find("CdUi").GetComponent<CDRUi>();

			//FIND Qcdr GAMEOBJECT AND GET SHOWCDR COMPONENT
			cdrUiQScript = GameObject.Find("Qcdr").GetComponent<ShowCDR>();
			//FIND Wcdr GAMEOBJECT AND GET SHOWCDR COMPONENT
			cdrUiWScript = GameObject.Find("Wcdr").GetComponent<ShowCDR>();
			//FIND Ecdr GAMEOBJECT AND GET SHOWCDR COMPONENT
			cdrUiEScript = GameObject.Find("Ecdr").GetComponent<ShowCDR>();
			//FIND Rcdr GAMEOBJECT AND GET SHOWCDR COMPONENT
			cdrUiRScript = GameObject.Find("Rcdr").GetComponent<ShowCDR>();

			//FIND EACH ELEMENT OF THE UI TO SHOW HEALTH, MANA AND SPELL LEVELS
			healthCdImage = GameObject.Find("HpBar").GetComponent<Image>();
			manaCdImage = GameObject.Find("ManaBar").GetComponent<Image>();
			spellLvlScript = GameObject.Find("SpellsLvl").GetComponent<SpellLvlScript>();
			//gameEventScript = gameManager.GetComponent<GameEvents>();
		
			//SET THE CAMERA ACTIVE TO THIS TARGET PLAYER
			cam = Camera.main;
			cameraObj = GameObject.Find("Custom_Camera");
			cameraScript = cameraObj.GetComponent<CustomCamera>();
			cameraScript.sources.target = this.transform;
			cameraScript.config.cameraActive = true;
			cameraScript.config.cameraLocked = true;

		
		}

		//playerStScript.stats  = playerStScript.stats;

		}

	void Update () {

		if (teleportObj)
		{
			teleportObj.transform.position = transform.position;
		}

		if (networkView.isOwner == true)
		{
			UpdateUIData();	

		}

		if (uLink.Network.isServer)
		{
			//DISABLE ANIMATOR COMPONENT IF THE PLAYER IS STUNNED
			if (playerStScript.stunObj && playerStScript.myHealth > 0)
			{
				if (uLink.Network.isServer)
				{
					agent.Stop();
					playerStatus = PlayerState.idle;
				}
				animator.enabled = false;
				return;
			}
			else
			{
				animator.enabled = true;
			}

			if (playerStScript.myHealth<=0)
			{
				playerStatus = PlayerState.dead;
			}

			
			//SHITCH CASE TO CONTROLL ALL ACTIONS OF THE PLAYER
			switch (playerStatus)
			{
			case PlayerState.idle:
				
				
				animator.SetInteger("AnimType", (int)PlayerState.idle);
				
				break;
			case PlayerState.running:
				if (!playerStScript.charLocked && !playerStScript.abilityLocked)
				{
					animator.SetInteger("AnimType", (int)PlayerState.running);
					
					MoveOrChase();
					
					if (!agent.pathPending)
					{
						if (agent.remainingDistance <= agent.stoppingDistance)
						{
							if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
							{
								playerStatus = PlayerState.idle;
							}
						}
					}
				}
				break;
				
			case PlayerState.chasing:
				
				break;
			case PlayerState.attackingBasic:
				
				CalculateAttack(playerStScript.stats.basic.weaponType, PlayerState.attackingBasic, playerStScript.stats.basic.rangue, playerStScript.stats.basic.cdr, playerStScript.stats.basic.cdrTStamp, 
				                playerStScript.stats.basic.manaCost);
				
				break;
			case PlayerState.attackingQ:
				
				CalculateAttack(playerStScript.stats.q.weaponType, PlayerState.attackingQ, playerStScript.stats.q.rangue, playerStScript.stats.q.cdr, playerStScript.stats.q.cdrTStamp, 
				                playerStScript.stats.q.manaCost);
				
				break;
				
			case PlayerState.attackingW:
				
				
				CalculateAttack(playerStScript.stats.w.weaponType, PlayerState.attackingW, playerStScript.stats.w.rangue, playerStScript.stats.w.cdr, playerStScript.stats.w.cdrTStamp, 
				                playerStScript.stats.w.manaCost);
				
				
				break;
				
			case PlayerState.attackingE:
				
				
				CalculateAttack(playerStScript.stats.e.weaponType, PlayerState.attackingE, playerStScript.stats.e.rangue, playerStScript.stats.e.cdr, playerStScript.stats.e.cdrTStamp, 
				                playerStScript.stats.e.manaCost);
				
				break;
			case PlayerState.attackingR:
				
				CalculateAttack(playerStScript.stats.r.weaponType, PlayerState.attackingR, playerStScript.stats.r.rangue, playerStScript.stats.r.cdr, playerStScript.stats.r.cdrTStamp, 
				                playerStScript.stats.r.manaCost);
				break;
				
			case PlayerState.dead:
				
				
				animator.SetInteger("AnimType", (int)PlayerState.dead);
				agent.Stop();
				break;
				
			case PlayerState.recall:
				
				animator.SetInteger("AnimType", (int)PlayerState.recall);
				
				break;
				
			case PlayerState.emote:
				
				animator.SetInteger("AnimType", (int)PlayerState.emote);
				agent.Stop();
				break;
				
			}
		}
	}
	
	
	public void OnGUI () {
		
		RaycastHit hit;
		if (!uLink.Network.isServer)
		{
			if (playerStScript.playerTeam == "red")
			{
				mask = redMask;
			}
			else
			{
				mask = blueMask;
			}
			
			if (Event.current.type == EventType.MouseDown && networkView.isOwner == true)
			{
				if (Physics.Raycast	(cam.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, mask)) 
					
				{				
					//If the player clicks something and are not in the chat mode
					if (cam != null && Event.current.button == 1  && !inputFieldGameChat.isFocused) 
					{
						mouseState = MouseState.idle;
						//If the player click on the floor
						if (hit.transform.tag == "Floor")
						{
							
							networkView.RPC("SendAttackToServer", uLink.RPCMode.Server, hit.point, PlayerState.running, "");
							if (!mouseMovePointer)
							{
								mouseMovePointer =	(GameObject)Instantiate(mouseMovePointerPrefab, hit.point, Quaternion.identity);
							}
							else
							{
								Destroy(mouseMovePointer);
								mouseMovePointer =	(GameObject)Instantiate(mouseMovePointerPrefab, hit.point, Quaternion.identity);
							}
						}
						else
						{
							//If the player click on a enemy or a player
							if (hit.transform.tag == "CreepTriggerTag"|| hit.transform.tag == "RedTeamTriggerTag"|| hit.transform.tag == "BlueTeamTriggerTag"|| hit.transform.tag == "RedPlayerTriggerTag"|| hit.transform.tag == "BluePlayerTriggerTag")
								networkView.RPC("SendAttackToServer", uLink.RPCMode.Server, hit.point, PlayerState.running, hit.transform.parent.name);
							
							if (hit.transform.tag == "RedPlayerTag"|| hit.transform.tag == "BluePlayerTag")
								networkView.RPC("SendAttackToServer", uLink.RPCMode.Server, hit.point, PlayerState.running, hit.transform.name);
						}
					}
				}
			}
		}
		//If a player hits B start recalling to base
		if (Input.GetKeyDown(KeyCode.B) && networkView.isMine == true  && !inputFieldGameChat.isFocused)
		{
			RecallBase();
		}

		//If a player press keycode Q it change the mouse state to attacking Q spell
		if (Input.GetKeyDown(KeyCode.Q) && networkView.isMine == true && playerStScript.stats.q.weaponLvl > 0 && !inputFieldGameChat.isFocused)
		{
			if (cdrUiScript.cdrQ < 0.1f   && playerStScript.mana >= playerStScript.stats.q.manaCost) {
				if (mouseProjector) {
					Destroy(mouseProjector);
				}
				mouseState = MouseState.attackingQ;
			}
			
		}
		//If a player press keycode W it change the mouse state to attacking W spell
		if (Input.GetKeyDown(KeyCode.W) && networkView.isMine == true  && playerStScript.stats.w.weaponLvl > 0 && !inputFieldGameChat.isFocused)
		{
			if (cdrUiScript.cdrW < 0.1f  && playerStScript.mana >= playerStScript.stats.w.manaCost) {
				mouseState = MouseState.attackingW;
				if (mouseProjector) {
					Destroy(mouseProjector);
				}
			}
			
		}
		
		//If a player press keycode E it change the mouse state to attacking E spell
		if (Input.GetKeyDown(KeyCode.E) && networkView.isMine == true  && playerStScript.stats.e.weaponLvl > 0 && !inputFieldGameChat.isFocused)
		{
			if (cdrUiScript.cdrE < 0.1f  && playerStScript.mana >= playerStScript.stats.e.manaCost) {
				mouseState = MouseState.attackingE;
				if (mouseProjector) {
					Destroy(mouseProjector);
				}
			}
			
		}
		//If a player press keycode R it change the mouse state to attacking R spell
		if (Input.GetKeyDown(KeyCode.R) && networkView.isMine == true  && playerStScript.stats.r.weaponLvl > 0 && !inputFieldGameChat.isFocused)
		{
			if (cdrUiScript.cdrR < 0.1f  && playerStScript.mana >= playerStScript.stats.r.manaCost) {
				mouseState = MouseState.attackingR;
				if (mouseProjector) {
					Destroy(mouseProjector);
				}
			}
			
		}
		


		switch (mouseState)
		{
		case MouseState.idle:
			
			break;
			
		case MouseState.attackingQ:
			CalculateProjectors(playerStScript.stats.q.weaponType, "Abilities/" + playerStScript.stats.prefabPath + "/Projectors/_Q", PlayerState.attackingQ);
			
			break;
			
		case MouseState.attackingW:
			CalculateProjectors(playerStScript.stats.w.weaponType, "Abilities/" + playerStScript.stats.prefabPath + "/Projectors/_W", PlayerState.attackingW);
			
			break;
			
		case MouseState.attackingE:
			CalculateProjectors(playerStScript.stats.e.weaponType, "Abilities/" + playerStScript.stats.prefabPath  + "/Projectors/_E", PlayerState.attackingE);
			
			break;
			
		case MouseState.attackingR:
			CalculateProjectors(playerStScript.stats.r.weaponType, "Abilities/" + playerStScript.stats.prefabPath  + "/Projectors/_R", PlayerState.attackingR);
			
			break;
		}
	}

	//THIS METHOD SEND ALL THE INFO OF THE PLAYER TO THE UI ELEMENTS
	void UpdateUIData(){

		PlayerDatabase playerDbScript = GameManager.GetComponent<PlayerDatabase>();
		playerDbScript.playerTeam = playerStScript.playerTeam;
		playerDbScript.playerName = this.gameObject.name;

		//UPDATE THE CDR UI SCRIPT WITH THE CDR VALUES
		cdrUiScript.cdrB = playerStScript.stats.basic.cdrTStamp - Time.time;
		cdrUiScript.cdrB = cdrUiScript.cdrB <= 0 ? cdrUiScript.cdrB = 0 : playerStScript.stats.basic.cdrTStamp - Time.time;
		
		
		cdrUiScript.cdrQ = playerStScript.stats.q.cdrTStamp - Time.time;
		cdrUiScript.cdrQ = cdrUiScript.cdrQ <= 0 ? cdrUiScript.cdrQ = 0 : playerStScript.stats.q.cdrTStamp - Time.time;
		cdrUiQScript.cdrActual = cdrUiScript.cdrQ;
		cdrUiQScript.cdrMax = playerStScript.stats.q.cdr;
		
		cdrUiScript.cdrW = playerStScript.stats.w.cdrTStamp - Time.time;
		cdrUiScript.cdrW = cdrUiScript.cdrW <= 0 ? cdrUiScript.cdrW = 0 : playerStScript.stats.w.cdrTStamp - Time.time;
		cdrUiWScript.cdrActual = cdrUiScript.cdrW;
		cdrUiWScript.cdrMax = playerStScript.stats.w.cdr;
		
		cdrUiScript.cdrE = playerStScript.stats.e.cdrTStamp - Time.time;
		cdrUiScript.cdrE = cdrUiScript.cdrE <= 0 ? cdrUiScript.cdrE = 0 : playerStScript.stats.e.cdrTStamp - Time.time;
		cdrUiEScript.cdrActual = cdrUiScript.cdrE;
		cdrUiEScript.cdrMax = playerStScript.stats.e.cdr;
		
		cdrUiScript.cdrR = playerStScript.stats.r.cdrTStamp - Time.time;
		cdrUiScript.cdrR = cdrUiScript.cdrR <= 0 ? cdrUiScript.cdrR = 0 : playerStScript.stats.r.cdrTStamp - Time.time;
		cdrUiRScript.cdrActual = cdrUiScript.cdrR;
		cdrUiRScript.cdrMax = playerStScript.stats.r.cdr;
		
		//UPDATE THE LEFT PANEL SCRIPT WITH THE DATA
		int totalAd = (int)playerStScript.stats.basic.ad + (int)playerStScript._adValueAdd;
		leftPanelScript.adText.text = totalAd.ToString();
		int totalAp =  (int)playerStScript.stats.basic.ap + (int)playerStScript._apValueAdd;
		leftPanelScript.apText.text = totalAp.ToString();
		int totalAdRes = (int)playerStScript.adRes  + (int)playerStScript.adResAdd;
		leftPanelScript.adResText.text = totalAdRes.ToString();
		int totalApRes = (int)playerStScript.apRes  + (int)playerStScript.apResAdd;
		leftPanelScript.apResText.text = totalApRes.ToString();
		
		
		leftPanelScript.goldText.text =  (int)playerStScript.gold + "";
		
		int totalSpeed = (int)playerStScript.speed + (int)playerStScript.speedAdd;
		leftPanelScript.speedText.text = totalSpeed.ToString();
		
		leftPanelScript.expText.text = (int)playerStScript.playerExp + " / " + (int)playerStScript.maxPlayerExp;
		
		leftPanelScript.expImage.fillAmount = playerStScript.playerExp / playerStScript.maxPlayerExp;
		
		if ( playerStScript.myHealth <= playerStScript.maxHealth)
			leftPanelScript.hpText.text = (int)playerStScript.myHealth + " / " + (int)playerStScript.maxHealth;
		
		if ( playerStScript.mana <= playerStScript.baseMana)
			leftPanelScript.manaText.text = (int)playerStScript.mana + " / " + (int)playerStScript.baseMana;
		
		
		string minutes = Mathf.Floor((float)uLink.Network.time / 60).ToString("00");
		string seconds = (uLink.Network.time % 60).ToString("00");
		leftPanelScript.timerText.text = minutes.ToString() + " / " + seconds.ToString();


		//UPDATE THE MANACDIMAGE TO SET THE FILL AMOUNT OF MANA
		manaCdImage.fillAmount = playerStScript.mana / playerStScript.baseMana;
		
		
		//UPDATE THE _UIHEALTH SCRIPT WITH THE HEALTH AND MAXHEALTH
		healthCdImage.fillAmount  = playerStScript.myHealth /playerStScript.maxHealth;
		
		//UPDATE SPELL LEVEL SCRIPT WITH THE ACTUAL VALUES
		spellLvlScript.qText.text = playerStScript.stats.q.weaponLvl.ToString();
		spellLvlScript.wText.text = playerStScript.stats.w.weaponLvl.ToString();
		spellLvlScript.eText.text = playerStScript.stats.e.weaponLvl.ToString();
		spellLvlScript.rText.text = playerStScript.stats.r.weaponLvl.ToString();
	}

	//USED TO GENERATE CUSTOM GIZMOS FOR EACH PLAYER OR ABILITY USED
	void CalculateProjectors(DmgDataclass.type abilityType, string ProjectPrefab, PlayerState StateAttacking)
	{
		RaycastHit hit;
		switch (abilityType)
		{

		case DmgDataclass.type.lockedShot_rangue:
				

			break;
		case DmgDataclass.type.skillShot_front:
			if (ProjectPrefab == "")
			{
				if (Physics.Raycast	(cam.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, floorMask)) {
					networkView.RPC("SendAttackToServer", uLink.RPCMode.Server, hit.point, StateAttacking, "");
				}
				
			}
			else
			{
				if (!mouseProjector)
				{
					mouseProjector = Instantiate(Resources.Load(ProjectPrefab, typeof(GameObject))) as GameObject;
				}
				if (Physics.Raycast	(cam.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, floorMask)) {
					Vector3 planePos = hit.point;
					planePos.y = transform.position.y;
					mouseProjector.transform.LookAt(planePos);
					mouseProjector.transform.position = transform.position;
					if (Input.GetMouseButtonDown(0))
					{
						mouseState = MouseState.idle;
						networkView.RPC("SendAttackToServer", uLink.RPCMode.Server, hit.point, StateAttacking, "");
						Destroy(mouseProjector);
					}
					if (Input.GetMouseButtonDown(1))
					{
						mouseState = MouseState.idle;
						Destroy(mouseProjector);
						
					}
					
					
				}
			}
			
			break;
		case DmgDataclass.type.skillShot_floor:
			if (ProjectPrefab == "")
			{
				if (Physics.Raycast	(cam.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, floorMask)) {
					networkView.RPC("SendAttackToServer", uLink.RPCMode.Server, hit.point, StateAttacking, "");
				}
				
			}
			else
			{
				if (!mouseProjector)
				{
					Debug.Log("loading Prefab");
					mouseProjector = Instantiate(Resources.Load(ProjectPrefab, typeof(GameObject))) as GameObject;
				}
				
				if (Physics.Raycast	(cam.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, floorMask)) {
					Vector3 planePos = hit.point;
					planePos.y = planePos.y + 0.03f;
					mouseProjector.transform.position = planePos;
					
					if (Input.GetMouseButtonDown(0))
					{
						mouseState = MouseState.idle;
						Destroy(mouseProjector);
						networkView.RPC("SendAttackToServer", uLink.RPCMode.Server, hit.point, StateAttacking, "");
					}
					if (Input.GetMouseButtonDown(1))
					{
						mouseState = MouseState.idle;
						Destroy(mouseProjector);
					}
					
				}
			}
			
			break;
		case DmgDataclass.type.aoe_instant:
			
			networkView.RPC("SendAttackToServer", uLink.RPCMode.Server, Vector3.zero, StateAttacking, "NoTarget");
			break;
		}
	}



	//THIS METHOD IS USED TO CALCULATE THE ATTACK COMMANDS OF THE PLAYER
	//AND CHANGE THE ANIMATION PARAMETERS TO THE ABILITY USED
	void CalculateAttack(DmgDataclass.type abilityType,  PlayerState StateAttacking, float range,float cdr_, float cdr_timestamp, float manaCost)
	{

		switch (abilityType)
		{
			
		case DmgDataclass.type.lockedShot_rangue:

			if (!playerStScript.charLocked && !playerStScript.abilityLocked)
			{
				if (targetObj)
				{
					if (targetObj.GetComponent<PlayerStats>().myHealth <=0)
					{
						targetObj = null;
						playerStatus = PlayerState.idle;
						return;
					}
				}
				else
				{
					playerStatus = PlayerState.idle;
				}
				if (Vector3.Distance(targetObj.transform.position,this.transform.position) < range)
				{
					agent.Stop();
					if (cdr_timestamp <= Time.time)
					{
						//DRAIN MANA FROM THE PLAYER
						playerStScript.mana = playerStScript.mana - manaCost;
						playerStScript.UpdateManaToClients();

						//ROTATE TO FACE THE TARGET
						Vector3 fromPosition = transform.position;
						fromPosition.y = 0;
						Vector3 toPosition = targetObj.transform.position;
						toPosition.y = 0; 
						Quaternion targetRotation = Quaternion.LookRotation (toPosition - fromPosition);
						transform.rotation = targetRotation;
						UpdateCdr( StateAttacking, cdr_);
						cdr_timestamp = Time.time + cdr_;
						playerStScript.charLocked = true;
						playerStScript.abilityLocked = true;
						animator.SetInteger("AnimType", (int)StateAttacking);
						playerSpellScript.AttackDestination = targetObj.transform.position;
						playerSpellScript.target_obj = targetObj;

						
					}
					else
					{
						//ROTATE TO FACE THE TARGET
						Vector3 fromPosition = transform.position;
						fromPosition.y = 0;
						Vector3 toPosition = targetObj.transform.position;
						toPosition.y = 0; 
						Quaternion targetRotation = Quaternion.LookRotation (toPosition - fromPosition);
						
						transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, 1);
						animator.SetInteger("AnimType", (int)PlayerState.idle);
					}
					
				}
				else
				{
					animator.SetInteger("AnimType", (int)PlayerState.running);
					MoveOrChase();
				}
				
				
			}


			
			break;
		case DmgDataclass.type.skillShot_front:
			if (!playerStScript.charLocked && !playerStScript.abilityLocked)
			{

				if (Vector3.Distance(destinationPos,this.transform.position) < range)
				{
					//ROTATE TO FACE THE TARGET
					Vector3 fromPosition = transform.position;
					fromPosition.y = 0;
					Vector3 toPosition = destinationPos;
					toPosition.y = 0; 
					Quaternion targetRotation = Quaternion.LookRotation (toPosition - fromPosition);
					agent.Stop();
					if (cdr_timestamp <= Time.time && playerStScript.mana >= manaCost)
					{
						//DRAIN MANA FROM THE PLAYER
						playerStScript.mana = playerStScript.mana - manaCost;
						playerStScript.UpdateManaToClients();
						
						transform.rotation = targetRotation;
						UpdateCdr( StateAttacking, cdr_);
						cdr_timestamp = Time.time + cdr_;
						playerStScript.charLocked = true;
						playerStScript.abilityLocked = true;
						animator.SetInteger("AnimType", (int)StateAttacking);
						playerSpellScript.AttackDestination = destinationPos;
						playerSpellScript.target_obj = targetObj;
						

					}
					else
					{

						transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, 1);
						animator.SetInteger("AnimType", (int)PlayerState.idle);
					}

				}
				else
				{
					animator.SetInteger("AnimType", (int)PlayerState.running);
					MoveOrChase();
				}
				
				
			}
			
			break;
		case DmgDataclass.type.skillShot_floor:

			if (!playerStScript.charLocked && !playerStScript.abilityLocked)
			{
				if (Vector3.Distance(destinationPos,this.transform.position) < range)
				{
					//ROTATE TO FACE THE TARGET
					Vector3 fromPosition = transform.position;
					fromPosition.y = 0;
					Vector3 toPosition = destinationPos;
					toPosition.y = 0; 
					Quaternion targetRotation = Quaternion.LookRotation (toPosition - fromPosition);
					agent.Stop();
					if (cdr_timestamp <= Time.time && playerStScript.mana >= manaCost)
					{
						//DRAIN MANA FROM THE PLAYER
						playerStScript.mana = playerStScript.mana - manaCost;
						playerStScript.UpdateManaToClients();
						
						transform.rotation = targetRotation;
						UpdateCdr( StateAttacking, cdr_);
						cdr_timestamp = Time.time + cdr_;
						playerStScript.charLocked = true;
						playerStScript.abilityLocked = true;
						animator.SetInteger("AnimType", (int)StateAttacking);
						playerSpellScript.AttackDestination = destinationPos;
						playerSpellScript.target_obj = targetObj;
												
					}
					else
					{

						transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, 1);
						animator.SetInteger("AnimType", (int)PlayerState.idle);
					}

				}
				else
				{
					animator.SetInteger("AnimType", (int)PlayerState.running);
					MoveOrChase();
				}
				
				
			}
			
			break;
		case DmgDataclass.type.aoe_instant:

			if (cdr_timestamp <= Time.time && playerStScript.mana >= manaCost)
			{
				//DRAIN MANA FROM THE PLAYER
				playerStScript.mana = playerStScript.mana - manaCost;
				playerStScript.UpdateManaToClients();
				UpdateCdr( StateAttacking, cdr_);
				cdr_timestamp = Time.time + cdr_;
				playerSpellScript.Attacking((int)StateAttacking);

			}
			
			break;

	
		}
	}

	//MOVE OR CHASE METHOD
	//USED TO CONTROL THE AGENT COMPONENT ATTACHED ON THE PLAYER
	public void MoveOrChase()
		{



		if (playerStScript.myHealth<=0)
		{
			agent.Stop();
			return;
		}
	
		agent.Resume();
		float finalSpeed = playerStScript.speed + ((playerStScript.speed * playerStScript.speedAdd) / 100);
		agent.speed = finalSpeed;
			
			
		if (targetObj)
		{
			agent.SetDestination(targetObj.transform.position);
			if (agent.velocity.normalized != Vector3.zero)
			transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
		}
		else
		{
			if (agent.velocity.normalized != Vector3.zero)
			transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
		}
			
			
		}

	//CALLED FROM THE ANIMATION EVENTS OF EACH PLAYER ANIMATION TO UNLOCK THE PLAYER
	//WHEN THE ABILITY ANIMATION IS ENDING
	void ExitAnim() 
	{
		playerStScript.charLocked = false;

	}


	//THIS METHOD IS RUNNING ON THE SERVER
	//CALCULATE IF THE PLAYER WILL MOVE TO ANY LOCATIO OR ATTACK AN ENEMY
	[PunRPC]
	void SendAttackToServer (Vector3 newPosition, PlayerState state, string targetName)
	{
		if (!playerStScript.charLocked && !playerStScript.abilityLocked && playerStScript.myHealth > 0)
		{
			if (teleportObj)
			{
				Destroy(teleportObj);
				networkView.RPC("ShowTeleportParticle", uLink.RPCMode.Others, false);
			}

			playerStatus = state;

			if (playerStatus == PlayerState.running) {
				if (targetName == "")
				{
					targetObj = null;
					agent.SetDestination(newPosition);
					playerStatus = PlayerState.running;
				}
				else
				{

					targetObj = GameObject.Find(targetName);
					playerStatus = PlayerState.attackingBasic;
				}	
			}

			if (playerStatus == PlayerState.attackingQ && targetName != "NoTarget") {
			agent.SetDestination(newPosition);
			destinationPos = newPosition;
			}
			if (playerStatus == PlayerState.attackingW && targetName != "NoTarget") {
				agent.SetDestination(newPosition);
				destinationPos = newPosition;
			}

			if (playerStatus == PlayerState.attackingE && targetName != "NoTarget") {
				agent.SetDestination(newPosition);
				destinationPos = newPosition;
			}

			if (playerStatus == PlayerState.attackingR && targetName != "NoTarget") {
				agent.SetDestination(newPosition);
				destinationPos = newPosition;

			}

		}
	}

	//THIS METHOD IS USED TO UPDATE THE CURRENT CDR OF THE SERVER TO THE OWNER PLAYER
	[PunRPC]
	void UpdateCdr (PlayerState state, float cdrFromServer)
	{
	if (uLink.Network.isServer)
	{
		networkView.RPC("UpdateCdr", uLink.RPCMode.Owner, state, cdrFromServer);
	}
		switch (state)
		{
		case PlayerState.attackingBasic:
			playerStScript.stats.basic.cdr = cdrFromServer;
			playerStScript.stats.basic.cdrTStamp = Time.time + playerStScript.stats.basic.cdr;
			if(basicClip)
			GetComponent<AudioSource>().PlayOneShot(basicClip);
		break;

		case PlayerState.attackingQ:
			playerStScript.stats.q.cdr = cdrFromServer;
			playerStScript.stats.q.cdrTStamp = Time.time + playerStScript.stats.q.cdr;
			if(qClip)
				GetComponent<AudioSource>().PlayOneShot(qClip);
		break;

		case PlayerState.attackingW:
			playerStScript.stats.w.cdr = cdrFromServer;
			playerStScript.stats.w.cdrTStamp = Time.time + playerStScript.stats.w.cdr;
			if(wClip)
				GetComponent<AudioSource>().PlayOneShot(wClip);
		break;

		case PlayerState.attackingE:
			playerStScript.stats.e.cdr = cdrFromServer;
			playerStScript.stats.e.cdrTStamp = Time.time + playerStScript.stats.e.cdr;
			if(eClip)
				GetComponent<AudioSource>().PlayOneShot(eClip);
		break;

		case PlayerState.attackingR:
			playerStScript.stats.r.cdr = cdrFromServer;
			playerStScript.stats.r.cdrTStamp = Time.time + playerStScript.stats.r.cdr;
			if(rClip)
				GetComponent<AudioSource>().PlayOneShot(rClip);
		break;
		}
	}

	//RECALL METHOD
	public void RecallBase() 
	{
		networkView.RPC("RecallBaseRPC", uLink.RPCMode.Server);
	}


	[PunRPC]
	public IEnumerator RecallBaseRPC() 
	{

		if (!playerStScript.abilityLocked && !playerStScript.charLocked && !teleportObj)
		{



			if (uLink.Network.isServer)
			{
				Debug.Log("particle created");
				teleportObj = Instantiate(teleportParticle, new Vector3(transform.position.x, 0.1f, transform.position.z), Quaternion.Euler(-90, 0, 0)) as GameObject;
				networkView.RPC("ShowTeleportParticle", uLink.RPCMode.Others, true);
				playerStatus = PlayerState.recall;
				agent.Stop();
				networkView.RPC("RecallBaseRPC", uLink.RPCMode.Others);
				//animator.SetInteger("AnimType", (int)PlayerState.idle);

			}
			else
			{
				GetComponent<AudioSource>().PlayOneShot(TeleportClip);
			}

			yield return new WaitForSeconds(4f);

			if (uLink.Network.isServer && teleportObj)
			{
				Destroy(teleportObj);
				networkView.RPC("ShowTeleportParticle", uLink.RPCMode.Others, false);
				playerStatus = PlayerState.idle;
				animator.SetInteger("AnimType", (int)PlayerState.idle);
				agent.Warp(initialPos);
				transform.rotation = initialRotation;
				agent.ResetPath();
			}
		}
		
	}

	[PunRPC]
	public IEnumerator ImDead() 
	{

		playerStatus = PlayerState.dead;
		//Transform trigger = transform.FindChild("Trigger");
		animator.SetInteger("AnimType", (int)PlayerState.dead);
		if (uLink.Network.isServer)
		{
			agent.enabled = true;
		}
		else
		{
			if (deadClip)
			GetComponent<AudioSource>().PlayOneShot(deadClip);
		}
		yield return new WaitForSeconds(playerStScript.respawnTime);

		if (uLink.Network.isServer)
		{
			agent.enabled = true;
			agent.Warp(initialPos);
		}
		Respawn();
		playerStScript.abilityLocked = false;
		playerStScript.charLocked = false;
		targetObj = null;
		playerStatus = PlayerState.idle;
		animator.SetInteger("AnimType", (int)PlayerState.idle);

		transform.rotation = initialRotation;
		
	}

	[PunRPC]
	public void Respawn ()
	{
		
		if (uLink.Network.isServer)
		{
			playerStScript.myHealth = playerStScript.maxHealth;
			playerStScript.mana = playerStScript.baseMana;
			playerStScript.destroyed = false;
			playerStScript.UpdateStatsOnClients();
		}
		
	}

	[PunRPC]
	public void ShowTeleportParticle (bool create)
	{
		Debug.Log("create tp " + create);
		if (create)
		{
			GetComponent<AudioSource>().PlayOneShot(TeleportClip);
			teleportObj = Instantiate(teleportParticle, new Vector3(transform.position.x, 0.1f, transform.position.z), Quaternion.Euler(-90, 0, 0)) as GameObject;
		}
			else
		{
		Destroy(teleportObj);
		}
		
	}

	[PunRPC]
	void ShowLvlUpParticle ()
	{
		
		GameObject lvlUpObj = Instantiate(particleScript.lvlUpParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0)) as GameObject;
		ParticleFollow pScript = lvlUpObj.GetComponent<ParticleFollow>();
		pScript.originatorObj = this.gameObject;
		pScript.offsetY = 0.1f;
		
	}
	


	
}