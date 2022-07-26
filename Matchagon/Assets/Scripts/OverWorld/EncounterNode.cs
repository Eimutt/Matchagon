using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EncounterNode : MapNode
{
    public int Difficulty;
    public List<Encounter> EncounterVariants;
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
        var encounter = EncounterVariants.Where(e => e.Difficulty <= Difficulty).OrderByDescending(e => e.Difficulty).First();
        GameObject.Find("GameHandler").GetComponent<GameHandler>().EnterCombat(encounter);
    }
}
