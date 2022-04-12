using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingProjectile : MonoBehaviour
{
    public Vector3 target;
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

    public void Init(int damage, Color color, int combo)
    {
        GetComponent<SpriteRenderer>().color = color;
        this.color = color;
        this.damage = damage;
        this.combo = combo;
    }

    // Update is called once per frame
    void Update()
    {
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
            gameObject.transform.position += Vector3.right * speed * Time.deltaTime;

            if (transform.position.x >= target.x)
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
    }
}
