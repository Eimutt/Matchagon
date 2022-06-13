using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    public int length;
    public int maxOptions;
    public int minOptions;

    public int pathParts;

    public MapNode currentLevel;

    public List<MapNode> mapNodes;

    public List<GameObject> NodeObjectPrefabs;
    public List<GameObject> EncounterPrefabs;
    public List<GameObject> BossEncounterPrefabs;
    public GameObject NodeBase;
    public GameObject Path;

    public Vector2 start;
    public float randomOffset;

    public GameObject playerCharacterPrefab;
    private GameObject playerCharacterSprite;

    private bool canMove = true;

    public bool Fog;

    private int Difficulty;
    public int DifficultyIncreaseFrequency;
    // Start is called before the first frame update
    void Start()
    {
        mapNodes = new List<MapNode>();
        generateWorld();

        playerCharacterSprite = Instantiate(playerCharacterPrefab, transform);

        MoveToStartNode(mapNodes[0]);

        AddFog();
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

        var prefabs = EncounterPrefabs.Union(NodeObjectPrefabs).ToList();


        for (int i = 1; i < length; i++)
        {
            while (last == options)
            {
                options = Random.Range(minOptions, maxOptions);
            }

            int extra = 0;

            bool encounter = (i % 1 == 0);
            for (int j = 1; j <= options; j++)
            {
                if (encounter)
                {
                    int randomEncounter = Random.Range(0, prefabs.Count);
                    var node = Instantiate(prefabs[randomEncounter], transform);

                    node.name = i + " " + j;

                    float x = Random.Range(-randomOffset, randomOffset);
                    float y = Random.Range(-randomOffset, randomOffset);

                    node.transform.localPosition = posAlongLine(j * (1f / ((float)options + 1)), i, 1, 2) + new Vector3(x, y, 0);

                    var mapNode = node.GetComponent<MapNode>();
                    mapNode.level = i;
                    mapNode.order = (options / 2) - j + 1 + extra;

                    if (mapNode.order == 0)
                    {
                        if(options % 2 == 0)
                            extra = -1;
                        mapNode.order = (options / 2) - j + 1 + extra;
                    }
                        

                    mapNodes.Add(mapNode);

                    var list = CalculateNodes(mapNode, options, last);

                    mapNode.reachableFrom.AddRange(list);

                    //create path objects

                    foreach (var child in mapNode.reachableFrom)
                    {
                        Vector3 p1 = child.transform.localPosition;
                        Vector3 p2 = mapNode.transform.localPosition;

                        for (int k = 1; k < pathParts; k++)
                        {
                            var pathPart = Instantiate(Path, transform);
                            pathPart.transform.localPosition = p1 * (float)k/(float)pathParts + p2 * (1-(float)k/(float)pathParts);
                        }
                    }
                    if(mapNode.GetType() == typeof(EncounterNode))
                    {
                        mapNode.GetComponent<EncounterNode>().Difficulty = Difficulty;
                    }
                }
                else
                {
                    int randomEncounter = Random.Range(0, NodeObjectPrefabs.Count);
                    var node = Instantiate(NodeObjectPrefabs[randomEncounter], transform);

                    node.name = i + " " + j;

                    float x = Random.Range(-randomOffset, randomOffset);
                    float y = Random.Range(-randomOffset, randomOffset);

                    node.transform.localPosition = posAlongLine(j * (1f / ((float)options + 1)), i, 1, 2) + new Vector3(x, y, 0);

                    var mapNode = node.GetComponent<MapNode>();
                    mapNode.level = i;
                    mapNode.order = (options / 2) - j + 1 + extra;

                    if (mapNode.order == 0)
                    {
                        if (options % 2 == 0)
                            extra = -1;
                        mapNode.order = (options / 2) - j + 1 + extra;
                    }


                    mapNodes.Add(mapNode);

                    var list = CalculateNodes(mapNode, options, last);

                    mapNode.reachableFrom.AddRange(list);

                    //create path objects

                    foreach (var child in mapNode.reachableFrom)
                    {
                        Vector3 p1 = child.transform.localPosition;
                        Vector3 p2 = mapNode.transform.localPosition;

                        for (int k = 1; k < pathParts; k++)
                        {
                            var pathPart = Instantiate(Path, transform);
                            pathPart.transform.localPosition = p1 * (float)k / (float)pathParts + p2 * (1 - (float)k / (float)pathParts);
                        }
                    }
                }
            }



            last = options;

            if (i % DifficultyIncreaseFrequency == 0){
                Difficulty++;
            }
        }

        //remove unconnected
        //var unused = mapNodes.Where(n => n.DeadEnd || n.reachableFrom.Count == 0).ToList();

        //unused.ForEach(n =>
        //{
        //    mapNodes.Remove(n);
        //    Destroy(n.gameObject);
        //});

        //generate boss node
        BossNode();
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

    private List<MapNode> CalculateNodes(MapNode mapNode, int options, int last)
    {
        var nodes = new List<MapNode>();

        if(last == 1)
        {
            return mapNodes.Where(n => (n.level == (mapNode.level - 1))).ToList();
        }

        nodes = mapNodes.Where(n => (n.level == (mapNode.level - 1) && (System.Math.Abs(n.order - mapNode.order) < 2))).ToList();

        //nodes = nodes.Where(x => Random.Range(0, 1f) > 0.25f).ToList();
        //nodes.ForEach(n => n.DeadEnd = false);

        return nodes;
    }

    void BossNode()
    {
        int i = length;
        int j = 1;
        int randomEncounter = Random.Range(0, BossEncounterPrefabs.Count);
        var node = Instantiate(BossEncounterPrefabs[randomEncounter], transform);

        node.name = "boss";

        float x = Random.Range(-randomOffset, randomOffset);
        float y = Random.Range(-randomOffset, randomOffset);

        node.transform.localPosition = posAlongLine(j * (1f / ((float)1 + 1)), i, 1, 2) + new Vector3(x, y, 0);

        var mapNode = node.GetComponent<MapNode>();
        mapNode.level = i;

        mapNodes.Add(mapNode);

        var list = CalculateNodes(mapNode, 0, 1);

        mapNode.reachableFrom.AddRange(list);

        //create path objects

        foreach (var child in mapNode.reachableFrom)
        {
            Vector3 p1 = child.transform.localPosition;
            Vector3 p2 = mapNode.transform.localPosition;

            for (int k = 1; k < pathParts; k++)
            {
                var pathPart = Instantiate(Path, transform);
                pathPart.transform.localPosition = p1 * (float)k / (float)pathParts + p2 * (1 - (float)k / (float)pathParts);
            }

        }
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

            var moveScript = playerCharacterSprite.AddComponent<Move>();
            moveScript.Init(nextNode.transform.position + new Vector3(0, 0.25f, 0), 1, nextNode.ActivateNode);

            canMove = false;
        }

    }

    public void ActivateMovement()
    {
        canMove = true;
        Destroy(currentLevel.transform.Find("NodeSprite").gameObject);
    }

    public void SetMove(bool movement)
    {
        canMove = movement;
    }

    public void AddFog()
    {
        if (Fog)
        {
            GameObject.Find("Fog").AddComponent<FogHandler>();
        }
    }
}
