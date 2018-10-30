using UnityEngine;
using System.Collections.Generic;

public class Turretcs : MonoBehaviour 
{

	private List<GameObject> enemyUnits = new List<GameObject> ();
	private List<GameObject> friendlyUnits = new List<GameObject> ();
	private Transform currentTarget;
	GameObject cachedTarget;

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag.Contains (this.tag) && col.gameObject.tag != "Terrain") {
			enemyUnits.Add (col.gameObject);
		} else if (col.gameObject.tag != "Terrain" && col.gameObject.tag.Contains (this.tag)) {
			friendlyUnits.Add (col.gameObject);
		} 
	}

	void OnCollisionExit (Collision col)
	{
		if (col.gameObject.tag.Contains (this.tag) && col.gameObject.tag != "Terrain") {
			if (cachedTarget == col.gameObject) {
				currentTarget = null;
			}
			enemyUnits.Remove (col.gameObject);
		} else if (col.gameObject.tag != "Terrain" && col.gameObject.tag.Contains (this.tag)) {
			friendlyUnits.Remove (col.gameObject);
		} 
	}

	void Update () 
	{
		if (enemyUnits.Count > 0) {
			if (currentTarget != null) {
				//target CurrentTarget
			} else {
				for (int i = 0; i < enemyUnits.Count; i++) {
					if (enemyUnits [i].name.Contains ("GOBLIN")) {
						//target Minion
						currentTarget = enemyUnits [i].transform;
						cachedTarget = currentTarget.gameObject;
						return;
					}
				}
				if (currentTarget == null) {
					currentTarget = enemyUnits [0].transform;
				}
			}
		}
	}
	public void DamageDealt (string PlayerName)
	{
		//Target Player
		currentTarget = GameObject.Find (PlayerName).transform;
	}
}