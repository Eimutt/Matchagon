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

    public float HealingMultiplier;

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
        GameObject.Find("Gold").GetComponent<Text>().text = Gold.ToString();
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
            AddItem(item.Item);
        }
    }

    public void GetCard(Card card)
    {
        DeckPrefabs.Add(card);
        CreateCardCopy(card);
    }

    public void AddItem(Item item)
    {
        Items.Add(item);

        item.Effects.Where(e => e.EffectType == EffectType.OnPickUp).ToList().ForEach(e => e.Trigger());
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);

        item.Effects.Where(e => e.EffectType == EffectType.OnRemoval).ToList().ForEach(e => e.Trigger());
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
        Health = newHealth > MaxHealth ? MaxHealth : newHealth;
        GameObject.Find("Health").GetComponent<Text>().text = Health + "/" + MaxHealth;
    }

    public void Heal(int amount)
    {
        int health = Health + (int)((float)amount * HealingMultiplier);
        UpdateHealth(health);
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
