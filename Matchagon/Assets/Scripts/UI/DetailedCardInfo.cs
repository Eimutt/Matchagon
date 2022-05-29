using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailedCardInfo : MonoBehaviour
{
    public CanvasGroup CanvasGroup;

    public bool Active;
    public float t;
    public float FadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        CanvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            t += Time.deltaTime * 1f / FadeSpeed;
            if (t > 1)
            {
                t = 1;
            }
        }
        else
        {
            t -= Time.deltaTime * 1f / FadeSpeed;
            if (t < 0)
            {
                t = 0;
            } 
        }


        CanvasGroup.alpha = t;
    }

    public void Populate(Sprite sprite, string cost, string description)
    {
        transform.Find("Image").GetComponent<Image>().sprite = sprite;
        transform.Find("Cost").GetComponent<Text>().text = cost;
        transform.Find("Description").GetComponent<Text>().text = description;
        Active = true;
    }

    public void FadeOut()
    {
        Active = false;
    }
}
