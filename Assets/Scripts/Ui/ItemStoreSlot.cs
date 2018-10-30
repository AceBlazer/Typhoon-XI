using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemStoreSlot : MonoBehaviour {

	public Items item;
	private GameEvents eventScript;

	private GameObject gameManager;
	private RectTransform rect;

	public Text priceText;
	//The player will send the reference to this script 
	public ItemManagement itemManagementScript;

	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform>();
		gameManager = GameObject.Find("GameManager_mn");
		eventScript = gameManager.GetComponent<GameEvents>();
	


	}

	public void BuyThisItem ()
	{
		itemManagementScript.BuyItem(item);
		Debug.Log("BuyItem");
	}

	public void ShowToolTip ()
	{
		eventScript.tooltipObj.transform.SetParent(transform.parent);
		eventScript.ShowToolTip(item.name, item.desc, item.cost, rect.transform.localPosition.x, rect.transform.localPosition.y);
	}

	public void HideToolTip ()
	{
		eventScript.HideToolTip();
	}

	// Update is called once per frame
	void Update () {
		priceText.text = item.cost.ToString();

	
	}
}
