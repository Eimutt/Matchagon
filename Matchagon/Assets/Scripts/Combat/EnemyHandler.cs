using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    private List<Enemy> Enemies;
    private CombatHandler CombatHandler;

    private Enemy FocusedEnemy;
    private GameObject Focus;

    private int eNum;
    private int y;
    private int c;
    private float t;
    private float waitTime;

    private Player Player;
    private bool active;

    private List<Vector3> positions;

    public Vector3 bottomLeft;
    public float distanceDiff;

    // Start is called before the first frame update
    void Start()
    {
        Focus = GameObject.Find("Focus");
        Focus.SetActive(false);

        positions = new List<Vector3>();
        Enemies = new List<Enemy>();// GameObject.FindObjectsOfType<Enemy>().ToList();
        CombatHandler = GameObject.Find("CombatHandler").GetComponent<CombatHandler>();

        GetLegitimateSpawnPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (c != eNum && active)
        {
            t += Time.deltaTime;
            if (t > waitTime)
            {
                t = 0;


                c++;
                if (Enemies[y].Dead)
                {
                    Enemies[y].Effects.Where(e => e.EffectType == EffectType.OnDeath).ToList().ForEach(e => e.Trigger(Enemies[y].GetInstanceID()));
                    Destroy(Enemies[y].gameObject);
                    if (Enemies[y] == FocusedEnemy)
                        FocusedEnemy = null;
                        Focus.SetActive(false);
                    Enemies.RemoveAt(y);

                }
                else
                {
                    waitTime = Enemies[y].TakeTurn(Player, Enemies);
                    y++;
                }
            }
        }

        if (c == eNum && active)
        {
            CombatHandler.AdvanceState();
            active = false;
        }
    }

    public void PerformEnemyActions(Player player)
    {
        Player = player;
        eNum = Enemies.Count;
        active = true;
        y = 0;
        c = 0;
    }

    public Enemy GetFirstEnemy()
    {
        if (FocusedEnemy != null && !FocusedEnemy.Dead)
            return FocusedEnemy;

        return Enemies.First(x => !x.Dead);
    }
    public List<Enemy> GetAllEnemies()
    {
        return Enemies;
    }

    public void FocusEnemy(Enemy enemy)
    {
        FocusedEnemy = enemy;
        Focus.SetActive(false);
        Focus.SetActive(true);
        Focus.transform.position = enemy.transform.position;
    }

    public bool NoEnemiesLeft()
    {
        var noEnemies = Enemies.Count == 0 || !Enemies.Any(x => !x.Dead);
        //if (noEnemies)
        //    active = false;

        return noEnemies;
    }

    public void AddEnemy(GameObject enemyGameObject)
    {
        bool positionFound = false;

        Vector3 position = Vector3.zero;

        int i = 0;
        while (!positionFound)
        {
            position = positions[i];
            if(!Enemies.Any(e => e.startPos == position))
            {
                positionFound = true;
            }

            i++;
            if(i > positions.Count)
            {
                Debug.Log("no room for enemy");
                return;
            }
        }


        GameObject enemyObject = Instantiate(enemyGameObject, position, Quaternion.identity);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        enemy.startPos = position;

        Enemies.Add(enemy);

        enemy.Effects.Where(e => e.EffectType == EffectType.SummonEffect).ToList().ForEach(e => e.Trigger(enemy.GetInstanceID()));
    }

    private void GetLegitimateSpawnPositions()
    {
        for(int i = 0; i < 2; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                positions.Add(bottomLeft + new Vector3(i * distanceDiff, j * distanceDiff, 0));
            }
        }
    }

    public void TriggerStartOfTurnEffects()
    {
        foreach(Enemy enemy in Enemies)
        {
            enemy.Effects.Where(e => e.EffectType == EffectType.StartOfTurn).ToList().ForEach(e => e.Trigger(enemy.GetInstanceID()));
        }
    }
}
