using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour {
	public GameObject spawnParticle;
	public GameObject teleportParticle;
	[HideInInspector] public GameObject teleportObj;
	public GameObject lvlUpParticle;
	public GameObject healthParticle;
	public GameObject manaParticle;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	public void CreateSpawnParticle(Transform objTransform)
	{
		Instantiate(spawnParticle, new Vector3(objTransform.position.x, 0.1f, objTransform.position.z), Quaternion.identity);
	}

	public GameObject CreateTeleportParticle(Transform objTransform)
	{
		GameObject teleportObj = Instantiate(teleportParticle, new Vector3(objTransform.position.x, 0.1f, objTransform.position.z), Quaternion.Euler(-90, 0, 0)) as GameObject;
	
		return teleportObj;
	}
	public void DestroyTeleportParticle()
	{

		if (teleportObj)
		{
			Destroy(teleportObj);		
		}

	}
}
