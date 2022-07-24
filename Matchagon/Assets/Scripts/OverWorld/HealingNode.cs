using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingNode : MapNode
{
    public int Amount;
    public float GlobalMultiplier;
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
        GameObject.Find("GameHandler").GetComponent<GameHandler>().EnterHealingNode(Amount);
    }
}
