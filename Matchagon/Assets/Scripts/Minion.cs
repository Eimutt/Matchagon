using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    private CombatAnimations CombatAnimations;

    public int[] Damages = new int[0];

    // Start is called before the first frame update
    void Start()
    {

        CombatAnimations.GetComponent<CombatAnimations>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
