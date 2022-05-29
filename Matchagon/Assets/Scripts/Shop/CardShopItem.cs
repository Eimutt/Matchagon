using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardShopItem : ShopItem
{
    public Card Card;
    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(BuyCard);
    }

    public void BuyCard()
    {
        var sale = GameObject.Find("Shop").GetComponent<Shop>().AttemptPurchase(Cost, Card);
        if (sale)
        {
            Destroy(this.gameObject);
        }
    }
}
