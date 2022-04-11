using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sphere : MonoBehaviour
{
    private TypeEnum Type;
    public bool destroy;

    public float t;

    // Start is called before the first frame update
    void Start()
    {
        Type = TypeEnumGenerator.GetRandomType();
        ColorSphere();
    }

    // Update is called once per frame
    void Update()
    {
        if (destroy)
        {
            t += Time.deltaTime;
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = 1 - t;
            GetComponent<SpriteRenderer>().color = tmp;

            if(t > 1)
            {
                Destroy(this);
            }
        }
    }

    private void ColorSphere()
    {
        switch (Type)
        {
            case TypeEnum.Fire:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case TypeEnum.Water:

                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case TypeEnum.Grass:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            //case TypeEnum.Dark:
            //    GetComponent<SpriteRenderer>().color = Color.magenta;
            //    break;
            case TypeEnum.Light:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
        }
    }

    public TypeEnum GetType() { return Type; }

    public void SetDestroy()
    {
        destroy = true;
    }
}
