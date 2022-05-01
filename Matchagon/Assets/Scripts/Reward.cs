using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    public void Start()
    {
        Debug.Log("new card");
    }

    public virtual void PickReward()
    {

    }
}

public class ItemReward : Reward
{
    public Item item;
    public override void PickReward()
    {

        GameObject.Find("GameHandler").GetComponent<PlayerData>().GetItem(item);
    }
}
