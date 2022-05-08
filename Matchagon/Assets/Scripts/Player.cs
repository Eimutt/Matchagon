using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Board Board;

    public int Shield;
    public int MaxHp;
    public int CurrentHp;

    private int mana = 0;
    private int maxMana = 0;
    private int tmpMana;

    public float ColorTintDuration;
    //public Color ShieldColor;
    public Color DamagedColor;
    [SerializeField]
    private InvulnerabilityColor InvulnerabilityColor;

    public List<Minion> Minions;
    private List<Vector3> positions;
    public Vector3 bottomLeft;
    public float distanceDiff;

    private static System.Random rng = new System.Random();

    public List<Card> Deck;
    public List<Card> Hand;

    public float[] DamageMultiplier = new float[] { 1, 1, 1, 1, 1, 1 };

    public GameObject sliderPrefab;
    public GameObject spawnPointPrefab;
    private List<GameObject> spawnPoints;

    public Card selectedCard;
    // Start is called before the first frame update
    void Start()
    {
        Board = GameObject.Find("Board").GetComponent<Board>();

        CurrentHp = GameObject.Find("GameHandler").GetComponent<PlayerData>().Health;
        Deck = GameObject.Find("GameHandler").GetComponent<PlayerData>().Deck;

        //CurrentHp = MaxHp;

        UpdateUIHealth();
        UpdateUIShield();

        Minions.Add(GetComponent<Minion>());
        GetComponent<Minion>().position = 0;

        InvulnerabilityColor = GetComponent<InvulnerabilityColor>();

        positions = new List<Vector3>();
        spawnPoints = new List<GameObject>();
        GetLegitimateSpawnPositions();

        SpawnDeck();
        Deck = Deck.OrderBy(a => rng.Next()).ToList();
        DrawCard();
        DrawCard();

}

// Update is called once per frame
void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        if(Shield > 0)
        {
            int tmp = damage;
            damage -= Shield;
            Shield -= tmp;
        }
        var healthDamage = Mathf.Max(0, damage);

        if(healthDamage > 0)
        {
            CurrentHp -= healthDamage;
            InvulnerabilityColor.SetTintColor(DamagedColor, ColorTintDuration);
            GameObject.Find("CombatHandler").GetComponent<DamageTextHandler>().SpawnDamageText(transform.position, Color.red, healthDamage, 1);
        }

        UpdateUIHealth();
        UpdateUIShield();

    }

    //public void GetShield(int shield)
    //{
    //    InvulnerabilityColor.SetTintColor(ShieldColor, ColorTintDuration);
    //    Shield += shield;
    //    UpdateUIShield();
    //}
    private void GetLegitimateSpawnPositions()
    {
        for (int i = 0; i < 5; i++)
        {
            var pos = bottomLeft + new Vector3(1 + i * distanceDiff, i % 2 == 0 ? -1 * distanceDiff : 1 * distanceDiff, 0);
            positions.Add(pos);

            var spawnPoint = Instantiate(spawnPointPrefab, pos, Quaternion.identity, transform);
            spawnPoints.Add(spawnPoint);
            spawnPoint.name = "Spawn" + i;

            spawnPoint.GetComponent<SpawnPoint>().position = i + 1;

            spawnPoint.SetActive(false);
        }
    }

    public void ResetShield()
    {
        Shield = 0;
        UpdateUIShield();
    }

    public void NewTurn()
    {
        ResetShield();
        DrawCard();
        IncreaseMana();
        mana = maxMana + tmpMana;
        tmpMana = 0;
        UpdatePlayableCards();

        Minions.ForEach(m => m.TriggerStartOfTurnEffects());
    }

    public void UpdatePlayableCards()
    {
        int i = 0;
        foreach(Card card in Hand)
        {
            card.gameObject.transform.Find("Cross").gameObject.SetActive(card.Cost > mana);

            card.transform.localPosition = new Vector3(i, 0);
            i++;
        }
        GameObject.Find("ManaText").GetComponent<Text>().text = mana.ToString() + "/" + maxMana.ToString();
    }

    public void UpdateUIHealth()
    {

        transform.Find("Canvas/Slider").GetComponent<Slider>().value = (float)CurrentHp / (float)MaxHp;
        GameObject.Find("HpText").GetComponent<Text>().text = CurrentHp.ToString() + "/" + MaxHp.ToString();
    }

    public void UpdateUIShield()
    {
        transform.Find("Canvas/Slider/Shield").GetComponent<Image>().enabled = Shield > 0;
        GameObject.Find("ShieldText").GetComponent<Text>().text = Shield > 0 ? "+(" + Shield.ToString() + ")" : "";
    }

    public void ResolveTurn(Combo combo, EnemyHandler enemyHandler)
    {
        List<PossibleAttack> attacks = new List<PossibleAttack>();


        foreach (KeyValuePair<TypeEnum, int> entry in combo.damageDict)
        {
            int value = entry.Value;

            if (entry.Key == TypeEnum.Shield)
            {
                foreach (Minion minion in Minions)
                {
                    Shield += (minion.GetShield() * combo.count);
                    UpdateUIShield();
                }
            }
            else
            {
                if (entry.Key == TypeEnum.Light)
                {
                    mana = mana < maxMana ? mana + 1 : mana;
                }
                if (enemyHandler.NoEnemiesLeft())
                {
                    continue;
                }

                foreach (Minion minion in Minions)
                {
                    float minionElementDamage = (float)minion.Damages[(int)entry.Key] * DamageMultiplier[(int)entry.Key];
                    if (minionElementDamage != 0)
                    {
                        attacks.Add(new PossibleAttack(minion, MatchEnum.Blob, entry.Key, value * minionElementDamage, combo.count, combo.damageMultiplier, minion.AOE));
                    }
                }
            }
        }


        //foreach() LAUNCH AOES
        var aoeAttack = attacks.Where(a => a.Area).ToList();

        int i = 0;
        int max = aoeAttack.Count;

        while (i < max && !enemyHandler.NoEnemiesLeft() && aoeAttack.Any())
        {

            var possibleAttack = aoeAttack.First();

            foreach (Enemy enemy in enemyHandler.GetAllEnemies())
            {
                enemy.AddIncomingDamage(possibleAttack.fullDamage);
            }
            attacks.Remove(possibleAttack);
            possibleAttack.source.GetComponent<CombatAnimations>().QueueAttack(MatchEnum.AOE, possibleAttack.type, possibleAttack.baseDamage, possibleAttack.fullDamage, combo.count, enemyHandler.GetAllEnemies().Select(e => e.gameObject).ToList());

            i++;
        }
        


        //Loop through attacks and enemies
        attacks.Sort((x, y) => x.fullDamage.CompareTo(y.fullDamage));

        i = 0;
        max = attacks.Count;

        while (i < max && !enemyHandler.NoEnemiesLeft())
        {
            var enemy = enemyHandler.GetFirstEnemy();

            var possibleAttack = attacks.FirstOrDefault(a => a.fullDamage > enemy.GetDamageLeftAfterIncoming());

            if (possibleAttack == null)
            {
                possibleAttack = attacks.Last();
            }

            enemy.AddIncomingDamage(possibleAttack.fullDamage);
            attacks.Remove(possibleAttack);
            possibleAttack.source.GetComponent<CombatAnimations>().QueueAttack(MatchEnum.Blob, possibleAttack.type, possibleAttack.baseDamage, possibleAttack.fullDamage, combo.count, enemy.gameObject);

            i++;
        }
    }

    public void TryPlayCard(int cardIndex)
    {
        var card = Hand[cardIndex];
        if(card.GetType() == typeof(MinionCard))
        {
            SelectMinionCard(cardIndex);
        } else
        {
            PlayCard(cardIndex);
        }

    }

    public void SelectMinionCard(int cardIndex)
    {
        var card = Hand[cardIndex];
        if (card.Cost <= mana)
        {
            card.gameObject.transform.position += new Vector3(0, 0.5f, 0);
            ShowViableSpawnPoints();

            selectedCard = card;
        }
    }

    public void ShowViableSpawnPoints()
    {
        for (int i = 0; i < spawnPoints.Count ; i++)
        {



            if (!Minions.Any(e => e.position - 1 == i))
            {
                spawnPoints[i].gameObject.SetActive(true);
            }
            else
            {
                spawnPoints[i].gameObject.SetActive(false);
            }
        }
    }
    
    public void HideSpawnPoints() {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            spawnPoints[i].gameObject.SetActive(false);
        }
    }


    public void AddMinion(GameObject minionGameObject)
    {
        bool positionFound = false;

        Vector3 position = Vector3.zero;

        int i = 0;
        while (!positionFound)
        {
            i++;
            position = positions[i - 1];
            if (!Minions.Any(e => e.position == i))
            {
                positionFound = true;
            }

            if (i > positions.Count + 1)
            {
                Debug.Log("no room for enemy");
                return;
            }
        }


        GameObject minionObject = Instantiate(minionGameObject, position, Quaternion.identity);

        GameObject hpbar = Instantiate(sliderPrefab, minionObject.transform);

        Minion minion = minionObject.GetComponent<Minion>();
        minion.position = i;

        Minions.Add(minion);
    }

    public void AddMinion(int positionIndex)
    {
        var position = positions[positionIndex - 1];

        var x = (MinionCard)selectedCard;

        GameObject minionObject = Instantiate(x.MinionPrefab, position, Quaternion.identity);

        GameObject hpbar = Instantiate(sliderPrefab, minionObject.transform);

        Minion minion = minionObject.GetComponent<Minion>();
        minion.position = positionIndex;

        Minions.Add(minion);

        HideSpawnPoints();

        mana -= selectedCard.Cost;
        Destroy(selectedCard.gameObject);
        Hand.Remove(selectedCard);
        UpdatePlayableCards();

        selectedCard = null;
    }

    public void PlayCard(int cardIndex)
    {
        var card = Hand[cardIndex];
        if(card.Cost <= mana)
        {
            card.Play();
            mana -= card.Cost;
            Destroy(card.gameObject);
            Hand.Remove(card);
            UpdatePlayableCards();

        }
    }

    public void PlayCard(Card card)
    {
        if (card.Cost <= mana)
        {
            card.Play();
            mana -= card.Cost;
            Destroy(card.gameObject);
            Hand.Remove(card);
            UpdatePlayableCards();

        }
    }

    public void SpawnDeck()
    {
        //var deck = GameObject.Find("Deck").transform;
        //foreach (Card card in Deck)
        //{
        //    GameObject cardObj = Instantiate(card.gameObject, Vector3.zero, Quaternion.identity, deck);
        //}
    }

    public void DrawCard()
    {
        if(Deck.Count == 0)
        {
            return;
        }
        var card = Deck[0];

        Deck.Remove(card);

        
        if(Hand.Count < 9)
        {
            var hand = GameObject.Find("Hand");

            GameObject cardObj = Instantiate(card.gameObject, hand.transform.position + new Vector3(Hand.Count, 0), Quaternion.identity, hand.transform);

            cardObj.transform.parent = hand.transform;

            Hand.Add(cardObj.GetComponent<Card>());
        }
    }

    public void ChangeDamage(TypeEnum type, int percentage)
    {
        DamageMultiplier[(int)type] *= 1 + ((float)percentage / 100f);
    }

    public void IncreaseMana()
    {
        maxMana = maxMana == 5 ? 5: maxMana + 1;
    }

    public void GrantTemporaryMana(int amount)
    {
        tmpMana += amount;
    }

    public void AttackFirstMinion(int damage)
    {
        var target = Minions.OrderByDescending(m => m.position).First();

        if(target.position == 0)
        {
            TakeDamage(damage);
        } else
        {
            target.TakeDamage(damage);
            if (target.Dead)
            {
                Minions.Remove(target);
                Destroy(target.gameObject);
            }
        }

    }
}
