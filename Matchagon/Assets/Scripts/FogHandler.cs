using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogHandler : MonoBehaviour
{

    private Material material;
    private Color materialTintColor;


    private Color materialColor;
    private bool fadeout;

    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        Player = GameObject.Find("PlayerSprite(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        material.SetVector("_CharacterPosition", Player.transform.position);
    }

    public void SetPosition()
    {

    }
}
