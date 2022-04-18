using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionCard : Card
{
    public GameObject MinionPrefab;

    public override void Play()
    {
        GameObject.Find("Player").GetComponent<Player>().AddMinion(MinionPrefab);
    }
}
