using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNode : MapNode
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ActivateNode()
    {

        GameObject.Find("GameHandler").GetComponent<GameHandler>().EnterShop();
    }
}
