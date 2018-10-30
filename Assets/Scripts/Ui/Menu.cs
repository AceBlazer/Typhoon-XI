using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	private Animator animator;

	private CanvasGroup canvasGroup;

	public GameObject MainMenu;

	public Text errorDialogText;

	public bool IsOpen
	{
		get {return animator.GetBool("IsOpen"); }
		set { animator.SetBool("IsOpen", value); }

	}
	
	// Update is called once per frame
	public void Awake () {
		animator = GetComponent<Animator>();
		canvasGroup = GetComponent<CanvasGroup>();

	
	}
	public void Update()
	{
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open") && MainMenu.GetComponent<MenuManager>().loadingScreenObj.activeSelf == false)
		{
			canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
		}
		else
		{
			canvasGroup.blocksRaycasts = canvasGroup.interactable = false;
		}
	}

}
