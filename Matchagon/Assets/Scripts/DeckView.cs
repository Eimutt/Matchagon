using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckView : MonoBehaviour
{
    public GameObject DetailedCardInfo;
    public GameObject RosterSlot;

    public float xDif;
    public float yDif;
    public float xStart;
    public float yStart;
    public float rowMax;

    public List<Card> Cards;
    public List<MinionCard> StartMinions;

    public int Power;
    public int PowerLimit;

    public PlayerData PlayerData;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData = GameObject.Find("GameHandler").GetComponent<PlayerData>();
        StartMinions = PlayerData.StartRoster.ToList();
        Cards = PlayerData.Deck.ToList();
        Power = StartMinions.Sum(m => m.PowerCost);

        PowerLimit = PlayerData.PowerLimit;
        UpdatePowerLimit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartMinions = PlayerData.StartRoster.ToList();
        Power = StartMinions.Sum(m => m.PowerCost);
        PowerLimit = PlayerData.PowerLimit;
        UpdatePowerLimit();
        Cards = PlayerData.Deck.ToList();
        LoadDeck();
        //LoadStartRoster();
    }

    public void LoadDeck()
    {
        int xCount = 0;
        int yCount = 0;
        foreach (var card in Cards)
        {
            var cardObj = Instantiate(DetailedCardInfo, Vector3.zero, Quaternion.identity, transform.Find("Cards"));

            cardObj.transform.localPosition = new Vector3(xStart + xCount * xDif, yStart + yCount * yDif);

            DetailedCardInfo detailedCardInfo = cardObj.GetComponent<DetailedCardInfo>();
            detailedCardInfo.Populate(card.Sprite, card.name, card.Cost.ToString(), card.Description, card.Rarity, false);


            if (card.GetType() == typeof(MinionCard))
            {
                var minionCard = (MinionCard)card;

                if (StartMinions.Contains(minionCard))
                {
                    var i = StartMinions.IndexOf(minionCard);
                    var rosterSlotObj = Instantiate(RosterSlot, transform.Find("StartTeam/Pos" + i), false);
                    i++;

                    rosterSlotObj.GetComponent<Image>().sprite = minionCard.Sprite;

                    var rosterSlot = rosterSlotObj.AddComponent<RosterSlot>();
                    rosterSlot.Minion = minionCard;
                }


                var rosterSlot2 = cardObj.AddComponent<RosterSlot>();
                rosterSlot2.Minion = minionCard;
            }
                






            xCount++;
            if (xCount >= rowMax)
            {
                yCount++;
                xCount = 0;
            }



        }
    }

    public void Close()
    {
        Clear();
        gameObject.SetActive(false);
    }

    public void SetStartRoster()
    {
        PlayerData.StartRoster = StartMinions;
    }

    public void Clear()
    {
        transform.Find("Cards").Clear();
        transform.Find("StartTeam").ClearChildren();
    }

    public void UpdateView()
    {
        Clear();
        LoadDeck();
    }


    public void ToggleMinion(MinionCard card)
    {
        if (StartMinions.Contains(card))
        {
            StartMinions.Remove(card);
            Power -= card.PowerCost;
            //
        } else
        {
            if(StartMinions.Count < 5 && card.PowerCost + Power <= PowerLimit)
            {
                StartMinions.Add(card);
                Power += card.PowerCost;
            }
        }
        UpdatePowerLimit();
        UpdateView();
    }

    public void UpdatePowerLimit()
    {
        GameObject.Find("SpawnPowerText").GetComponent<Text>().text = Power.ToString() + "/" + PowerLimit.ToString();
    }
}
