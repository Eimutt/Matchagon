using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopItem : ShopItem
{
    public Item Item;
    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(BuyItem);
    }

    public void BuyItem()
    {
        var sale = GameObject.Find("Shop").GetComponent<Shop>().AttemptPurchase(Cost, Item);
        if (sale)
        {
            Destroy(this.gameObject);
        }
    }
}
