using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleAttack
{
    public Minion source;
    public MatchEnum attackType;
    public TypeEnum type;
    public int damage;
    public int combo;
    public PossibleAttack(Minion source, MatchEnum attackType, TypeEnum type, int damage, int combo)
    {
        this.source = source;
        this.attackType = attackType;
        this.type = type;
        this.damage = damage;
        this.combo = combo;
    }
}
