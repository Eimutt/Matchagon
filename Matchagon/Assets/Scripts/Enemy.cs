using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHp;
    public int CurrentHp;
    public int Damage;
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentHp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
