using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardScreen : MonoBehaviour
{
    public Card[] CardPrefabs;
    public GameObject CardBase;
    // Start is called before the first frame update
    void Start()
    {
        CardPrefabs = Resources.LoadAll<Card>("Prefab/Card");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GenerateCardRewards(0);
        }
    }

    public void GenerateCardRewards(int rarity)
    {
        int i = UnityEngine.Random.Range(0, CardPrefabs.Length);
        var card = CardPrefabs[i];

        var card1 = Instantiate(CardBase, transform);
        card1.GetComponent<RectTransform>().localPosition = new Vector3(-90,0,0);GetComponent<RectTransform>();

    }
}
