using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int Cost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Play()
    {

    }

    public void OnMouseDown()
    {
        GameObject.Find("Player").GetComponent<Player>().PlayCard(this);
    }
}
