using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EffectType
{
    StartOfTurn,
    Passive,
    OnKill,
    OnDeath,
    SummonEffect,
    StartOfCombat,
    OnPickUp,
    OnRemoval,
    OnCombo
}

[Serializable] public class IntEvent : UnityEvent<int> { }

public abstract class GenericEffect<T>
{
    public string effectName;
    public int effectStrength;
    public EffectType EffectType;
    public int ComboLimit;


    public IntEvent Function;
}
[System.Serializable]
public class Effect : GenericEffect<ActionEnum> {
    public void Trigger()
    {
        Function.Invoke(effectStrength);



        if (effectName == "RandomToGreen")
        {
            GameObject.Find("Board").GetComponent<Board>().TransformRandomSpheres(TypeEnum.Grass, effectStrength);
        }
        if (effectName == "HideRandom")
        {
            GameObject.Find("Board").GetComponent<Board>().HideRandomSpheres(effectStrength);
        }


        else if (effectName == "TurnTimeIncrease")
        {
            GameObject.Find("CombatHandler").GetComponent<CombatHandler>().IncreaseTimeForNextTurn(effectStrength);
        }
        else if (effectName == "GrassDamageMultiplier")
        {
            GameObject.Find("Player").GetComponent<Player>().ChangeDamage(TypeEnum.Grass, effectStrength);
        }
        else if (effectName == "WaterDamageMultiplier")
        {
            GameObject.Find("Player").GetComponent<Player>().ChangeDamage(TypeEnum.Water, effectStrength);
        }
        else if (effectName == "ArmorStartOfTurn")
        {
            GameObject.Find("Player").GetComponent<Player>().GetShield(effectStrength);
        }
        else if (effectName == "ModifySpawnRateFire")
        {
            GameObject.Find("Board").GetComponent<SphereGenerator>().MultiplyWeight(TypeEnum.Fire, effectStrength);
        }
        else if (effectName == "ModifySpawnRateWater")
        {
            GameObject.Find("Board").GetComponent<SphereGenerator>().MultiplyWeight(TypeEnum.Water, effectStrength);
        }
        else if (effectName == "ModifySpawnRateGrass")
        {
            GameObject.Find("Board").GetComponent<SphereGenerator>().MultiplyWeight(TypeEnum.Grass, effectStrength);
        }
        else if (effectName == "ModifySpawnRateMana")
        {
            GameObject.Find("Board").GetComponent<SphereGenerator>().MultiplyWeight(TypeEnum.Light, effectStrength);
        }

        

    }
    
}
