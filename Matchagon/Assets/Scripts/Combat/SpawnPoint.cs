using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public int position;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Number").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + position.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        GameObject.Find("Player").GetComponent<Player>().AddMinion(position);
    }
}
