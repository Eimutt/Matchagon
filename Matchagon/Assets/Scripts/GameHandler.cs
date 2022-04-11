using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        Turn = 1;
        Board = GameObject.Find("Board").GetComponent<Board>();
        state = 0;
        combo = new Combo();
        CombatUIHandler = GetComponent<CombatUIHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    EndTurn();
                    combo = new Combo();
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
                    state = 0;
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

    void EndTurn()
    {
        matches = Board.IdentifyMatches();

        state = 1;
    }

    void DoDamage()
    {
        float i = 0;
        foreach (KeyValuePair<TypeEnum, int> entry in combo.damageDict)
        {
            Debug.Log(entry.Value * combo.count + " " + entry.Key.ToString() + " damage!");
            CombatUIHandler.CreateDamageText(new Vector3(8 + i, 2), entry.Key, entry.Value * combo.count);
            i += 1f;
        }
        state = 3;
    }
}
