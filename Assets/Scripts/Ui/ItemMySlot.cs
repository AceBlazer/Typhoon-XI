using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemMySlot : MonoBehaviour {

	public Items item;
	private GameEvents eventScript;
	private GameObject gameManager;
	private RectTransform rect;

	//The player will send the reference to this script 
	public ItemManagement itemManagementScript;
	public int slotNumber;
	public CanvasGroup stackCanvas;
	public Text stackText; 

	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform>();
		gameManager = GameObject.Find("GameManager_mn");
		eventScript = gameManager.GetComponent<GameEvents>();
	

	}

	public void UseThisItem ()
	{
		itemManagementScript.UseItem(item, slotNumber);
	}

	public void ShowToolTip ()
	{
		if (item.itemType != ItemDataClass.ItemType.Empty)
			eventScript.tooltipObj.transform.SetParent(transform.parent);
		eventScript.ShowToolTip(item.name, item.desc, item.cost, rect.transform.localPosition.x + 100, rect.transform.localPosition.y + 200);

	}

	public void HideToolTip ()
	{
		eventScript.HideToolTip();
	}

	// Update is called once per frame
	void Update () {
		if (item.itemSprite != null)
		{
			GetComponent<Image>().sprite = item.itemSprite;
		}

		if (item.stacks >1)
		{
			stackText.text = item.stacks.ToString();
			stackCanvas.alpha = 1;
		}
		else
		{
			stackCanvas.alpha = 0;
		}
	
	
	}
}
