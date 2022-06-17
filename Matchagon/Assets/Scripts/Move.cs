using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Vector3 TargetPos;
    private Vector3 StartPos;
    private float t;

    public float Duration;

    private Action Action;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / Duration;
        gameObject.transform.position = t * TargetPos + (1 - t) * StartPos;

        if(t >= 1)
        {
            gameObject.transform.position = TargetPos;
            if (Action != null) Action.Invoke();
            Destroy(this);
        }
    }

    public void Init(Vector3 target, float duration, Action action)
    {
        StartPos = gameObject.transform.position;
        Duration = duration;
        TargetPos = target;
        Action = action;
    }

    public void Init(Vector3 target, float duration)
    {
        StartPos = gameObject.transform.position;
        Duration = duration;
        TargetPos = target;
    }
}
