using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIHandler : MonoBehaviour
{
    public GameObject defaultText;
    public bool showDamageText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void DamageEnemy(GameObject target, int damage, float percentHp)
    //{
    //    if (showDamageText)
    //    {
    //        CreateDamageText(target, damage);
    //    }

    //    //target.transform.Find("Canvas/Slider").GetComponent<Slider>().value = percentHp;
    //}


    public void CreateDamageText(Vector3 position, TypeEnum type, int damage)
    {
        GameObject damageText = Instantiate(defaultText, position, Quaternion.identity, gameObject.transform);
        DamageNumber dNum = damageText.GetComponent<DamageNumber>();


        var color = ToColor(type);

        dNum.Init(color, damage);
    }

    public Color ToColor(TypeEnum type)
    {
        if (type == TypeEnum.Fire) return new Color(255f/255f, 200f/255f, 0);
        if (type == TypeEnum.Grass) return new Color(20f/255f, 199f/255f, 41f/255f);
        if (type == TypeEnum.Water) return new Color(15f/255f, 169f/255f, 212f/255f);
        if (type == TypeEnum.Light) return new Color(201f/255f, 188f/255f, 62f/255f);

        return Color.white;
    }
}
