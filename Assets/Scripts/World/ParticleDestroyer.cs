using UnityEngine;
using System.Collections;

public class ParticleDestroyer : MonoBehaviour {
	void Start()
	{
		StartCoroutine("DestroyMe");
	}
	
	IEnumerator DestroyMe()
	{
		yield return new WaitForSeconds(2);
		Destroy(gameObject);
	}
	// Use this for initialization

}
