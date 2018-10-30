using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	// Use this for initialization

	public Menu CurrentMenu;
	public GameObject loadingScreenObj;


	public void Start () {
		ShowMenu(CurrentMenu);
	}

	public void LoadingScreens (bool status) {
		loadingScreenObj.SetActive(status);
	}



	public void ShowMenu(Menu menu)
	{
		if (CurrentMenu != null)
			CurrentMenu.IsOpen = false;

		CurrentMenu = menu;
		CurrentMenu.IsOpen = true;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
