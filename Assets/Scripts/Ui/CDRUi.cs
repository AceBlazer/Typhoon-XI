using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CDRUi : MonoBehaviour {
	private Camera myCamera;
	
	public Transform myTransform;
	
	public Vector3 worldPosition  = new Vector3();
	
	public Vector3 screenPosition  = new Vector3();
	
	public Vector3 screenToPosition  = new Vector3();
	
	public Vector3 cameraRelativePosition = new Vector3();
	
	public Vector3 cameraRelativePositionFinal = new Vector3();

	public float cdrB;
	public float cdrQ;
	public float cdrW;
	public float cdrE;
	public float cdrR;
	public int MyLvl;
	
	public int MyexpToNext;
	
	public Camera uiCamera;
	
	public Transform enemy;
	
	public float totalDmg;
	
	public bool dealingDamage = true;
	
	private float expireTime = 5;

	private Text text;
	
	// Use this for initialization
	void Start () {
		
	
		text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		

		text.text = "Basic" + cdrB.ToString() + "\n" + "Q" + cdrQ.ToString() + "\n" + "W" + cdrW.ToString() + "\n" + "E" + cdrE.ToString() + "\n" + "R" + cdrR.ToString();

	}
	
	
	IEnumerator DestroyMyselfAfterSomeTime()
	{

		yield return new WaitForSeconds (expireTime);
		Destroy(this.gameObject);
		
	}
}