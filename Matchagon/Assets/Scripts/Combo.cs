using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo
{
    public int count;
    public Dictionary<TypeEnum, int> damageDict;
    public float damageMultiplier;

    public Combo()
    {
        damageDict = new Dictionary<TypeEnum, int>();
    }

    public void IncreaseDamageType(TypeEnum type, int damage)
    {
        int currentDamage = 0;
        damageDict.TryGetValue(type, out currentDamage);
        
        damageDict[type] = currentDamage + damage;
        count++;

        damageMultiplier = 1 + 0.25f * (count - 1);
    }
}
