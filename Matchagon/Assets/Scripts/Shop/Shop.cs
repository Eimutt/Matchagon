using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public PlayerData PlayerData;

    public Card[] CardPrefabs;
    public Item[] ItemPrefabs;
    public GameObject ShopItemBase;




    public Sprite CantAfford;
    public Sprite Sale;

    public int CardMinCost;
    public int CardMaxCost;
    public int ItemMinCost;
    public int ItemMaxCost;
    public int CardRarityCostIncrease;
    public int ItemRarityCostIncrease;

    public int startX, startY;
    public int Dif;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData = GameObject.Find("GameHandler").GetComponent<PlayerData>();
        CardPrefabs = Resources.LoadAll<Card>("Prefab/Card");
        ItemPrefabs = Resources.LoadAll<Item>("Prefab/Items");

        GenerateCards();
        GenerateItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateItems()
    {
        Random.seed = System.DateTime.Now.Millisecond;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                var normalItems = ItemPrefabs.Where(it => !it.Special).ToList();
                int r = Random.Range(0, normalItems.Count);
                var item = normalItems[r];

                var itemObj = Instantiate(ShopItemBase, transform.Find("Items"));
                itemObj.GetComponent<RectTransform>().localPosition = new Vector3(startX + i * Dif, startY + j * Dif, 0); GetComponent<RectTransform>();
                itemObj.transform.Find("Image").GetComponent<Image>().sprite = item.Sprite;
                itemObj.transform.Find("Name").GetComponent<Text>().text = item.Name;
                itemObj.transform.Find("Description").GetComponent<Text>().text = item.Description;

                var itemshopItem = itemObj.AddComponent<ItemShopItem>();
                itemshopItem.Item = item;
                itemshopItem.Cost = Random.Range(ItemMinCost, ItemMaxCost) + item.Rarity * ItemRarityCostIncrease;
                itemObj.transform.Find("GoldCost/Text").GetComponent<Text>().text = itemshopItem.Cost.ToString();
            }
        }

    }

    private void GenerateCards()
    {
        Random.seed = System.DateTime.Now.Millisecond;

        for(int i = 0; i < 2; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                int r = Random.Range(0, CardPrefabs.Length);
                var card = CardPrefabs[r];

                var cardObj = Instantiate(ShopItemBase, transform.Find("Cards"));
                cardObj.GetComponent<RectTransform>().localPosition = new Vector3(startX + i * Dif, startY + j * Dif, 0); GetComponent<RectTransform>();
                cardObj.transform.Find("Image").GetComponent<Image>().sprite = card.Sprite;
                cardObj.transform.Find("Cost").GetComponent<Text>().text = card.Cost.ToString();
                cardObj.transform.Find("Name").GetComponent<Text>().text = card.Name;
                cardObj.transform.Find("Description").GetComponent<Text>().text = card.Description;

                var cardshopItem = cardObj.AddComponent<CardShopItem>();
                cardshopItem.Card = card;
                cardshopItem.Cost = Random.Range(CardMinCost, CardMaxCost) + card.Rarity * CardRarityCostIncrease;
                cardObj.transform.Find("GoldCost/Text").GetComponent<Text>().text = cardshopItem.Cost.ToString();
            }
        }
    }

    public bool AttemptPurchase(int cost, Card card)
    {
        if (CheckIfAfford(cost))
        {
            PlayerData.LoseGold(cost);
            PlayerData.GetCard(card);

            return true;
        }

        return false;
    }

    public bool AttemptPurchase(int cost, Item item)
    {
        if (CheckIfAfford(cost))
        {
            PlayerData.LoseGold(cost);
            PlayerData.GetItem(item);

            return true;
        }

        return false;
    }

    public bool CheckIfAfford(int cost)
    {
        if (cost < PlayerData.Gold)
        {
            //buy
            GameObject.Find("SpeechBubble").GetComponent<SpeechBubble>().SetSprite(Sale);
            return true;
        }
        else
        {
            //you cant afford that picture
            GameObject.Find("SpeechBubble").GetComponent<SpeechBubble>().SetSprite(CantAfford);
            return false;
        }
    }

    public void LeaveShop()
    {
        GameObject.Find("GameHandler").GetComponent<GameHandler>().LeaveShop();
    }
}
