using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextHandler : MonoBehaviour
{

    public GameObject damageTextPrefab;
    public GameObject healTextPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDamageText(Vector3 position, Color color, int baseDamage, int combo)
    {

        GameObject damageText = Instantiate(damageTextPrefab, position, Quaternion.identity);
        DamageNumber dNum = damageText.GetComponent<DamageNumber>();

        dNum.Init(color, baseDamage, combo, false);

    }

    public void SpawnHealText(Vector3 position, Color color, int healAmount)
    {

        GameObject damageText = Instantiate(healTextPrefab, position, Quaternion.identity);
        DamageNumber dNum = damageText.GetComponent<DamageNumber>();

        dNum.Init(color, healAmount, 1, true);
    }
}
