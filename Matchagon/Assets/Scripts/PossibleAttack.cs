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
    public PossibleAttack(Minion source, MatchEnum attackType, TypeEnum type, int damage, int combo, float multiplier)
    {
        this.source = source;
        this.attackType = attackType;
        this.type = type;
        this.baseDamage = damage;
        this.fullDamage = (int)(damage * multiplier);
        this.combo = combo;
    }
}
