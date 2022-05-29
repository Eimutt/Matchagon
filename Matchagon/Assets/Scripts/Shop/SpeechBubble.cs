using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public float LifeTime;
    private float t;

    public Image Image;
    // Start is called before the first frame update
    void Start()
    {
        Image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if(t > LifeTime)
        {
            Image.enabled = false;
        }
    }

    public void SetSprite(Sprite sprite)
    {
        Image.sprite = sprite;
        Image.enabled = true;
        t = 0;
    }
}
