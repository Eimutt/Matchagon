using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Board Board;

    public int Shield;
    public int MaxHp;
    public int CurrentHp;
    public int Damage;

    public float ColorTintDuration;
    public Color ShieldColor;
    public Color DamagedColor;
    [SerializeField] private InvulnerabilityColor InvulnerabilityColor;


    public Minion Minions;
    private CombatAnimations CombatAnimations;

    // Start is called before the first frame update
    void Start()
    {
        Board = GameObject.Find("Board").GetComponent<Board>();
        CombatAnimations.GetComponent<CombatAnimations>();
        CurrentHp = MaxHp;

        UpdateUIHealth();
        UpdateUIShield();
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

    public void GetShield(int shield)
    {
        InvulnerabilityColor.SetTintColor(ShieldColor, ColorTintDuration);
        Shield += shield;
        UpdateUIShield();
    }

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



    public void QueueAttack(MatchEnum attackType, TypeEnum type, int damage, int combo, List<Enemy> targets)
    {




        //CombatAnimations.QueueAttack(attackType, type, damage, combo, targets);
    }
}
