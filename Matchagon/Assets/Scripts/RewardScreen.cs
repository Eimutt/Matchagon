using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RewardScreen : MonoBehaviour
{
    public List<Card> CardPrefabs;
    public List<Item> ItemPrefabs;
    public GameObject RewardBase;

    public GameObject confirmButton;

    private Reward SelectedReward;
    private int GoldReward;

    public int goldMax;
    public int goldMin;

    public 
    // Start is called before the first frame update
    void Start()
    {
        CardPrefabs = Resources.LoadAll<Card>("Prefab/Card").ToList();
        ItemPrefabs = Resources.LoadAll<Item>("Prefab/Items").ToList();

        confirmButton = transform.Find("Rewards/ConfirmReward").gameObject;
        transform.Find("Rewards").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GenerateCardRewards(0);
        }
    }

    public void GenerateCardRewards(int rarity)
    {
        Random.seed = System.DateTime.Now.Millisecond;
        int xpos = -170;
        for(int i = 0; i < 3; i++)
        {
            
            int r = UnityEngine.Random.Range(0, CardPrefabs.Count);
            var card = CardPrefabs[r];

            var cardObj = Instantiate(RewardBase, transform.Find("Rewards/Card Rewards"));
            cardObj.GetComponent<RectTransform>().localPosition = new Vector3(xpos, 0, 0); GetComponent<RectTransform>();
            //cardObj.transform.Find("Image").GetComponent<Image>().sprite = card.Sprite;
            //cardObj.transform.Find("Name").GetComponent<Text>().text = card.Name;
            //cardObj.transform.Find("Cost").GetComponent<Text>().text = card.Cost.ToString();
            //cardObj.transform.Find("Description").GetComponent<Text>().text = card.Description;

            DetailedCardInfo detailedCardInfo = cardObj.GetComponent<DetailedCardInfo>();
            detailedCardInfo.Populate(card.Sprite, card.Name, card.Cost.ToString(), card.SummonCost.ToString(), card.Description, card.Rarity, false);
            cardObj.name = card.Name;

            cardObj.AddComponent<Button>();

            var cardReward = cardObj.AddComponent<CardReward>();
            cardReward.card = card;
            xpos += 170;
        }
    }


    public void GenerateItemReward()
    {
        var normalItems = ItemPrefabs.Where(i => !i.Special).ToList();
        int r = UnityEngine.Random.Range(0, normalItems.Count);
        var item = normalItems[r];

        var itemObj = Instantiate(RewardBase, transform.Find("Rewards/Card Rewards"));
        itemObj.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0); GetComponent<RectTransform>();

        DetailedCardInfo detailedCardInfo = itemObj.GetComponent<DetailedCardInfo>();
        detailedCardInfo.Populate(item.Sprite, item.Name, "", "", item.Description, item.Rarity, false);
        itemObj.name = item.Name;

        itemObj.AddComponent<Button>();

        var itemReward = itemObj.AddComponent<ItemReward>();
        itemReward.Item = item;
    }

    public void GenerateSpawnPowerReward(int amount)
    {
        if(amount == 1)
        {
            var spawn1 = ItemPrefabs.Where(i => i.Name == "Flag1").FirstOrDefault();
            transform.Find("Rewards/Spawn Power").GetComponent<ItemReward>().Item = spawn1;

            transform.Find("Rewards/Spawn Power/Image").GetComponent<Image>().sprite = spawn1.Sprite;
        }
        else if (amount == 3)
        {
            var spawn3 = ItemPrefabs.Where(i => i.Name == "Flag3").FirstOrDefault();
            transform.Find("Rewards/Spawn Power").GetComponent<ItemReward>().Item = spawn3;
            transform.Find("Rewards/Spawn Power/Image").GetComponent<Image>().sprite = spawn3.Sprite;
        }
    }

    public void GenerateBattleRewards()
    {
        transform.Find("Rewards").gameObject.SetActive(true);
        GenerateCardRewards(0);
        GenerateGoldAmount();
        GenerateSpawnPowerReward(1);
    }

    public void GenerateTreasureRewards()
    {
        transform.Find("Rewards").gameObject.SetActive(true);
        GenerateItemReward();
        GenerateSpawnPowerReward(3);
    }

    public void GenerateGoldAmount()
    {
        GoldReward = Random.Range(goldMin, goldMax);

        transform.Find("Rewards/GoldReward").gameObject.SetActive(true);
        transform.Find("Rewards/GoldReward/Text").GetComponent<Text>().text = " + " + GoldReward.ToString();

    }

    public void Close()
    {
        foreach (Transform child in transform.Find("Rewards/Card Rewards"))
        {
            Destroy(child.gameObject);
        };

        transform.Find("Rewards/GoldReward").gameObject.SetActive(false);
        transform.Find("Rewards").gameObject.SetActive(false);
    }

    public void SelectReward(Reward reward)
    {
        SelectedReward = reward;
        UpdateSelectedButton();
    }

    public void PickReward()
    {
        GameObject.Find("GameHandler").GetComponent<PlayerData>().GetReward(SelectedReward);
        GameObject.Find("GameHandler").GetComponent<PlayerData>().GetGold(GoldReward);

        GameObject.Find("GameHandler").GetComponent<GameHandler>().CloseRewards();
        ClearButton();
    }

    private void UpdateSelectedButton()
    {
        confirmButton.GetComponent<Button>().interactable = true;
        confirmButton.transform.Find("Text").GetComponent<Text>().text = "Pick " + SelectedReward.name + " + " + GoldReward.ToString() + " gold"; 

    }

    private void ClearButton()
    {
        confirmButton.GetComponent<Button>().interactable = false;
        confirmButton.transform.Find("Text").GetComponent<Text>().text = "Select a card";
    }
}
