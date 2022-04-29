using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int Health;
    public List<Card> Deck;
    public List<Item> Items;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetCard(Card card)
    {
        Deck.Add(card);
    }

    public void GetItem(Item item)
    {
        Items.Add(item);
    }
}
