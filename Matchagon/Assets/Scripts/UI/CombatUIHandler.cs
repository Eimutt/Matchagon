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


    //public void CreateDamageText(Vector3 position, TypeEnum type, int damage)
    //{
    //    GameObject damageText = Instantiate(defaultText, position, Quaternion.identity, gameObject.transform);
    //    DamageNumber dNum = damageText.GetComponent<DamageNumber>();


    //    var color = TypeEnumGenerator.GetColor(type);

    //    dNum.Init(color, damage);
    //}
}
