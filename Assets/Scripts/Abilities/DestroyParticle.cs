using UnityEngine;
using System.Collections;

public class DestroyParticle : MonoBehaviour {

	public float time;
	// Use this for initialization
	void Start () {
	
		StartCoroutine(DestroyMyself());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator DestroyMyself()
	{
		
		yield return new WaitForSeconds (time);

		Destroy(this.gameObject);		
	
		
	}
}
