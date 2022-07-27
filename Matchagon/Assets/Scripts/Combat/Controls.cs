using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controls : MonoBehaviour
{
    private Board Board;
    public Sphere Selected;
    public Vector2Int Position;
    private int maxX;
    private int maxY;
    private GameObject Cursor;
    private CombatHandler CombatHandler;
    private Player Player;

    public int row;
    public int column;
    // Start is called before the first frame update
    void Start()
    {
        Board = GameObject.Find("Board").GetComponent<Board>();
        Cursor = GameObject.Find("Cursor");
        maxX = Board.x - 1;
        maxY = Board.y - 1;
        Position = new Vector2Int(0, 0);
        MoveCursor();
        CombatHandler = GameObject.Find("CombatHandler").GetComponent<CombatHandler>();
        Player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!Selected && CombatHandler.GetState() == GameState.UserSpells && Board.IsSelectable(Position.x, Position.y))
            {
                Selected = Board.GetSphere(Position);
                CombatHandler.AdvanceState();
            }
            else if (CombatHandler.GetState() == GameState.Moving)
            {
                CombatHandler.EndMove();
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (Board.IsMoveable(Position.x + 1, Position.y) && (Board.IsSelectable(Position.x + 1, Position.y) || !Selected))
            {
                MoveSphere(1, 0);
                Position += new Vector2Int(1, 0);
            }

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {

            if (Board.IsMoveable(Position.x - 1, Position.y) && (Board.IsSelectable(Position.x - 1, Position.y) || !Selected))
            {
                MoveSphere(-1, 0);
                Position += new Vector2Int(-1, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {

            if (Board.IsMoveable(Position.x, Position.y + 1) && (Board.IsSelectable(Position.x, Position.y + 1) || !Selected))
            {
                MoveSphere(0, 1);
                Position += new Vector2Int(0, 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {

            if (Board.IsMoveable(Position.x, Position.y - 1) && (Board.IsSelectable(Position.x, Position.y - 1) || !Selected))
            {
                MoveSphere(0, -1);
                Position += new Vector2Int(0, -1);
            }
        }

        if (CombatHandler.GetState() == GameState.UserSpells && Player.selectedCard == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Player.TryPlayCard(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //Player.PlayCard(1);
                Player.TryPlayCard(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Player.TryPlayCard(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Player.TryPlayCard(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Player.TryPlayCard(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Player.TryPlayCard(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                Player.TryPlayCard(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                Player.TryPlayCard(7);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                Player.TryPlayCard(8);
            }

        }
        else if (CombatHandler.GetState() == GameState.UserSpells && Player.selectedCard != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Player.AddMinion(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Player.AddMinion(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Player.AddMinion(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Player.AddMinion(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Player.AddMinion(5);
            } else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Player.DeselectMinionCard();
            } 

        }

        if (Input.GetKeyDown("left"))
        {
            Board.MoveRow(row, false);
        }
        if (Input.GetKeyDown("right"))
        {
            Board.MoveRow(row, true);
        }
        if (Input.GetKeyDown("up"))
        {
            Board.MoveColumn(column, true);
        }
        if (Input.GetKeyDown("down"))
        {
            Board.MoveColumn(column, false);
        }

        MoveCursor();
    }

    private void MoveCursor()
    {
        Cursor.transform.position = new Vector3(Position.x, Position.y, 0);
    }

    public void MoveSphere(int deltaX, int deltaY)
    {
        if (Selected)
        {
            Board.SwitchSpheres(Position.x, Position.x + deltaX, Position.y, Position.y + deltaY);
        }
    }

    public void EndTurn()
    {
        Selected = null;
    }
}