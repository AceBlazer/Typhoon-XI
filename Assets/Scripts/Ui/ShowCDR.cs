using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ShowCDR : MonoBehaviour {
	public Image uiSprite;
	public float cdrActual;
	public float cdrMax;
	private float cdrUiShow;
	public bool active = false;
	
	// Use this for initialization
	void Start () {
		uiSprite = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!active)
			return;

		cdrUiShow = cdrActual / cdrMax;
		cdrUiShow = -cdrUiShow;
		cdrUiShow += 1;
		uiSprite.fillAmount = cdrUiShow;

	}
}
