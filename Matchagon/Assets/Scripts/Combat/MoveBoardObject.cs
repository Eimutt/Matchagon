using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoardObject : BoardObject
{
    public int Order; 

    public int Index;

    public int Direction; //0,1,2,3

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(int order, int index, int direction, int sourceId)
    {
        Debug.Log(direction);
        Order = order;
        Index = index;
        Direction = direction;
        Source = sourceId;

        if (direction == 0)
        {
            transform.position = new Vector3(2.5f, index);
        }
        else if (direction == 1)
        {
            transform.position = new Vector3(index, 2.5f);
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == 2)
        {
            transform.position = new Vector3(2.5f, index);
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (direction == 3)
        {
            transform.position = new Vector3(index, 2.5f);
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }


    }

    public void Trigger()
    {
        if (Direction == 0)
        {
            GameObject.Find("Board").GetComponent<Board>().MoveRow(Index, false);
        }
        else if (Direction == 1)
        {
            GameObject.Find("Board").GetComponent<Board>().MoveColumn(Index, false);
        }
        else if (Direction == 2)
        {
            GameObject.Find("Board").GetComponent<Board>().MoveRow(Index, true);
        }
        else if (Direction == 3)
        {
            GameObject.Find("Board").GetComponent<Board>().MoveColumn(Index, true);
        }

        Direction = -1;
        Destroy(gameObject);
    }
}
