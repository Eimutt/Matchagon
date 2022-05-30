using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string Name;
    public int Cost;
    public int Rarity;
    public Sprite Sprite;
    public string Description;
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

    public void OnMouseEnter()
    {
        DetailedCardInfo detailedCardInfo = GameObject.Find("DetailedCardInfo").GetComponent<DetailedCardInfo>();
        detailedCardInfo.Populate(Sprite, Name, Cost.ToString(), Description, Rarity, true);
    }

    public void OnMouseExit()
    {
        DetailedCardInfo detailedCardInfo = GameObject.Find("DetailedCardInfo").GetComponent<DetailedCardInfo>();
        detailedCardInfo.FadeOut();
    }
}
