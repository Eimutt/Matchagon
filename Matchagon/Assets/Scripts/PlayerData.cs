using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public int MaxHealth;
    public int Health;
    public int Gold;
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

    public void GetReward(Reward reward)
    {
        if (reward.GetType() == typeof(CardReward))
        {
            var card = (CardReward)reward;
            GetCard(card.card);
        } else if (reward.GetType() == typeof(ItemReward)){
            var item = (ItemReward)reward;
            GetItem(item.Item);
        }
    }

    public void GetCard(Card card)
    {
        Deck.Add(card);
    }

    public void GetItem(Item item)
    {
        Items.Add(item);
    }

    public void GetGold(int gold)
    {
        Gold += gold;
        GameObject.Find("Gold").GetComponent<Text>().text = Gold.ToString();
        //Play animation
    }

    public void LoseGold(int gold)
    {
        Gold -= gold;
        GameObject.Find("Gold").GetComponent<Text>().text = Gold.ToString();
        //Play animation

    }

    public void UpdateHealth(int newHealth)
    {
        Health = newHealth;
        GameObject.Find("Health").GetComponent<Text>().text = newHealth + "/" + MaxHealth;
    }
}
