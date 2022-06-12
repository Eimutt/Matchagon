using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float Speed;
    public bool ClickedMoving;

    private Vector3 MousePos;
    private GameObject ParentObj;

    private Vector3 ClickPos;
    private Vector3 StartPos;
    // Start is called before the first frame update
    void Start()
    {
        ParentObj = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (ClickedMoving)
        {
            Vector3 newMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            Vector3 diff = newMousePos - ClickPos;

            ParentObj.transform.position = StartPos + diff;
        }

        if (Input.GetKeyDown("space"))
        {
            ParentObj.transform.position = Vector3.zero - GameObject.Find("PlayerSprite(Clone)").transform.localPosition;
        }
    }

    public void OnMouseDown()
    {
        ClickedMoving = true;
        ClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        StartPos = ParentObj.transform.position;
    }

    public void OnMouseUp()
    {
        ClickedMoving = false;
    }
}
