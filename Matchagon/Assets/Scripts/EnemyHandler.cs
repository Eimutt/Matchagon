using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public List<Enemy> Enemies;
    private GameHandler GameHandler;

    private int eNum;
    private int y;
    private int c;
    private float t;
    private float waitTime;

    private Player Player;
    private bool active;
    // Start is called before the first frame update
    void Start()
    {

        Enemies = GameObject.FindObjectsOfType<Enemy>().ToList();
        GameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
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
                    Destroy(Enemies[y].gameObject);
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
            GameHandler.AdvanceState();
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
        return Enemies.First(x => !x.Dead);
    }

    public bool EnemiesLeft()
    {
        return (Enemies.Count == 0 || !Enemies.Any(x => !x.Dead));
    }
}
