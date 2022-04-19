using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    StartOfTurn,
    UserSpells,
    Moving,
    Cascade,
    Resolve,
    FillBoard,
    EnemyTurn,
    EnemyActions,
    max
    //Win
}


public class GameHandler : MonoBehaviour
{
    private Board Board;
    private int Turn;
    private List<Match> matches;
    private float t = 1;

    public GameState state; //0 = play, 1 = resolve, 2 = damage, 3 = spawn

    private Combo combo;

    private Controls Controls;
    private Player Player;

    private float turnTimer;

    private Text TimertText;
    private Text ComboText;

    public int turnLimit;

    public EnemyHandler EnemyHandler;

    public Encounter Encounter;
    // Start is called before the first frame update
    void Start()
    {
        Turn = 0;
        Board = GameObject.Find("Board").GetComponent<Board>();
        state = GameState.StartOfTurn;
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
            case GameState.StartOfTurn:
                OnTurnStart();

                break;
            case GameState.UserSpells:
                
                //don't do anything
                //read inputs from contorls
                break;

            case GameState.Moving:
                turnTimer -= Time.deltaTime;
                TimertText.text = turnTimer.ToString("n1");


                if (turnTimer <= 0)
                {
                    EndMove();
                }
                break;
            case GameState.Cascade:

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
                            AdvanceState();
                            return;
                        }
                    }

                    var match = matches[0];
                    match.Spheres.ForEach(s => s.SetDestroy());
                    matches.RemoveAt(0);
                    t = 0;

                    if(match.ElementType == TypeEnum.Grass)
                    {
                        Player.DrawCard();
                    }

                    combo.IncreaseDamageType(match.ElementType, match.Spheres.Count);

                    ComboText.text = combo.count.ToString();
                }

                break;
            case GameState.Resolve:
                ResolveTurn();
                AdvanceState();
                break;

            case GameState.FillBoard:

                t += Time.deltaTime;
                if (t > 1)
                {
                    Board.FillBoard();
                    AdvanceState();
                    t = 0;
                }

                break;
            case GameState.EnemyTurn:
                t += Time.deltaTime;
                if (t > 1)
                {
                    EnemyHandler.PerformEnemyActions(Player);
                    AdvanceState();
                    t = 0;
                }

                break;
            case GameState.EnemyActions:
                //if (EnemyHandler.NoEnemiesLeft())
                //    AdvanceState();
                break;
        }
    }

    public GameState GetState()
    {
        return state;
    }

    public void EndMove()
    {
        matches = Board.IdentifyMatches();

        combo = new Combo();
        TimertText.text = "";
        AdvanceState();

        Controls.EndTurn();
    }

    void ResolveTurn()
    {
        Player.ResolveTurn(combo, EnemyHandler);
        
    }

    public void StartMove()
    {
        turnTimer = turnLimit;
        state = 0;
        Player.ResetShield();
    }

    public void AdvanceState()
    {
        state = (GameState)((((int)state) + 1) % (int)GameState.max);
        Debug.Log(state.ToString());
    }

    public void Win()
    {
        //state = GameState.Win;
    }

    public void NextTurn()
    {
        state = 0;
    }

    public void TrySendWave()
    {
        var wave = Encounter.Waves.FirstOrDefault(e => e.Turn == Turn);

        if (wave != null)
        {
            for(int i = 0; i < wave.Quanitity; i++)
            {
                EnemyHandler.AddEnemy(wave.Enemy);
            }
        }
    }

    public void OnTurnStart()
    {
        Turn++;
        turnTimer = turnLimit;
        Player.NewTurn();
        TrySendWave();
        AdvanceState();
    }
}
