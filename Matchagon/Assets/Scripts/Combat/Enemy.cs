using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int MaxHp;
    public int CurrentHp;
    public int Damage;

    public UnitAbilities unitAbilities;
    private int t = 0;

    public bool Dead;

    public int incomingDamage;

    public Vector3 startPos;

    public List<Effect> Effects;


    private GameObject Tooltip;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHp = MaxHp;
        Tooltip = GameObject.Find("EnemyTooltipObject");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        incomingDamage -= damage;
        CurrentHp -= damage;
        float percentage = (float)CurrentHp / (float)MaxHp;
        transform.Find("Canvas/Slider").GetComponent<Slider>().value = percentage;

        if(CurrentHp < 0)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);

        }

    }

    public float TakeTurn(Player x, List<Enemy> enemies)
    {
        if (Dead) { Destroy(gameObject); }
        var ability = unitAbilities.Abilities[t % unitAbilities.Abilities.Count];

        if(ability.Type == ActionEnum.Attack)
        {
            x.AttackFirstMinion(Damage);
            GetComponent<Shake>().Move();

        }
        else if(ability.Type == ActionEnum.Heal)
        {
            TakeDamage(-Damage);
            GameObject.Find("CombatHandler").GetComponent<DamageTextHandler>().SpawnHealText(gameObject.transform.position + new Vector3(0, -0.7f, 0), Color.green, Damage * 2);

        }

        t++;
        return ability.time;
    }

    public void AddIncomingDamage(int damage)
    {
        incomingDamage += damage;
        if(CurrentHp <= incomingDamage)
        {
            Dead = true;
        }
    }

    public int GetDamageLeftAfterIncoming()
    {
        return CurrentHp - incomingDamage;
    }

    public void OnMouseEnter()
    {
        Tooltip.transform.position = Input.mousePosition;
        //Tooltip.SetActive(true);
        PopulateTooltipObject();
    }

    public void OnMouseExit()
    {
        //Tooltip.SetActive(false);
        Tooltip.transform.position = new Vector3(-1000, 0, 0);
    }

    private void PopulateTooltipObject()
    {
        //foreach(UnitAbilities unitAbilities in UnitAbilities)
        //{
        //    Tooltip.transform
        //}

        //foreach (UnitAbilities unitAbilities in UnitAbilities)
        //{
        //    Tooltip.transform
        //}
        //Tooltip.transform.Find("Fire/Text").GetComponent<Text>().text = Damages[0].ToString();
        //Tooltip.transform.Find("Water/Text").GetComponent<Text>().text = Damages[1].ToString();
        //Tooltip.transform.Find("Grass/Text").GetComponent<Text>().text = Damages[2].ToString();
        //Tooltip.transform.Find("Dark/Text").GetComponent<Text>().text = Damages[3].ToString();
        //Tooltip.transform.Find("Light/Text").GetComponent<Text>().text = Damages[4].ToString();
        //Tooltip.transform.Find("Shield/Text").GetComponent<Text>().text = Damages[5].ToString();
    }
}
