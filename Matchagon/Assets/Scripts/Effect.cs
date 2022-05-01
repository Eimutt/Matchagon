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

    public EffectType EffectType;
}
[System.Serializable]
public class Effect : GenericEffect<ActionEnum> {
    public void Trigger()
    {
        if (effectName == "3RandomToGreen")
        {
            GameObject.Find("Board").GetComponent<Board>().TransformRandomSpheres(TypeEnum.Grass, 3);
        } else if (effectName == "TurnTimeIncrease3")
        {
            GameObject.Find("CombatHandler").GetComponent<CombatHandler>().IncreaseTimeForNextTurn(3);
        }
    }
}
