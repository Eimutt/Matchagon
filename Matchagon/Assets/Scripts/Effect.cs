using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    StartOfTurn,
    Passive,
    OnKill,
    OnDeath,
    SummonEffect
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
        } else if (effectName == "TurnTimeIncrease")
        {
            GameObject.Find("CombatHandler").GetComponent<CombatHandler>().IncreaseTimeForNextTurn(effectStrength);
        }
    }
}
