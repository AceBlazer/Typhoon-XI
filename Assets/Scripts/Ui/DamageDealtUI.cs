using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageDealtUI : MonoBehaviour {

	public Transform myTransform;
	
	public Vector3 worldPosition  = new Vector3();
	
	public Vector3 screenPosition  = new Vector3();
	
	public Vector3 screenToPosition  = new Vector3();
	
	public Vector3 cameraRelativePosition = new Vector3();

	public Vector3 cameraRelativePositionFinal = new Vector3();

	public string reciever;

	public Camera uiCamera;

	public Transform enemy;

	public float totalDmg;

	public bool dealingDamage = true;

	private float expireTime = 2;

	//new on gui
	private float minimumZ = 1.5f;
	private float adjustment = 1.5F;
	private float offset;
	private int labelTop = 18;
	
	private int labelWidth = 110;
	
	private int labelHeight = 15;

	public Font font;
	
	private GUIStyle myStyle = new GUIStyle();

	public float alpha = 255;
	// Use this for initialization
	void Start () {

		uiCamera = GameObject.Find("MainCamera_mn").GetComponent<Camera>();
		enemy = GameObject.Find(reciever).transform;



		StartCoroutine(DestroyMyselfAfterSomeTime());

		myStyle.font = font;
		
		myStyle.fontSize = 12;
		
		myStyle.fontStyle = FontStyle.Bold;
		
		
		//Allow the text to extend beyond the width of the label.
		
		myStyle.clipping = TextClipping.Overflow;
		myStyle.alignment = TextAnchor.MiddleCenter;


	}
	
	// Update is called once per frame
	void Update () {
	


		cameraRelativePosition = uiCamera.transform.InverseTransformPoint(enemy.position);

	
	}

	void OnGUI ()
	{
		//Only display the player's name if they are in front of the camera and also the 
		//player should be in front of the camera by at least minimumZ.
		
		if(cameraRelativePosition.z > minimumZ)
		{
			//Set the world position to be just a bit above the player.
			worldPosition = new Vector3(enemy.position.x, enemy.position.y + adjustment,
			                            enemy.position.z);
			
			//Convert the world position to a point on the screen.

			screenPosition = uiCamera.WorldToScreenPoint(worldPosition);
			
			

			offset += 0.2f;

			alpha -= 1f;

			if (dealingDamage == false)
			{
				myStyle.normal.textColor = new Color(255,0,0,alpha);
			}
			else
			{
				myStyle.normal.textColor = new Color(255,255,0,alpha);
			}
			string stringToShow = "+" + totalDmg.ToString();
			GUI.Label(new Rect(screenPosition.x - labelWidth / 2,
			                   Screen.height - screenPosition.y - labelTop - offset,
			                   labelWidth, labelHeight), stringToShow, myStyle);
			
			
			
		}
	}

	IEnumerator DestroyMyselfAfterSomeTime()
	{
		yield return new WaitForSeconds (expireTime);
		Destroy(this.gameObject);
		
	}
}
