using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour {

	private GameObject playerPickObj;

	private PickSelection pickScript;

	void Start () {
		playerPickObj = GameObject.Find("Window_PickSelection_mn");
		pickScript	= playerPickObj.GetComponent<PickSelection>();
	}

	public void Ready()
	{

		pickScript.Ready();


	}
	// Update is called once per frame
	void Update () {
	
	}
}
