using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingProjectile : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public float ExplosionDuration;
    private float t;
    public float GrowthRate;
    private int damage;
    private int combo;
    private Color color;

    public GameObject damageTextPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int damage, Color color, int combo, GameObject target)
    {
        GetComponent<SpriteRenderer>().color = color;
        this.color = color;
        this.damage = damage;
        this.combo = combo;
        this.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) Destroy(gameObject);

        if(speed == 0)
        {
            t += Time.deltaTime;
            gameObject.transform.localScale = Vector3.one * GrowthRate * (1+t);
            if(t >= ExplosionDuration)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            gameObject.transform.position += Vector3.Normalize(target.transform.position - gameObject.transform.position) * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.transform.position) < 0.05)
            {
                ReachTarget();
            }
        }
    }

    void ReachTarget()
    {
        speed = 0;
        GetComponent<Animator>().enabled = true;

        GameObject damageText = Instantiate(damageTextPrefab, gameObject.transform.position, Quaternion.identity);
        DamageNumber dNum = damageText.GetComponent<DamageNumber>();

        dNum.Init(color, damage, combo);

        target.GetComponent<Enemy>().TakeDamage(damage * combo);
    }
}
