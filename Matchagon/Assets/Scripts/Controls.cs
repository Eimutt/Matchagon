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
    private GameHandler gameHandler;
    private Player Player;

    private List<Card> Cards;
    // Start is called before the first frame update
    void Start()
    {
        Board = GameObject.Find("Board").GetComponent<Board>();
        Cursor = GameObject.Find("Cursor");
        maxX = Board.x - 1;
        maxY = Board.y - 1;
        Position = new Vector2Int(0, 0);
        MoveCursor();
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        Player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!Selected && gameHandler.GetState() == GameState.UserSpells)
            {
                Selected = Board.GetSphere(Position);
                gameHandler.AdvanceState();
            }
            else if (gameHandler.GetState() == GameState.Moving)
            {
                gameHandler.EndMove();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Position.x < maxX)
            {
                MoveSphere(1, 0);
                Position += new Vector2Int(1, 0);
            }

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            if (Position.x > 0)
            {
                MoveSphere(-1, 0);
                Position += new Vector2Int(-1, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            if (Position.y < maxY)
            {
                MoveSphere(0, 1);
                Position += new Vector2Int(0, 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            if (Position.y > 0)
            {
                MoveSphere(0, -1);
                Position += new Vector2Int(0, -1);
            }
        }

        if(gameHandler.GetState() == GameState.UserSpells)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Player.PlayCard(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Player.PlayCard(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Player.PlayCard(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Player.PlayCard(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Player.PlayCard(4);
            }
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