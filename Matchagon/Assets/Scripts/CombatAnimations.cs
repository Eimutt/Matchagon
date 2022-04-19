using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimations : MonoBehaviour
{
    public GameObject BasicAttack;
    public GameObject Shield;

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

    //public void QueueAttack(MatchEnum attackType, TypeEnum type, int damage, int combo, List<GameObject> targets)
    //{
    //    attackQueue.Enqueue(new Attack(attackType, type, damage, combo, targets));
    //}
    public void QueueAttack(MatchEnum attackType, TypeEnum type, int baseDamage, int fullDamage, int combo, GameObject target)
    {
        attackQueue.Enqueue(new Attack(attackType, type, baseDamage, fullDamage, combo, target));
    }

    private void SpawnAttack()
    {
        var attack = attackQueue.Dequeue();

        if (attack.attackType == MatchEnum.Blob)
        {
            foreach(GameObject target in attack.targets)
            {
                GameObject basicAttack = Instantiate(BasicAttack, gameObject.transform.position, Quaternion.Euler(0, 0, -90), gameObject.transform);
                ExplodingProjectile expProj = basicAttack.GetComponent<ExplodingProjectile>();


                var color = TypeEnumGenerator.GetColor(attack.type);

                expProj.Init(attack.baseDamage, attack.fullDamage, color, attack.combo, target);
            }
            
        }
    }
}

public class Attack {
    public MatchEnum attackType;
    public TypeEnum type;
    public int baseDamage;
    public int fullDamage;
    public int combo;
    public List<GameObject> targets;
    public Attack(MatchEnum attackType, TypeEnum type, int baseDamage, int fullDamage, int combo, List<GameObject> targets)
    {
        this.attackType = attackType;
        this.type = type;
        this.baseDamage = baseDamage;
        this.fullDamage = fullDamage;
        this.combo = combo;
        this.targets = targets;
    }

    public Attack(MatchEnum attackType, TypeEnum type, int baseDamage, int fullDamage, int combo, GameObject target)
    {
        this.attackType = attackType;
        this.type = type;
        this.baseDamage = baseDamage;
        this.fullDamage = fullDamage;
        this.combo = combo;
        targets = new List<GameObject>() { target };
    }
}
