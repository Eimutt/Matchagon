using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReward : Reward
{
    public Card card;
    public override void PickReward()
    {

        GameObject.Find("GameHandler").GetComponent<PlayerData>().GetCard(card);
    }
}
