using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaProjectile : MonoBehaviour
{
    public List<GameObject> targets;
    public float speed;
    public float Duration;
    private float t;
    public float GrowthRate;
    private int baseDamage;
    private int fullDamage;
    private int combo;
    private Color color;

    public GameObject damageTextPrefab;

    public bool DamageDone;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(int baseDamage, int fullDamage, Color color, int combo, List<GameObject> targets)
    {
        GetComponent<SpriteRenderer>().color = color;
        this.color = color;
        this.baseDamage = baseDamage;
        this.fullDamage = fullDamage;
        this.combo = combo;
        this.targets = targets;
    }

    // Update is called once per frame
    void Update()
    {
        
        t += Time.deltaTime;

        gameObject.transform.position += Vector3.right * speed * Time.deltaTime;


        gameObject.transform.localScale = new Vector3(1 * GrowthRate * (1 + t), 3 * GrowthRate * (1 + t), 1);
        
        if(gameObject.transform.position.x > 8 && !DamageDone)
        {
            targets.ForEach(t => ReachTarget(t));
            DamageDone = true;
        }
        
        if (gameObject.transform.position.x > 12)
        {
            Destroy(gameObject);
            
        }
    }

    void ReachTarget(GameObject target)
    {
        //GetComponent<Animator>().enabled = true;
        GameObject.Find("CombatHandler").GetComponent<DamageTextHandler>().SpawnDamageText(target.transform.position, color, baseDamage, combo);

        target.GetComponent<Enemy>().TakeDamage(fullDamage);
    }
}
