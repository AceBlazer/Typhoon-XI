using UnityEngine;
using System.Collections;



public class StunObj : MobaStormAbility {
	
	//Variables Start_________________________________________________________	
	


	protected override void OnStart ()
	{

		MobaStunObj(destinationGameObj,	ad);
	}

	public void OnDestroy () {
	//HERE YOU CAN ADD ANY LINE WHEN THE BUFF IS OVER
	}


	void Update () 
	{
		if (!spellActive)
			return;



		//transform.Rotate(Vector3.up * (rotateSpeed + Time.deltaTime));
		transform.position = destinationGameObj.transform.position;
	}

	protected override IEnumerator DestroyMyselfAfterSomeTime()
	{
		
		
		yield return new WaitForSeconds (expireTime);

		
	}


}
