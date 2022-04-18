using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    private List<Enemy> Enemies;
    private GameHandler GameHandler;

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
        positions = new List<Vector3>();
        Enemies = new List<Enemy>();// GameObject.FindObjectsOfType<Enemy>().ToList();
        GameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();

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
            //GameHandler.AdvanceState();
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
    public List<Enemy> GetAllEnemies()
    {
        return Enemies;
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
            if(!Enemies.Any(e => e.gameObject.transform.position == position))
            {
                positionFound = true;
            }

            i++;
            if(i >= positions.Count)
            {
                Debug.Log("no room for enemy");
                return;
            }
        }


        GameObject enemyObject = Instantiate(enemyGameObject, position, Quaternion.identity);
        Enemy enemy = enemyObject.GetComponent<Enemy>();

        Enemies.Add(enemy);
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
}
