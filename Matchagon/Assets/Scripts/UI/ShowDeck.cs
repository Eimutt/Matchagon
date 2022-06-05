using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDeck : MonoBehaviour
{
    public GameObject DeckContainer;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        DeckContainer = GameObject.Find("DeckContainer");
        DeckContainer.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetKeyDown("escape"))
        {
            Close();
        }
    }

    public void Click()
    {
        if (active)
        {
            Close();

        } else
        {
            Show();
        }
    }

    public void Show()
    {
        DeckContainer.GetComponent<DeckView>().Show();

        active = true;
        GameObject.Find("WorldMap").GetComponent<WorldMap>().SetMove(false);
    }

    public void Close()
    {
        DeckContainer.GetComponent<DeckView>().Close();
        active = false;
        GameObject.Find("WorldMap").GetComponent<WorldMap>().SetMove(true);
    }
}
