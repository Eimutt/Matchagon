using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class TransformExtensions
{
    static public void Clear(this Transform transform)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    static public void ClearChildren(this Transform transform)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).Clear();
        }
    }

}
