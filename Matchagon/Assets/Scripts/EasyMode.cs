using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasyMode : MonoBehaviour
{
    private bool Active;
    public Item Item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle()
    {
        if (!Active)
        {
            GameObject.Find("GameHandler").GetComponent<PlayerData>().AddItem(Item);
            Active = true;
            transform.Find("Text").GetComponent<Text>().text = "Disable Easy Mode (+50% damage, +8sec turn time)";
        } else
        {
            GameObject.Find("GameHandler").GetComponent<PlayerData>().RemoveItem(Item);
            Active = false;
            transform.Find("Text").GetComponent<Text>().text = "Enable Easy Mode (+50% damage, +8sec turn time)";
        }
    }
}
