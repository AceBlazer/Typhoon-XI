/// <summary>
/// AI character controller.
/// Just A basic AI Character controller
/// will looking for a Target and moving to and Attacking
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof (NavMeshAgent))]
public class CreepScript : uLink.MonoBehaviour { 
	//UPDATE RATE OF THE AGENT.SETDESTINATION
	private float updateTargetRate = 10F;

	//VARIABLE USED TO STORE THE CURRENT TARGET OBJ
	private GameObject targetObj;

	//USED TO STORE THE TRIGGER GAMEOBJ
	public GameObject trigger;

	//OBJ TO DROP WHEN THIS CREEP DIE
	public GameObject dropObj;

	//STATE OF THE CREEP
	public enum State
	{
		idle,
		
		chasing,
		
		attackingMelee,
		
		attackingRangue, 

		retreat,

		dead
		
		
	}
	//CURRENT STATE OF THE CREEP
	public State creepStatus = State.idle;

	//ATTACK SPEED RATE
	public float attackSpeed = 3;
	private float timeAttack = 0;

	//COMPONETS VARIABLE
	private Animator animator;

	//DISTANCE FROM THE ENEMY AND MAXIMUN DISTANCE ALLOWED
	private float distanceFromPlayer;
	public float attackRange = 2;
	public float maxDistance = 6;

	//PLAYERSTATS SCRIPT THAT STORES ALL THE STATS ABOUT THIS OBJ
	private PlayerStats playerStatsScript;

	//CALCULATE POSITION AND ROTATIONS
	private float distanceFromOrigin;
	private Quaternion initialRotation;
	private Vector3 objectPos;
	private Vector3 initialPos;


	//AUDIO CLIPS
	private AudioSource audioSource;
	public AudioClip attackingSound;
	public AudioClip chasingSound;
	public AudioClip talkingSound;

	//agent variables
	public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
	private Vector3 move;
	private float m_MovingTurnSpeed = 360;
	private float m_StationaryTurnSpeed = 1000;
	float m_TurnAmount;
	float m_ForwardAmount;

	//VARIABLES USED TO STORE THE ENEMY PATH AND THE PATH TO THE INITIAL POSITION
	private NavMeshPath enemyPosPath;
	private NavMeshPath initPosPath;
	
	void Start () {

		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		playerStatsScript = GetComponent<PlayerStats>();
		agent = GetComponentInChildren<NavMeshAgent>();

		agent.updateRotation = false;
		agent.updatePosition = true;
		initialPos = transform.position;
		initialRotation = transform.rotation;
		enemyPosPath = new NavMeshPath();
		initPosPath = new NavMeshPath();
	}
	

	void PlayTalkSound()
	{
		if (creepStatus == State.idle)
		audioSource.PlayOneShot(talkingSound, 0.7f);

		Invoke("PlayTalkSound", Random.Range(5, 20));
	}
	void OnTriggerEnter(Collider other)
	{
		if (playerStatsScript.playerTeam == "boss")
		{
			if(other.tag == "RedPlayerTag" || other.tag == "BluePlayerTag")
			{			
				networkView.RPC("PlayClientSound", uLink.RPCMode.Others, 1);
				targetObj = other.gameObject;
			}
		}
		if (playerStatsScript.playerTeam == "blue")
		{
			if(other.tag == "RedPlayerTag")
			{			

				targetObj = other.gameObject;

			}
		}
		if (playerStatsScript.playerTeam == "red")
		{
			if(other.tag == "BluePlayerTag")
			{			

				targetObj = other.gameObject;
			}
		}

	
	}


	private float PathLength(NavMeshPath path) {
		// The length is implicitly zero if there aren't at least
		// two corners in the path.
		if (path.corners.Length < 2)
			return 0;
		
		var previousCorner = path.corners[0];
		float lengthSoFar = 0;
		
		// Calculate the total distance by adding up the lengths
		// of the straight lines between corners.
		for (var i = 1; i < path.corners.Length; i++) {
			var currentCorner = path.corners[i];
			lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
			previousCorner = currentCorner;
		}
		
		return lengthSoFar;
	}

	
	void Update () 
	{
		if (uLink.Network.isClient)
		{
			GetComponent<SphereCollider>().enabled = false;
		}

		if (playerStatsScript.abilityLocked && playerStatsScript.myHealth > 0)
		{
			if (uLink.Network.isServer)
			{
				agent.Stop();
				creepStatus = State.idle;
			}
			animator.enabled = false;
			return;
		}
		else
		{
			animator.enabled = true;
		}


		if (uLink.Network.isServer)
		{
			

		
			//CALCULATE ATTACKING TIME
			timeAttack += Time.deltaTime;
			
			var direction = Vector3.zero;
			var myPos = this.gameObject.transform.position;
		

			
			if(targetObj)
				
			{

				objectPos = targetObj.transform.position;
				objectPos.y = myPos.y;
				agent.CalculatePath(objectPos, enemyPosPath);
				distanceFromPlayer = PathLength(enemyPosPath);


				

				if (distanceFromPlayer > maxDistance +2)
					targetObj = null;
			}

			//Case for all the states of the creep enemy
			//This is used to control the animations and attacks
			switch (creepStatus)
			{
			case State.idle:

				if (playerStatsScript.myHealth < playerStatsScript.maxHealth && Vector3.Distance(transform.position, initialPos) < 0.2);
				{
					playerStatsScript.myHealth = playerStatsScript.maxHealth;
					playerStatsScript.UpdateHealthClients();
				}
				if (uLink.Network.isServer)
				agent.Stop();
				//If a player is detected in this range, the enemy will start chasing
				if(distanceFromPlayer<=maxDistance - 2 && targetObj) {
				creepStatus = State.attackingMelee;
				}

				transform.rotation = initialRotation;
				animator.SetInteger("AnimType", (int)State.idle);

				break;
			case State.dead:

				agent.Stop();
				animator.SetInteger("AnimType", (int)State.dead);
			
				break;
			case State.attackingMelee:

				if (!targetObj)
				{
					creepStatus = State.retreat;
					return;
				}
				else
				{
					PlayerStats enemyPlayerStatsScript = targetObj.GetComponent<PlayerStats>();
					if (enemyPlayerStatsScript.myHealth <=0)
					{
						creepStatus = State.retreat;
						return;
					}
					transform.LookAt(targetObj.transform.position);
				}

				if (!playerStatsScript.charLocked)
				{



					if(distanceFromPlayer<=attackRange) {
						if (timeAttack > attackSpeed)
						{

							audioSource.PlayOneShot(attackingSound, 0.7f);
							agent.Stop();
							animator.SetInteger("AnimType", (int)State.attackingMelee);
							playerStatsScript.charLocked = true;
							timeAttack = 0;
						}
						else
						{
							agent.Stop();
							animator.SetInteger("AnimType", (int)State.idle);
						}
					}
					else
					{
						animator.SetInteger("AnimType", (int)State.chasing);

						
						MoveOrChase();
					}

					agent.CalculatePath(initialPos, initPosPath);
					distanceFromOrigin = PathLength(initPosPath);

					if (distanceFromOrigin > maxDistance)

					
					

						if(distanceFromPlayer>=maxDistance || distanceFromOrigin > maxDistance)
						{	
							
							targetObj = null;
							agent.SetDestination(initialPos);
							creepStatus = State.retreat;

						}
				}
				break;
			}



			if (creepStatus == State.retreat)
			{
				agent.CalculatePath(initialPos, initPosPath);
				distanceFromOrigin = PathLength(initPosPath);

				if (distanceFromOrigin > 0.5f)
				{
					targetObj = null;
				
					animator.SetInteger("AnimType", (int)State.chasing);

					direction = this.transform.forward;
					direction.Normalize();


					MoveOrChase();
				}
				else
				{
					playerStatsScript.myHealth = playerStatsScript.maxHealth;
					playerStatsScript.UpdateHealthClients();
					creepStatus = State.idle;
				}
			}






		}
		else if (uLink.Network.isClient)
		{
			agent.enabled = false;
		}


	}

	void MoveOrChase()
	{
			agent.Resume();

			updateTargetRate++;
			if (updateTargetRate > 5)
			{
				if (targetObj != null)
				{
					updateTargetRate = 0;
					agent.SetDestination(targetObj.transform.position);
				}
				else
				{
					updateTargetRate = 0;
					agent.SetDestination(initialPos);
				}
			}

	
		
		
		move = agent.desiredVelocity;
		if (move.magnitude > 1f) move.Normalize();
		move = transform.InverseTransformDirection(move);
		move = Vector3.ProjectOnPlane(move, Vector3.zero);
		m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_ForwardAmount = move.z;
	
		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		transform.LookAt(agent.nextPosition);

		
	}
	//Called on animation events to unlock the character when the animation ends
	void ExitAnim() 
	{
		playerStatsScript.charLocked = false;
		
	}

	void Attacking(State type) {
		audioSource.PlayOneShot(attackingSound, 0.7f);
		if (!uLink.Network.isServer)
			return;
	

		switch (type)
		{
		case State.attackingMelee:

			animator.SetInteger("AnimType", 0);
			playerStatsScript.charLocked = false;
			break;
		}

	
		PlayerStats enemyPlayerStatsScript = targetObj.GetComponent<PlayerStats>();
		enemyPlayerStatsScript.DrainHealth(playerStatsScript._bAdValue, playerStatsScript._bApValue, transform.name, this.gameObject, this.gameObject.tag, CharacterClass.charName.Empty);

	}

	//This respawn the character on the server
	[PunRPC]
	public void Respawn ()
	{

		if (uLink.Network.isServer)
		{
			playerStatsScript.maxHealth *= 2;
			playerStatsScript._bAdValue *= 2;
			playerStatsScript._bApValue *= 2;
			playerStatsScript.adRes *= 1.5f;
			playerStatsScript.apRes *= 1.5f;
			playerStatsScript.myHealth = playerStatsScript.maxHealth;
			playerStatsScript.playerLvl++;
			playerStatsScript.destroyed = false;
			//playerStatsScript.UpdateDataOnClients();
		
		}
		
	}

	[PunRPC]
	public void PlayClientSound (int type)
	{
		if (type == 1)
			audioSource.PlayOneShot(chasingSound, 0.7f);

	}

	[PunRPC]
	public IEnumerator ImDead() 
	{
		if (uLink.Network.isServer && dropObj)
		{
			Vector3 dropPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
			uLink.Network.Instantiate(uLink.NetworkPlayer.server, dropObj,dropObj,dropObj, dropPos, Quaternion.identity, 0);

		}
		creepStatus = State.dead;
		trigger.GetComponent<BoxCollider>().enabled = false;
		animator.SetInteger("AnimType", (int)State.dead);
		yield return new WaitForSeconds(playerStatsScript.respawnTime);
		targetObj = null;
		trigger.GetComponent<BoxCollider>().enabled = true;
		creepStatus = State.idle;
		animator.SetInteger("AnimType", (int)State.idle);
		if (uLink.Network.isServer)
		{
			agent.Warp(initialPos);
			agent.ResetPath();
			transform.rotation = initialRotation;
			Respawn();
		}

		
		
		
	}


}
