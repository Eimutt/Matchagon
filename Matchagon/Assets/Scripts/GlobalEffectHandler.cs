using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEffectHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DrawCard(int amount)
    {
        GameObject.Find("Player").GetComponent<Player>().DrawCard();
    }

    public void ModifySpawnPower(int amount)
    {

    }

    public void Test()
    {

    }
}
