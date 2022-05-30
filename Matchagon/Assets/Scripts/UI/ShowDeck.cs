using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDeck : MonoBehaviour
{
    public GameObject DeckContainer;
    public GameObject DetailedCardInfo;
    public bool active;

    public float xDif;
    public float yDif;
    public float xStart;
    public float yStart;
    public float rowMax;

    // Start is called before the first frame update
    void Start()
    {
        DeckContainer = GameObject.Find("DeckContainer");
        DeckContainer.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetKeyDown("escape"))
        {
            Close();
        }
    }

    public void Click()
    {
        if (active)
        {
            Close();
        } else
        {
            Show();
        }
    }

    public void Show()
    {
        DeckContainer.active = true;
        List<Card> cards = GameObject.Find("GameHandler").GetComponent<PlayerData>().Deck;


        int xCount = 0;
        int yCount = 0;
        foreach(var card in cards)
        {
            var cardObj = Instantiate(DetailedCardInfo, Vector3.zero, Quaternion.identity, DeckContainer.transform);

            cardObj.transform.localPosition = new Vector3(xStart + xCount * xDif, yStart + yCount * yDif);

            DetailedCardInfo detailedCardInfo = cardObj.GetComponent<DetailedCardInfo>();
            detailedCardInfo.Populate(card.Sprite, card.name, card.Cost.ToString(), card.Description, card.Rarity, false);

            xCount++;
            if(xCount >= rowMax)
            {
                yCount++;
                xCount = 0;
            }
        }
        active = true;
    }

    public void Close()
    {
        DeckContainer.active = false;
        DeckContainer.Clear();
        active = false;
    }


}
