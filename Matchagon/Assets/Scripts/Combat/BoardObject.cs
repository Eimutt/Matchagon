using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardObject : MonoBehaviour
{
    public int TurnDuration;
    public int Source;
    public float moveDur;
    public Vector2Int Min;
    public Vector2Int Max;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveRandomDirection()
    {
        bool done = false;
        Vector3 dir = Vector3.zero;
        while (!done)
        {
            var r = Random.Range(0, 4);

            switch (r)
            {
                case 0:
                    dir = new Vector3(1, 0);
                    break;
                case 1:
                    dir = new Vector3(-1, 0);
                    break;
                case 2:
                    dir = new Vector3(0, 1);
                    break;
                case 3:
                    dir = new Vector3(0, -1);
                    break;
                default:
                    dir = new Vector3(1, 0);
                    break;
            }

            if (transform.position.x + dir.x > Max.x - 1 || transform.position.y + dir.y > Max.y - 1 || transform.position.x + dir.x < Min.x || transform.position.y + dir.y < Min.y)
                continue;


            done = true;
        }

        var move1 = gameObject.AddComponent<Move>();
        move1.Init(transform.position + dir, moveDur);
    }
}
