using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public int MaxHealth;
    public int Health;
    public int Gold;
    public List<Card> DeckPrefabs;
    public List<Card> Deck;
    public List<Item> Items;

    public int Power;
    public int PowerLimit;
    public List<MinionCard> StartRoster;
    // Start is called before the first frame update
    void Start()
    {
        CreateCardCopies();
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
        DeckPrefabs.Add(card);
        CreateCardCopy(card);
    }

    public void GetItem(Item item)
    {
        Items.Add(item);

        item.Effects.Where(e => e.EffectType == EffectType.OnPickUp).ToList().ForEach(e => e.Trigger());
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

    public void RemoveCard(Card card)
    {
        Deck.Remove(card);
    }

    public void CreateCardCopies()
    {
        foreach (var card in DeckPrefabs)
        {
            var c = Instantiate(card);
            Deck.Add(c);
            c.gameObject.active = false;
            c.transform.position = new Vector3(10000, 0, 0);
            if (card.GetType() == typeof(MinionCard))
            {
                var m = (MinionCard)c;
                if (m.PowerCost + Power <= PowerLimit)
                {
                    StartRoster.Add(m);
                    Power += m.PowerCost;
                } 
            }
        }
    }

    public void CreateCardCopy(Card card)
    {
        var c = Instantiate(card);
        Deck.Add(c);
        c.gameObject.active = false;
        c.transform.position = new Vector3(10000, 0, 0);
    }

    public void ModifySpawnPower(int change)
    {
        PowerLimit += change;
    }
}
