using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    public void Start()
    {
        Debug.Log("new card"); 
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(PickRewar2d);
    }

    public virtual void PickReward()
    {
        var i = 1;
    }
    public void PickRewar2d()
    {
        var i = 1;
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
