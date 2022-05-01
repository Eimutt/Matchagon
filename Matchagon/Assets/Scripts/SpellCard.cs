using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCard : Card
{
    public string spellType;
    public int amount;
    public TypeEnum elementType;
    public override void Play()
    {
        if(spellType == "BoardModification")
        {
            GameObject.Find("Board").GetComponent<Board>().TransformRandomSpheres(elementType, amount);
        } else if (spellType == "TimeIncrease")
        {
            GameObject.Find("CombatHandler").GetComponent<CombatHandler>().IncreaseTimeForNextTurn(amount);
        }
    }
}
