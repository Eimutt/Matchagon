using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimations : MonoBehaviour
{
    public GameObject BasicAttack;

    public float Delay;
    public Queue<Attack> attackQueue;
    
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        attackQueue = new Queue<Attack>();
        t = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(attackQueue.Count != 0)
        {
            t += Time.deltaTime;
            if(t > Delay)
            {
                SpawnAttack();
                t = 0;
                Delay *= 0.75f;
            }
        }
        Delay = 0.5f;
    }

    public void QueueAttack(MatchEnum attackType, TypeEnum type, int damage, int combo)
    {
        attackQueue.Enqueue(new Attack(attackType, type, damage, combo));
    }

    private void SpawnAttack()
    {
        var attack = attackQueue.Dequeue();

        if (attack.attackType == MatchEnum.Blob)
        {
            GameObject basicAttack = Instantiate(BasicAttack, gameObject.transform.position, Quaternion.Euler(0, 0, -90), gameObject.transform);
            ExplodingProjectile expProj = basicAttack.GetComponent<ExplodingProjectile>();


            var color = TypeEnumGenerator.GetColor(attack.type);

            expProj.Init(attack.damage, color, attack.combo);
        }
    }
}

public class Attack {

    public MatchEnum attackType;
    public TypeEnum type;
    public int damage;
    public int combo;
    public Attack(MatchEnum attackType, TypeEnum type, int damage, int combo)
    {
        this.attackType = attackType;
        this.type = type;
        this.damage = damage;
        this.combo = combo;
    }
}
