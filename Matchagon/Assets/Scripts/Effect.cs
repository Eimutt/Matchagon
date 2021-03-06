using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EffectType
{
    StartOfTurn,
    EndOfTurn,
    Passive,
    OnKill,
    OnDeath,
    SummonEffect,
    StartOfCombat,
    OnPickUp,
    OnRemoval,
    OnComboOver,
    EndOfCombat,
    OnComboUnder
}

[Serializable] public class IntIntEvent : UnityEvent<int, int> { }
[Serializable] public class IntEvent : UnityEvent<int> { }

public abstract class GenericEffect<T>
{
    public string effectName;
    public int effectStrength;
    public EffectType EffectType;
    public int ComboLimit;
    public string Description;
    public bool Visible;

    public IntIntEvent InstanceFunction;
    public IntEvent Function;
}
[System.Serializable]
public class Effect : GenericEffect<ActionEnum> {
    public void Trigger(int instanceId)
    {
        if(InstanceFunction != null)
            InstanceFunction.Invoke(instanceId, effectStrength);
        
        if(Function != null)
            Function.Invoke(effectStrength);


    }

    public void Trigger()
    {
        Function.Invoke(effectStrength);

    }

}
