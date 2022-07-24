using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Unit
{
    public int Damage;

    public UnitAbilities unitAbilities;
    private int t = 0;

    public bool Dead;

    public int incomingDamage;

    public Vector3 startPos;

    public List<Effect> Effects;

    private UnitInfo UnitInfo;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHp = MaxHp;
        UnitInfo = GameObject.Find("UnitInfo").GetComponent<UnitInfo>();
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

    public void OnMouseDown()
    {

        UnitInfo.PopulateUi(this);
    }

    public void OnMouseEnter()
    {
        
    }

    public void OnMouseExit()
    {

    }

}
