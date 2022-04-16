using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    private Board Board;
    private int Turn;
    private bool DestroyState;
    private List<Match> matches;
    private float t = 1;

    private int state; //0 = play, 1 = resolve, 2 = damage, 3 = spawn

    private Combo combo;

    private Controls Controls;
    private Player Player;

    private float turnTimer;

    private Text TimertText;
    private Text ComboText;

    public int turnLimit;

    public List<Enemy> Enemies;
    // Start is called before the first frame update
    void Start()
    {
        Turn = 1;
        Board = GameObject.Find("Board").GetComponent<Board>();
        state = 5;
        combo = new Combo();

        Controls = GameObject.Find("Player").GetComponent<Controls>();
        Player = GameObject.Find("Player").GetComponent<Player>();

        TimertText = GameObject.Find("TimerText").GetComponent<Text>();
        ComboText = GameObject.Find("ComboText").GetComponent<Text>();
        turnTimer = turnLimit;

        Enemies = GameObject.FindObjectsOfType<Enemy>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                turnTimer -= Time.deltaTime;
                TimertText.text = turnTimer.ToString("n2");


                if (turnTimer <= 0)
                {
                    EndMove();
                }
                break;
            case 1:

                t += Time.deltaTime;
                if (t > 1)
                {
                    if (matches.Count == 0)
                    {
                        Board.CascadeBoard(); 
                        matches = Board.IdentifyMatches();
                        t = 0;

                        if(matches.Count == 0)
                        {
                            state = 2;
                            return;
                        }
                    }

                    var match = matches[0];
                    match.Spheres.ForEach(s => s.SetDestroy());
                    matches.RemoveAt(0);
                    t = 0;

                    combo.IncreaseDamageType(match.ElementType, match.Spheres.Count);

                    ComboText.text = combo.count.ToString();
                }

                break;
            case 2:
                ResolveTurn();
                break;

            case 3:

                t += Time.deltaTime;
                if (t > 1)
                {
                    Board.FillBoard();
                    state = 4;
                    t = 0;
                }

                break;
            case 4:
                t += Time.deltaTime;
                if (t > 1)
                {
                    PerformEnemyActions();
                    state = 5;
                    t = 0;
                }

                break;

        }
    }

    public void EndMove()
    {
        matches = Board.IdentifyMatches();

        combo = new Combo();
        TimertText.text = "";
        state = 1;

        Controls.EndTurn();
    }

    void ResolveTurn()
    {
        foreach (KeyValuePair<TypeEnum, int> entry in combo.damageDict)
        {
            int value = entry.Value;

            if(entry.Key == TypeEnum.Shield)
            {
                //Player.GetComponent<CombatAnimations>().QueueAttack(MatchEnum.Blob, entry.Key, value, combo.count);
                Player.GetShield(value * combo.count);
            } else
            {
                if(Enemies.Count == 0 || !Enemies.Any(x => !x.Dead))
                {
                    continue;
                }
                var enemy = Enemies.First(x => !x.Dead);
                enemy.AddIncomingDamage(value * combo.count);
                var targets = enemy.gameObject;
                Player.GetComponent<CombatAnimations>().QueueAttack(MatchEnum.Blob, entry.Key, value, combo.count, targets);
                //Enemies[0].TakeDamage(value * combo.count);

                //foreach (Enemy enemy in Enemies)
                //{
                //    enemy.TakeDamage(value * combo.count);
                //}
            }
            
        }
        state = 3;
    }

    public void StartMove()
    {
        turnTimer = turnLimit;
        state = 0;
    }

    public void PerformEnemyActions()
    {
        int x = Enemies.Count;

        int y = 0;
        int c = 0;
        while(c != x)
        {
            c++;
            if (Enemies[y].Dead)
            {
                Destroy(Enemies[y].gameObject);
                Enemies.RemoveAt(y);
            } else
            {
                Enemies[y].TakeTurn(Player, Enemies);
                y++;
            }
        }

        if(Enemies.Count == 0)
        {
            //Game won
            state = 99;
        }

        Player.ResetShield();
    }
}
