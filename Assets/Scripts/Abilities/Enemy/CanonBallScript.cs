using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CanonBallScript : MobaStormAbility {

	public float firingAngle 	= 45.0f;
	public float gravity 		= 14f;
	private float blastRadius 	= 5;
	private Transform myTransform;

	protected override void OnStart ()
	{
		myTransform = transform;
		StartCoroutine(SimulateProjectile(myTransform));
	}

	public void OnDestroy () {
	
	}

	// Update is called once per frame
	void Update () 
	{

		if (!spellActive)
			return;

		//Debug.DrawRay(myTransform.position, new Vector3(0,-0.5f,0), Color.green);
		if (transform.position.y < 0.50f && uLink.Network.isServer && !expended)
		{	
			expended = true;

			List<Collider> struckObjects = new List<Collider>(Physics.OverlapSphere(transform.position, blastRadius));
			
			foreach(Collider objectsHit in struckObjects)
			{
				//Used to deal dmg on the target
				MobaDealDmgTriggerObj(objectsHit.gameObject);
			}

			Vector3 floorPosition = transform.position;
			floorPosition.y = floorPosition.y - 0.2f;
			uLink.Network.Destroy(this.gameObject);

		}
	}
	
	protected override IEnumerator DestroyMyselfAfterSomeTime() 
	{
		yield return new WaitForSeconds(expireTime);	
		
		uLink.Network.Destroy(gameObject);
	}

	IEnumerator SimulateProjectile(Transform Projectile)
	{
		// Short delay added before Projectile is thrown
		//yield return new WaitForSeconds(0.1f);
		Projectile = transform;
		myTransform = transform;
		// Move projectile to the position of throwing object + add some offset if needed.
		Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);
		
		// Calculate distance to target
		float target_Distance = Vector3.Distance(Projectile.position, floorPos);
		
		// Calculate the velocity needed to throw the object to the target at specified angle.
		float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
		
		// Extract the X  Y componenent of the velocity
		float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
		float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
		
		// Calculate flight time.
		float flightDuration = target_Distance / Vx;
		
		// Rotate projectile to face the target.
		Projectile.rotation = Quaternion.LookRotation(floorPos - Projectile.position);
		
		float elapse_time = 0;
		
		while (elapse_time < flightDuration && expended == false)
		{
			Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
			
			elapse_time += Time.deltaTime;
			
			yield return null;
		}


	}  
}
