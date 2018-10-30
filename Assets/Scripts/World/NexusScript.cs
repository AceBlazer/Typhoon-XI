using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NexusScript : MonoBehaviour {

	public PlayerStats playerStScript;
	private PlayerDatabase playerDataScript;
	public Sprite redWinSprite;
	public Sprite blueWinSprite;
	public GameObject winObj;
	public AudioClip winClip;
	public AudioClip loseClip;
	public AudioClip quakeClip;
	public GameObject explotionObj;

	private bool expended;

	// Use this for initialization
	void Start () {
		playerStScript = GetComponent<PlayerStats>();

		//FIND GAMEMAGER AND ACCESS THE DMGDATABASE SCRIPT
		GameObject GameManager = GameObject.Find("GameManager_mn");
		playerDataScript = GameManager.GetComponent<PlayerDatabase>();
		if (this.gameObject.tag == "RedTeamTag")
		{
			playerStScript.playerTeam = "red";
		}
		else
		{
			playerStScript.playerTeam = "blue";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (playerStScript.myHealth <=0 && !expended)
		{
			GetComponent<Animator>().SetBool("destroyed", true);
			LockCamera();
			expended = true;
		}


	}

	void WinEvent () {
		winObj.GetComponent<CanvasGroup>().alpha = 1;
		if (playerStScript.playerTeam == "red")
		{
			winObj.GetComponent<Image>().sprite = blueWinSprite;

		}
		else
		{
			winObj.GetComponent<Image>().sprite = redWinSprite;
		}
	

		if (playerDataScript.playerTeam == playerStScript.playerTeam)
		{
			GetComponent<AudioSource>().PlayOneShot(loseClip);
		}
		else
		{
			GetComponent<AudioSource>().PlayOneShot(winClip);
		}
		Instantiate(explotionObj, transform.position, Quaternion.identity);

		StartCoroutine(EndMatch());
	}

	void LockCamera () {
		GameObject cameraObj = GameObject.Find("Custom_Camera");
		cameraObj.GetComponent<AudioSource>().PlayOneShot(quakeClip);
		CustomCamera cameraScript = cameraObj.GetComponent<CustomCamera>();
		cameraScript.sources.target = this.transform;
		cameraScript.config.cameraActive = true;
		cameraScript.config.cameraLocked = true;

		
	}


	public IEnumerator EndMatch ()
	{
		yield return new WaitForSeconds(5);
		Application.Quit();
	}
}
