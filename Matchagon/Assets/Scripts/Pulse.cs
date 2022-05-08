using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float Difference;
    public float speed;

    public float  alphaStart;

    public float t;

    public SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        alphaStart = sprite.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        t += speed * Time.deltaTime;
        var sinValue = Mathf.Sin(t);

        sprite.color = new Color(1, 1, 1, alphaStart + Difference * sinValue);
    }
}
