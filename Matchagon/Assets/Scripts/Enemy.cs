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
    // Start is called before the first frame update
    void Start()
    {
        CurrentHp = MaxHp;
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
            x.TakeDamage(Damage);
            Debug.Log("damaging " + x.name + " for " + Damage.ToString());
            GetComponent<Shake>().Move();
        } else if(ability.Type == ActionEnum.Wait)
        {

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
}
