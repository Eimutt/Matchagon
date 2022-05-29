using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleAttack
{
    public Minion source;
    public MatchEnum attackType;
    public TypeEnum type;
    public int baseDamage;
    public int fullDamage;
    public int combo;
    public bool Area;
    public PossibleAttack(Minion source, MatchEnum attackType, TypeEnum type, float damage, int combo, float multiplier, bool area)
    {
        this.source = source;
        this.attackType = attackType;
        this.type = type;
        this.baseDamage = (int)damage;
        this.fullDamage = (int)(damage * multiplier);
        this.combo = combo;
        this.Area = area;
    }
}
