using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    public float TargetSize;
    public float duration;

    private float t;

    private bool active;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {

        transform.localScale = new Vector3(1, 1);
        t = 0;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        t += Time.deltaTime / duration;

        var size = 1f - (t * (1 - TargetSize));


        transform.localScale = new Vector3(size, size);

        if (t > 1)
        active = false;
                
    }
}
