using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    StartOfTurn,
    Passive,
    OnKill,
    OnDeath,
    SummonEffect,
    StartOfCombat,
    OnPickUp,
    OnRemoval
}

public abstract class GenericEffect<T>
{

    public string effectName;
    public int effectStrength;
    public EffectType EffectType;
}
[System.Serializable]
public class Effect : GenericEffect<ActionEnum> {
    public void Trigger()
    {
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
        else if (effectName == "DrawCard")
        {
            GameObject.Find("Player").GetComponent<Player>().DrawCard();
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
        else if (effectName == "ModifySpawnPower")
        {
            GameObject.Find("GameHandler").GetComponent<PlayerData>().ModifySpawnPower(effectStrength);
        }
    }
}
