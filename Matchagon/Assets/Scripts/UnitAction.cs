using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionEnum {
    Attack,
    AttackAoe,
    Debuff,
    Buff,
    Wait,
    Heal
}

public abstract class GenericUnitAbility <T>
{
    public T Type;
    public float id;
    public float time;
}

public abstract class GenericUnitAbilityList<T, U> where T : GenericUnitAbility<U>
{
    [SerializeField]
    public List<T> Abilities;
}

[System.Serializable]
public class Ability : GenericUnitAbility<ActionEnum> { }

[System.Serializable]
public class UnitAbilities : GenericUnitAbilityList<Ability, ActionEnum> { }
