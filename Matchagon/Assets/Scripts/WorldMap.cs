using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    public int length;
    public int maxOptions;
    public int minOptions;
    public MapNode currentLevel;

    public List<MapNode> mapNodes;

    public List<GameObject> NodeObjectPrefabs;
    public List<GameObject> EncounterPrefabs;
    public GameObject NodeBase;
    public GameObject Path;

    public Vector2 start;
    public float randomOffset;

    public GameObject playerCharacterPrefab;
    private GameObject playerCharacterSprite;

    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        mapNodes = new List<MapNode>();
        generateWorld();

        playerCharacterSprite = Instantiate(playerCharacterPrefab, transform);

        MoveToStartNode(mapNodes[0]);
    }

    // Update is called once per frame
    void Update()
    {
        start = new Vector2(0, 0);
    }

    void generateWorld()
    {
        //generate start node
        StartNode();

        int last = 1;
        int options = 1;
        for (int i = 1; i < length; i++)
        {
            while (last == options)
            {
                options = Random.Range(minOptions, maxOptions);
            }

            bool encounter = (i % 1 == 0);
            for (int j = 1; j <= options; j++)
            {

                if (encounter)
                {
                    int randomEncounter = Random.Range(0, EncounterPrefabs.Count);
                    var node = Instantiate(EncounterPrefabs[randomEncounter], transform);

                    node.name = i + " " + j;

                    float x = Random.Range(-randomOffset, randomOffset);
                    float y = Random.Range(-randomOffset, randomOffset);

                    node.transform.localPosition = posAlongLine(j * (1f / ((float)options + 1)), i, 1, 2) + new Vector3(x, y, 0);

                    var mapNode = node.GetComponent<MapNode>();
                    mapNode.level = i;
                    mapNodes.Add(mapNode);

                    var list = mapNodes.Where(n => (n.level == (mapNode.level - 1))).ToList();

                    mapNode.reachableFrom.AddRange(list);

                    //create path objects

                    foreach (var child in mapNode.reachableFrom)
                    {
                        Vector3 p1 = child.transform.localPosition;
                        Vector3 p2 = mapNode.transform.localPosition;

                        var pathPart1 = Instantiate(Path, transform);
                        var pathPart2 = Instantiate(Path, transform);

                        pathPart1.transform.localPosition = p1 * 0.33f + p2 * 0.66f;
                        pathPart2.transform.localPosition = p1 * 0.66f + p2 * 0.33f;

                    }
                }
                else
                {
                    var node = Instantiate(NodeBase, transform);

                    node.name = i + " " + j;

                    float x = Random.Range(-randomOffset, randomOffset);
                    float y = Random.Range(-randomOffset, randomOffset);


                    node.transform.localPosition = posAlongLine(j * (1f / ((float)options + 1)), i, 1, 2) + new Vector3(x, y, 0);

                    var mapNode = node.AddComponent<MapNode>();
                    mapNode.level = i;
                    mapNodes.Add(mapNode);

                    var list = mapNodes.Where(n => (n.level == (mapNode.level - 1))).ToList();

                    mapNode.reachableFrom.AddRange(list);

                    //create path objects

                    foreach (var child in mapNode.reachableFrom)
                    {
                        Vector3 p1 = child.transform.position;
                        Vector3 p2 = mapNode.transform.position;

                        var pathPart1 = Instantiate(Path, transform);
                        var pathPart2 = Instantiate(Path, transform);

                        pathPart1.transform.localPosition = p1 * 0.33f + p2 * 0.66f;
                        pathPart2.transform.localPosition = p1 * 0.66f + p2 * 0.33f;

                    }
                }
            }



            last = options;

        }
        //generate boss node
    }

    void StartNode()
    {
        var startNode = Instantiate(NodeBase, transform);

        //set pos
        startNode.transform.localPosition = new Vector3(start.x, start.y, 0);


        var s = startNode.AddComponent<MapNode>();
        s.level = 0;
        mapNodes.Add(s);

    }

    void BossNode()
    {

    }


    Vector3 posAlongLine(float t, float i, float yDif, float xDif)
    {
        Vector3 pos = new Vector3();

        Vector3 lineStart = new Vector3(start.x - xDif + (i * xDif), start.y + yDif + (i * yDif), 0);
        Vector3 lineEnd = new Vector3(start.x + xDif + (i * xDif), start.y - yDif + (i * yDif), 0);

        pos.x = lineStart.x * t + lineEnd.x * (1 - t);

        pos.y = lineStart.y * t + lineEnd.y * (1 - t);

        return pos;

    }
    public void MoveToStartNode(MapNode nextNode)
    {
        currentLevel = nextNode;
        playerCharacterSprite.transform.position = nextNode.transform.position;
    }

    public void MoveTo(MapNode nextNode)
    {
        if (canMove)
        {
            currentLevel = nextNode;
            playerCharacterSprite.transform.position = nextNode.transform.position;

            canMove = false;
        }

    }

    public void ActivateMovement()
    {
        canMove = true;
    }
}
