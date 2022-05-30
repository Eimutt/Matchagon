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
    public bool Fades;

    // Start is called before the first frame update
    void Start()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        //CanvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Fades)
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
        

    }

    public void Populate(Sprite sprite, string name, string cost, string description, int rarity, bool fades)
    {
        transform.Find("Image").GetComponent<Image>().sprite = sprite;
        transform.Find("Name").GetComponent<Text>().text = name;
        transform.Find("Cost").GetComponent<Text>().text = cost;
        transform.Find("Description").GetComponent<Text>().text = description;
        transform.Find("Rarity").GetComponent<Image>().color = GetColor(rarity);
        Fades = fades;
        CanvasGroup.alpha = fades ? 0 : 1;
        Active = true;
    }

    public void FadeOut()
    {
        CanvasGroup.alpha = 0;
        Active = false;
    }

    public Color GetColor(int rarity)
    {
        if (rarity == 1) return new Color32(50, 200, 30, 181);
        if (rarity == 2) return new Color32(207, 9, 0, 201);
        if (rarity == 3) return new Color32(102, 0, 255, 221);


        return new Color32(85, 107, 108, 161);
    }
}
