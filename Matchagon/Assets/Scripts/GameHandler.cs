using System.Collections;
using System.Collections.Generic;
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

    private CombatUIHandler CombatUIHandler;

    private Player Player;

    private float turnTimer;

    private Text TimertText;
    private Text ComboText;

    public int turnLimit;
    // Start is called before the first frame update
    void Start()
    {
        Turn = 1;
        Board = GameObject.Find("Board").GetComponent<Board>();
        state = 4;
        combo = new Combo();
        CombatUIHandler = GetComponent<CombatUIHandler>();

        Player = GameObject.Find("Player").GetComponent<Player>();

        TimertText = GameObject.Find("TimerText").GetComponent<Text>();
        ComboText = GameObject.Find("ComboText").GetComponent<Text>();
        turnTimer = turnLimit;
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
                DoDamage();
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
        }
        //if (DestroyState)
        //{
        //    t += Time.deltaTime;
        //    if(t > 1)
        //    {
        //        if (matches.Count == 0)
        //        {
        //            DestroyState = false;
        //            Board.CascadeBoard();
        //        }

        //        var match = matches[0];
        //        match.Spheres.ForEach(s => s.SetDestroy());
        //        matches.RemoveAt(0);
        //        t = 0;

                
        //    }
        //}

        // else if (Input.GetKeyDown(KeyCode.R)){
        //    Board.FillBoard();
        //}
    }

    public void EndMove()
    {
        matches = Board.IdentifyMatches();

        combo = new Combo();
        TimertText.text = "";
        state = 1;

        Player.EndTurn();
    }

    void DoDamage()
    {
        float i = 0;
        foreach (KeyValuePair<TypeEnum, int> entry in combo.damageDict)
        {
            int damage = entry.Value;
            Debug.Log(damage + " " + entry.Key.ToString() + " damage!");
            //CombatUIHandler.CreateDamageText(new Vector3(8 + i, 2), entry.Key, entry.Value * combo.count);
            //i += 1f;
            Player.GetComponent<CombatAnimations>().QueueAttack(MatchEnum.Blob, entry.Key, damage, combo.count);
        }
        state = 3;
    }

    public void StartMove()
    {
        turnTimer = turnLimit;
        state = 0;
    }
}
