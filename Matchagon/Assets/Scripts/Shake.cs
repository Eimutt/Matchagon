using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float Speed;
    public List<Vector3> Points;

    public bool moving;
    public int i;

    public Vector3 next;
    // Start is called before the first frame update
    void Start()
    {
        Points.Add(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            transform.position += Vector3.Normalize(next - transform.position) * Speed * Time.deltaTime;
            
            if (Vector3.Distance(transform.position, next) < 0.05)
            {
                transform.position = next;
                ReachPoint();
            }
        }
    }

    public void Move()
    {
        moving = true;
        next = Points[0];
        i = 0;
    }

    private void ReachPoint()
    {
        i++;
        if (i == Points.Count)
        {
            moving = false;
            return;
        }
        next = Points[i];
    }
}
