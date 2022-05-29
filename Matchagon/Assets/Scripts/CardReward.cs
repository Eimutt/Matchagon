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
        btn.onClick.AddListener(SelectCard);
    }

    public void SelectCard()
    {
        GameObject.Find("VictoryScreen").GetComponent<RewardScreen>().SelectReward(this);
    }

}
