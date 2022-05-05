using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sphere : MonoBehaviour
{
    public TypeEnum Type;
    public bool destroy;
    private bool fadeout;

    public float t;

    public float ColorTintDuration;
    //public Color ShieldColor;
    [SerializeField]
    private InvulnerabilityColor InvulnerabilityColor;
    // Start is called before the first frame update
    void Start()
    {
        //Type = TypeEnumGenerator.GetRandomType();
        //ColorSphere();
        InvulnerabilityColor = GetComponent<InvulnerabilityColor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeout)
        {
            t += Time.deltaTime;
            //Color tmp = GetComponent<SpriteRenderer>().color;
            //tmp.a = 1 - (2 * t);
            //GetComponent<SpriteRenderer>().color = tmp;

            if(t > ColorTintDuration * 2)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Init(TypeEnum type)
    {
        Type = type;
        ColorSphere();
    }

    private void ColorSphere()
    {
        GetComponent<SpriteRenderer>().color = TypeEnumGenerator.GetColor(Type);
        
    }

    public TypeEnum GetType() { return Type; }

    public void TriggerMatch()
    {
        destroy = true;
        
        InvulnerabilityColor.SetTintColor(TypeEnumGenerator.GetColor(Type), ColorTintDuration);

    }

    public void SetFadeOut()
    {
        fadeout = true;
        InvulnerabilityColor.SetFadeOut(TypeEnumGenerator.GetColor(Type), ColorTintDuration*2);
    }

    public void SetType(TypeEnum type, Sprite sprite)
    {
        Type = type;
        GetComponent<SpriteRenderer>().sprite = sprite;
        //ColorSphere();
    }
}
