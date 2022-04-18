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

    public float ColorTintDuration;
    //public Color ShieldColor;
    public Color DamagedColor;
    [SerializeField]
    private InvulnerabilityColor InvulnerabilityColor;

    public List<Minion> Minions;

    // Start is called before the first frame update
    void Start()
    {
        Board = GameObject.Find("Board").GetComponent<Board>();
        CurrentHp = MaxHp;

        UpdateUIHealth();
        UpdateUIShield();

        Minions.Add(GetComponent<Minion>());

        InvulnerabilityColor = GetComponent<InvulnerabilityColor>();
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
        CurrentHp -= Mathf.Max(0, damage);

        InvulnerabilityColor.SetTintColor(DamagedColor, ColorTintDuration);

        UpdateUIHealth();
        UpdateUIShield();

    }

    //public void GetShield(int shield)
    //{
    //    InvulnerabilityColor.SetTintColor(ShieldColor, ColorTintDuration);
    //    Shield += shield;
    //    UpdateUIShield();
    //}

    public void ResetShield()
    {
        Shield = 0;
        UpdateUIShield();
    }

    public void NewTurn()
    {
        ResetShield();
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
                if (enemyHandler.NoEnemiesLeft())
                {
                    continue;
                }

                foreach (Minion minion in Minions)
                {
                    int minionElementDamage = minion.Damages[(int)entry.Key];
                    if (minionElementDamage != 0)
                    {
                        attacks.Add(new PossibleAttack(minion, MatchEnum.Blob, entry.Key, value * minionElementDamage, combo.count));
                    }
                }
            }
        }


        //foreach() LAUNCH AOES

        //Loop through attacks and enemies
        attacks.Sort((x, y) => y.damage.CompareTo(x.damage));

        int i = 0;
        int max = attacks.Count;

        while (i < max && !enemyHandler.NoEnemiesLeft())
        {
            var enemy = enemyHandler.GetFirstEnemy();

            var possibleAttack = attacks.FirstOrDefault(a => a.damage * a.combo > enemy.GetDamageLeftAfterIncoming());

            if (possibleAttack == null)
            {
                possibleAttack = attacks[0];
            }

            enemy.AddIncomingDamage(possibleAttack.damage * combo.count);
            attacks.Remove(possibleAttack);
            possibleAttack.source.GetComponent<CombatAnimations>().QueueAttack(MatchEnum.Blob, possibleAttack.type, possibleAttack.damage, combo.count, enemy.gameObject);

            i++;
        }
    }
}
