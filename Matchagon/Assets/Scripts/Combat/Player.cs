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
    public Color ShieldColor;
    public Color DamagedColor;
    [SerializeField]
    private InvulnerabilityColor InvulnerabilityColor;

    public List<Minion> Minions;
    private List<Vector3> positions;
    public Vector3 bottomLeft;
    public float distanceDiff;

    private static System.Random rng = new System.Random();

    public List<Item> Items;
    public List<Card> Deck;
    public List<Card> Hand;
    public List<MinionCard> StartMinions;

    public float[] TypeDamageMultipliers = new float[] { 100, 100, 100, 100, 100, 100 };
    public float GlobalDamageMultiplier;

    public GameObject sliderPrefab;
    public GameObject spawnPointPrefab;
    private List<GameObject> spawnPoints;

    public Card selectedCard;
    // Start is called before the first frame update
    void Start()
    {
        Board = GameObject.Find("Board").GetComponent<Board>();

        CurrentHp = GameObject.Find("GameHandler").GetComponent<PlayerData>().Health;

        GetComponent<Minion>().CurrentHp = CurrentHp;
        GetComponent<Minion>().CurrentHp = CurrentHp;

        Deck = GameObject.Find("GameHandler").GetComponent<PlayerData>().Deck;
        Items = GameObject.Find("GameHandler").GetComponent<PlayerData>().Items;

        StartMinions = GameObject.Find("GameHandler").GetComponent<PlayerData>().StartRoster;

        //CurrentHp = MaxHp;

        UpdateUIHealth();
        UpdateUIShield();

        Minions.Add(GetComponent<Minion>());
        GetComponent<Minion>().position = 0;

        InvulnerabilityColor = GetComponent<InvulnerabilityColor>();

        positions = new List<Vector3>();
        spawnPoints = new List<GameObject>();
        GetLegitimateSpawnPositions();


        Deck = Deck.OrderBy(a => rng.Next()).ToList();
        Deck.ForEach(d => d.gameObject.SetActive(true));

    }

// Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        //if(Shield > 0)
        //{
        //    int tmp = damage;
        //    damage -= Shield;
        //    Shield -= tmp;
        //}
        //var healthDamage = Mathf.Max(0, damage);

        //if(healthDamage > 0)
        //{
        //    CurrentHp -= healthDamage;
        //    InvulnerabilityColor.SetTintColor(DamagedColor, ColorTintDuration);
        //    GameObject.Find("CombatHandler").GetComponent<DamageTextHandler>().SpawnDamageText(transform.position, Color.red, healthDamage, 1);
        //}
        CurrentHp -= damage;
        InvulnerabilityColor.SetTintColor(DamagedColor, ColorTintDuration);
        GameObject.Find("CombatHandler").GetComponent<DamageTextHandler>().SpawnDamageText(transform.position, Color.red, damage, 1);
        //UpdateUIHealth();
        //UpdateUIShield();
        if(CurrentHp <= 0)
        {
            Lose();
        }
    }

    private void Lose()
    {
        GameObject.Find("GameHandler").GetComponent<GameHandler>().Lose();
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
        DrawCard();
        IncreaseMana();
        mana = maxMana + tmpMana;
        tmpMana = 0;
        UpdatePlayableCards();

        TriggerStartOfTurnItemEffects();
        Minions.ForEach(m => m.TriggerStartOfTurnEffects());
    }

    public void EndOfTurn()
    {
        TriggerEndOfTurnItemEffects();
        ResetShield();
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

    public void GetShield(int shield)
    {
        Shield += shield;
        UpdateUIShield();
    }

    public void ResolveTurn(Combo combo, EnemyHandler enemyHandler)
    {

        TriggerComboEffects(combo.count);

        List<PossibleAttack> attacks = new List<PossibleAttack>();


        foreach (KeyValuePair<TypeEnum, int> entry in combo.damageDict)
        {
            if ((int)entry.Key > 6)
                continue;

            int value = entry.Value;

            if (entry.Key == TypeEnum.Shield)
            {
                foreach (Minion minion in Minions)
                {
                    Shield += (minion.GetShield() * combo.count);
                    UpdateUIShield();

                    if (minion.AttackOnShield)
                    {
                        if (minion.Damages[(int)entry.Key] != 0)
                        {
                            var r = Random.Range(0.8f, 1.2f);

                            float minionElementDamage = r * (float)minion.Damages[(int)entry.Key] * (float)(TypeDamageMultipliers[(int)entry.Key] * (GlobalDamageMultiplier) / 10000);

                            attacks.Add(new PossibleAttack(minion, MatchEnum.Blob, entry.Key, value * minionElementDamage, combo.count, combo.damageMultiplier, minion.AOE));

                        }
                    }
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
                    if(minion.Damages[(int)entry.Key] != 0)
                    {
                        var r = Random.Range(0.8f, 1.2f); 
                        
                        float minionElementDamage = r * (float)minion.Damages[(int)entry.Key] * (float)(TypeDamageMultipliers[(int)entry.Key] * (GlobalDamageMultiplier) / 10000);

                        //Debug.Log("damage = random(" + r + ") * basedamage(" + minion.Damages[(int)entry.Key] + ") * amount(" + value + ") * damageMultipliers(" + (float)(TypeDamageMultipliers[(int)entry.Key] * (GlobalDamageMultiplier) / 10000) + ") * comboMultuplier(" + combo.damageMultiplier + ") = " + value * minionElementDamage * combo.damageMultiplier);



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
            aoeAttack.Remove(possibleAttack);
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
            //Debug.Log(possibleAttack.type + " for " + possibleAttack.fullDamage + " damage");
            i++;
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
                spawnPoints[i].gameObject.SetActive(true);
                spawnPoints[i].transform.Find("Location").GetComponent<SpriteRenderer>().color = Color.red;
                spawnPoints[i].transform.Find("Location").GetComponent<Pulse>().Occupied = true;
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

        var occupied = Minions.FirstOrDefault(m => m.position == positionIndex);
        if (occupied)
        {
            KillMinion(occupied);
        }

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

    public void TryPlayCard(int cardIndex)
    {
        var card = Hand[cardIndex];
        if (card.GetType() == typeof(MinionCard))
        {
            SelectMinionCard(cardIndex);
        }
        else
        {
            PlayCard(cardIndex);
        }

    }

    public void TryPlayCard(Card card)
    {
        if (card.GetType() == typeof(MinionCard))
        {
            SelectMinionCard(card);
        }
        else
        {
            PlayCard(card);
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

    public void SelectMinionCard(Card card)
    {
        if (card.Cost <= mana)
        {
            card.gameObject.transform.position += new Vector3(0, 0.5f, 0);
            ShowViableSpawnPoints();

            selectedCard = card;
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
    public void PlayCard(int cardIndex)
    {
        var card = Hand[cardIndex];
        if (card.Cost <= mana)
        {
            card.Play();
            mana -= card.Cost;
            Destroy(card.gameObject);
            Hand.Remove(card);
            UpdatePlayableCards();

        }
    }

    public void DeselectMinionCard()
    {
        selectedCard.gameObject.transform.position -= new Vector3(0, 0.5f, 0);
        HideSpawnPoints();

        selectedCard = null;

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

    public void ChangeTypeDamagePercentage(TypeEnum type, int percentage)
    {
        TypeDamageMultipliers[(int)type] *= 1 + ((float)percentage / 100f);
    }
    public void ChangeTypeDamageFlat(TypeEnum type, int flatPercentage)
    {
        TypeDamageMultipliers[(int)type] += (float)flatPercentage;
    }

    public void ChangeGlobalDamageFlat(int flatPercentage)
    {
        GlobalDamageMultiplier += (float)flatPercentage;
    }

    public void ChangeGlobalDamagePercentage(int flatPercentage)
    {
        GlobalDamageMultiplier += (float)flatPercentage;
    }


    public void IncreaseMana()
    {
        maxMana = maxMana == 5 ? 5: maxMana + 1;
    }

    public void GrantTemporaryMana(int amount)
    {
        tmpMana += amount;
    }
    public void AttackAllMinions(int damage)
    {
        int tmp = damage;
        if (Shield > 0)
        {
            damage -= Shield;
            Shield -= tmp;
        }
        var healthDamage = Mathf.Max(0, damage);

        if (healthDamage > 0)
        {
            TakeDamage(damage);

            var summonedMinions = Minions.Where(m => m.position != 0).ToList();
            foreach (Minion m in summonedMinions)
            {
                m.TakeDamage(damage);
                if (m.Dead)
                {
                    KillMinion(m);
                }
            }
            //InvulnerabilityColor.SetTintColor(DamagedColor, ColorTintDuration);
            //GameObject.Find("CombatHandler").GetComponent<DamageTextHandler>().SpawnDamageText(transform.position, Color.red, healthDamage, 1);
        }
        else
        {
            InvulnerabilityColor.SetTintColor(ShieldColor, ColorTintDuration);
            GameObject.Find("CombatHandler").GetComponent<DamageTextHandler>().SpawnDamageText(transform.position, ShieldColor, tmp, 1);
        }

        UpdateUIHealth();
        UpdateUIShield();
    }
    public void AttackFirstMinion(int damage)
    {
        var target = Minions.OrderBy(m => m.position).Last();

        int tmp = damage;
        if (Shield > 0)
        {
            damage -= Shield;
            Shield -= tmp;
        }
        var healthDamage = Mathf.Max(0, damage);

        if (healthDamage > 0)
        {
            TakeDamage(damage);
            target.TakeDamage(damage);
            if (target.Dead)
            {
                KillMinion(target);
            }
            //InvulnerabilityColor.SetTintColor(DamagedColor, ColorTintDuration);
            //GameObject.Find("CombatHandler").GetComponent<DamageTextHandler>().SpawnDamageText(transform.position, Color.red, healthDamage, 1);
        } else
        {
            InvulnerabilityColor.SetTintColor(ShieldColor, ColorTintDuration);
            GameObject.Find("CombatHandler").GetComponent<DamageTextHandler>().SpawnDamageText(transform.position, ShieldColor, tmp, 1);
        }

        UpdateUIHealth();
        UpdateUIShield();
    }

    public void StartOfBattle()
    {
        SpawnStartMinions();
        DrawCard();
        DrawCard(); 
        TriggerStartOfCombatItemEffects();
    }

    public void DamageAllMinions(int percentage)
    {
        var summonedMinions = Minions.Where(m => m.position != 0).ToList();
        foreach(Minion m in summonedMinions)
        {
            int damage = (m.MaxHp * percentage / 100);
            m.TakeDamage(damage); 
            if (m.Dead)
            {
                KillMinion(m);
            }
        }
    }

    public void KillMinion(Minion m)
    {
        Minions.Remove(m);
        Destroy(m.gameObject);
        m.Effects.Where(e => e.EffectType == EffectType.OnDeath).ToList().ForEach(e => e.Trigger());
        spawnPoints[m.position].transform.Find("Location").GetComponent<Pulse>().Occupied = false;
    }

    public void TriggerStartOfCombatItemEffects()
    {
        Items.ForEach(i => i.Effects.Where(e => e.EffectType == EffectType.StartOfCombat).ToList().ForEach(e => e.Trigger()));
    }

    public void TriggerStartOfTurnItemEffects()
    {
        Items.ForEach(i => i.Effects.Where(e => e.EffectType == EffectType.StartOfTurn).ToList().ForEach(e => e.Trigger()));
    }
    public void TriggerEndOfTurnItemEffects()
    {
        Items.ForEach(i => i.Effects.Where(e => e.EffectType == EffectType.EndOfTurn).ToList().ForEach(e => e.Trigger()));
    }

    public void TriggerEndOfCombatItemEffects()
    {
        Items.ForEach(i => i.Effects.Where(e => e.EffectType == EffectType.EndOfCombat).ToList().ForEach(e => e.Trigger()));
    }

    public void TriggerComboEffects(int count)
    {
        foreach (Minion minion in Minions)
        {
            minion.Effects.Where(e => e.EffectType == EffectType.OnCombo && e.ComboLimit <= count).ToList().ForEach(e => e.Trigger());
        }
    }


    private void SpawnStartMinions()
    {
        foreach (var minionCard in StartMinions)
        {
            Deck.Remove(minionCard);

            AddMinion(minionCard.MinionPrefab);
        }
    }
}
