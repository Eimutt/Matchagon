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
    EndOfTurn,
    max
    //Win
}


public class CombatHandler : MonoBehaviour
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

    public bool active;

    public Encounter Encounter;
    public EncounterInfo EncounterInfo;
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

        EncounterInfo = GameObject.Find("EncounterInfo").GetComponent<EncounterInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;
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
                if (t > 0.55f)
                {
                    if (matches.Count == 0)
                    {
                        t = 0;
                        if (Board.DestroySpheres())
                        {
                            t -= 0.5f;
                            return;
                        }
                        Board.CascadeBoard(); 
                        matches = Board.IdentifyMatches();
                        

                        if(matches.Count == 0)
                        {
                            AdvanceState();
                            return;
                        }
                    }

                    var match = matches[0];
                    match.Spheres.ForEach(s => s.TriggerMatch());
                    matches.RemoveAt(0);
                    t = 0;

                    if(match.ElementType == TypeEnum.Light)
                    {
                        Player.GrantTemporaryMana(1);
                    } else if (match.ElementType == TypeEnum.Dark)
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
            case GameState.EndOfTurn:
                OnTurnEnd();
                break;
        }
    }

    public GameState GetState()
    {
        return state;
    }

    public void EndMove()
    {
        Board.TriggerMovement();
        matches = Board.IdentifyMatches();

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
        //Debug.Log(state.ToString());
    }

    public void CheckIfWin()
    {
        if(EnemyHandler.NoEnemiesLeft() && Encounter.Waves.All(e => e.Turn <= Turn))
        {

            GameObject.Find("GameHandler").GetComponent<PlayerData>().UpdateHealth(Player.CurrentHp);


            Player.TriggerEndOfCombatItemEffects();

            GameObject.Find("GameHandler").GetComponent<GameHandler>().LeaveCombat();

        }
        //state = GameState.Win;
    }

    public void NextTurn()
    {
        state = 0;
    }

    public void TrySendWave()
    {
        var waves = Encounter.Waves.Where(e => e.Turn == Turn).ToList();

        if (waves.Count != 0)
        {
            foreach(Wave wave in waves)
            {

                for (int i = 0; i < wave.Quanitity; i++)
                {
                    EnemyHandler.AddEnemy(wave.Enemy);
                }
            }
        }

        EncounterInfo.CrossWave(Turn);
    }

    public void OnTurnStart()
    {
        CheckIfWin();
        Turn++;
        turnTimer = turnLimit;
        combo = new Combo();

        Player.NewTurn();
        TrySendWave();

        Board.TriggerSpecialBoardEffects();


        EnemyHandler.TriggerStartOfTurnEffects();

        AdvanceState();
    }

    public void OnTurnEnd()
    {
        Player.EndOfTurn();
        AdvanceState();
    }

    public void IncreaseTimeForNextTurn(int seconds)
    {
        turnTimer += seconds;
    }

    public void StartOfBattle()
    {

        Player.StartOfBattle();

        Board.FillBoard();
        active = true;
    }

    public void IncreaseCombo(int amount)
    {
        combo.count += amount;
        ComboText.text = combo.count.ToString();
    }
}
