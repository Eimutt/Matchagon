using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float Distance;
    public float speed;

    public Vector3 start;

    public float t;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        t += speed * Time.deltaTime;
        var sinValue = Mathf.Sin(t);

        transform.position = start + Vector3.up * Distance * sinValue;
    }
}
