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
        GetComponent<SpriteRenderer>().color = TypeEnumGenerator.GetColor(Type);
        
    }

    public TypeEnum GetType() { return Type; }

    public void SetDestroy()
    {
        destroy = true;
    }
}
