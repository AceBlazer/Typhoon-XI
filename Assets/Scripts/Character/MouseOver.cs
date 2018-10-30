using UnityEngine;
using System.Collections;

public class MouseOver : MonoBehaviour {
	public GameObject mesh;
	public int dataScript=0;
	private Renderer rend;
	// Use this for initialization
	void Start () {
		rend = mesh.GetComponent<Renderer>();

		 dataScript = GetComponent<gamemanager1>().teamm;
	}
	
	// Update is called once per frame
	void Update () {


	}

	void OnMouseOver () {
		if (dataScript == 0)
		{
			if (gameObject.tag == "BlueTeamTriggerTag")
				rend.material.SetColor("_OutColor", Color.cyan);
			if (gameObject.tag == "RedTeamTriggerTag")
				rend.material.SetColor("_OutColor", Color.red);
			if (gameObject.tag == "RedPlayerTriggerTag")
				rend.material.SetColor("_OutColor", Color.red);

		}

		if (dataScript == 1)
		{
			if (gameObject.tag == "RedTeamTriggerTag")
				rend.material.SetColor("_OutColor", Color.cyan);

			if (gameObject.tag == "BlueTeamTriggerTag")
				rend.material.SetColor("_OutColor", Color.red);
			if (gameObject.tag == "BluePlayerTriggerTag")
				rend.material.SetColor("_OutColor", Color.red);

		}

		if (gameObject.tag == "CreepTriggerTag")
			rend.material.SetColor("_OutColor", Color.red);


	}

	void OnMouseExit () {
		
		rend.material.SetColor("_OutColor", Color.black);
	}
}
