﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    private Board Board;
    private int Turn;
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

    public EnemyHandler EnemyHandler;
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

        EnemyHandler = GetComponent<EnemyHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                turnTimer -= Time.deltaTime;
                TimertText.text = turnTimer.ToString("n1");


                if (turnTimer <= 0)
                {
                    EndMove();
                }
                break;
            case 1:

                t += Time.deltaTime;
                if (t > 0.5f)
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
                    EnemyHandler.PerformEnemyActions(Player);
                    state = 5;
                    t = 0;
                }

                break;
            case 5:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Board.TransformSpheres(TypeEnum.Fire, TypeEnum.Grass);
                } else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Board.TransformSpheres(TypeEnum.Grass, TypeEnum.Water);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    Board.TransformSpheres(TypeEnum.Water, TypeEnum.Fire);
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
                Player.GetShield(value * combo.count);
            } 
            else
            {
                if(EnemyHandler.EnemiesLeft())
                {
                    continue;
                }
                var enemy = EnemyHandler.GetFirstEnemy();
                enemy.AddIncomingDamage(value * combo.count);
                var targets = enemy.gameObject;
                Player.GetComponent<CombatAnimations>().QueueAttack(MatchEnum.Blob, entry.Key, value, combo.count, targets);
                
            }
            
        }
        state = 3;
    }

    public void StartMove()
    {
        turnTimer = turnLimit;
        state = 0;
        Player.ResetShield();
    }

    public void AdvanceState()
    {
        state++;
    }

    public void Win()
    {
        state = 99;
    }
}
