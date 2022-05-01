using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardReward : Reward
{
    public Card card;

    public void Start()
    {
        base.Start();
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(PickReward);
    }
    public void PickReward()
    {
        GameObject.Find("GameHandler").GetComponent<PlayerData>().GetCard(card);

        GameObject.Find("GameHandler").GetComponent<GameHandler>().CloseRewards();
    }

}
