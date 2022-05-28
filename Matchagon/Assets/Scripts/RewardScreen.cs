using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardScreen : MonoBehaviour
{
    public Card[] CardPrefabs;
    public GameObject CardBase;

    public GameObject confirmButton;

    private Card SelectedCard;
    private int GoldReward;

    public int goldMax;
    public int goldMin;

    // Start is called before the first frame update
    void Start()
    {
        CardPrefabs = Resources.LoadAll<Card>("Prefab/Card");

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
            
            int r = UnityEngine.Random.Range(0, CardPrefabs.Length);
            var card = CardPrefabs[r];

            var cardObj = Instantiate(CardBase, transform.Find("Rewards/Card Rewards"));
            cardObj.GetComponent<RectTransform>().localPosition = new Vector3(xpos, 0, 0); GetComponent<RectTransform>();
            cardObj.transform.Find("Image").GetComponent<Image>().sprite = card.Sprite;
            cardObj.transform.Find("Cost").GetComponent<Text>().text = card.Cost.ToString();
            cardObj.transform.Find("Description").GetComponent<Text>().text = card.Description;
            cardObj.GetComponent<CardReward>().card = card;
            xpos += 170;
        }

    }

    public void GenerateRewards()
    {
        transform.Find("Rewards").gameObject.SetActive(true);
        GenerateCardRewards(0);
        GenerateGoldAmount();
    }

    public void GenerateGoldAmount()
    {
        GoldReward = Random.Range(goldMin, goldMax);
        transform.Find("Rewards/GoldReward/Text").GetComponent<Text>().text = " + " + GoldReward.ToString();

    }

    public void Close()
    {
        foreach (Transform child in transform.Find("Rewards/Card Rewards"))
        {
            Destroy(child.gameObject);
        };
        
        transform.Find("Rewards").gameObject.SetActive(false);
    }

    public void SelectCard(Card card)
    {
        SelectedCard = card;
        UpdateSelectedButton();
    }

    

    public void PickReward()
    {
        GameObject.Find("GameHandler").GetComponent<PlayerData>().GetCard(SelectedCard);
        GameObject.Find("GameHandler").GetComponent<PlayerData>().GetGold(GoldReward);

        GameObject.Find("GameHandler").GetComponent<GameHandler>().CloseRewards();
        ClearButton();
    }

    private void UpdateSelectedButton()
    {
        confirmButton.GetComponent<Button>().interactable = true;
        confirmButton.transform.Find("Text").GetComponent<Text>().text = "Pick " + SelectedCard.name + " + " + GoldReward.ToString() + " gold"; 

    }

    private void ClearButton()
    {
        confirmButton.GetComponent<Button>().interactable = false;
        confirmButton.transform.Find("Text").GetComponent<Text>().text = "Select a card";

    }
}
