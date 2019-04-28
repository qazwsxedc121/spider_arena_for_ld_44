using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCtrl : MonoBehaviour {

    public GameObject player;
    public GameObject[] items;
    private int[] priceList = new int[] {1, 2, 2, 3, 2, 5};
    private int[] initStockList = new int[] {-1, 2, 2, 1, 1, 1};
	// Use this for initialization
	void Start () {
		for(int i = 0; i < 6; i++) {
            ItemUI comp = items[i].GetComponent<ItemUI>();
            comp.index = i;
            comp.price = priceList[i];
            comp.stock = initStockList[i];
            comp.UpdateState();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Buy(int index) {
        player.GetComponent<PlayerCtrl>().Buy(index);
    }
    public void UpdateInfo(int money, int[] stockList) {
        transform.Find("LifeText").gameObject.GetComponent<Text>().text = money.ToString();
		for(int i = 0; i < 6; i++) {
            ItemUI comp = items[i].GetComponent<ItemUI>();
            comp.stock = stockList[i];
            comp.UpdateState();
        }
    }
}
