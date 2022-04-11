using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumberHandler : MonoBehaviour
{
    public GameObject defaultText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateDamageText(int damage)
    {
        GameObject text = Instantiate(defaultText);
        Text myText = text.GetComponent<Text>();
        myText.text = damage.ToString();
        text.transform.parent = this.transform;
    }
}
