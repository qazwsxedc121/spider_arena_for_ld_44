using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

    public int price = 0;
    public int stock = 0;
    public int index = 0;
    private GameObject buyBtn;
    private GameObject coverMask;
    private GameObject priceText;
    private GameObject stockText;
    private GameObject upgradePanel;

    private void Awake() {
        buyBtn = transform.Find("buyBtn").gameObject;
        coverMask = transform.Find("coverMask").gameObject;
        priceText = transform.Find("priceText").gameObject;
        stockText = transform.Find("stockText").gameObject;
        upgradePanel = transform.parent.gameObject;
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Buy() {
        upgradePanel.GetComponent<UpgradeCtrl>().Buy(index);
    }

    public void UpdateState() {
        if (!gameObject.activeInHierarchy) {
            return;
        }
        priceText.GetComponent<Text>().text = "$" + price.ToString();
        if(stock == -1) {
            stockText.GetComponent<Text>().text = "infinite";
        } else {
            stockText.GetComponent<Text>().text = stock.ToString();
        }
        if (stock == 0) {
            buyBtn.SetActive(false);
            coverMask.SetActive(true);
        } else {
            buyBtn.SetActive(true);
            coverMask.SetActive(false);
        }
    }
}
