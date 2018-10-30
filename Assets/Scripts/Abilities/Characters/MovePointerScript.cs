using UnityEngine;
using System.Collections;

//This script is used to rotate and destroy the mouse pointer after the expire time
public class MovePointerScript : MonoBehaviour {
	public float expireTime = 1.5f;
	public bool destroyThis;
	// Use this for initialization
	void Start () {
		if (destroyThis)
		StartCoroutine(DestroyMyselfAfterSomeTime());
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
	}

	IEnumerator DestroyMyselfAfterSomeTime()
	{
		yield return new WaitForSeconds (expireTime);
		Destroy(this.gameObject);
		
	}
}
