using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Sphere[][] Spheres;
    public int x;
    public int y;
    public GameObject SphereObject;

    public SphereGenerator SphereGenerator;

    public GameObject HiddenFogPrefab;
    public GameObject FrozenPrefab;
    public GameObject MovementPrefab;
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        Spheres = Enumerable
           .Range(0, x)
           .Select(i => new Sphere[y])
           .ToArray();
        SphereGenerator = gameObject.GetComponent<SphereGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sphere GetSphere(Vector2Int position)
    {
        return Spheres[position.x][position.y];
    }

    public void FillBoard()
    {
        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                if(Spheres[i][j] == null)
                {
                    Spheres[i][j] = SphereGenerator.GenerateRandomSphere(i, j);
                }
            }
        }
    }

    public void SwitchSpheres(int x1, int x2, int y1, int y2)
    {
        var Sphere1 = Spheres[x1][y1];
        var Sphere2 = Spheres[x2][y2];

        Spheres[x1][y1] = Sphere2;
        Spheres[x2][y2] = Sphere1;

        var move1 = Spheres[x1][y1].gameObject.AddComponent<Move>();
        move1.Init(new Vector3(x1, y1), 0.1f);
        var move2 = Spheres[x2][y2].gameObject.AddComponent<Move>();
        move2.Init(new Vector3(x2, y2), 0.1f);

    }

    public List<Match> IdentifyMatches()
    {
        var matches = new List<Match>();

        //IdentifyRows(matches);
        //Identify5(matches);
        //Identify4(matches);
        //Identify3(matches);
        var Regions = IdentifyRegions();

        foreach(Region region in Regions)
        {
            if (region.nodes.Count < 3) continue;

            var match = new Match(region.Type);

            region.nodes.ForEach(n => 
            {
                if (region.PartOfDestroy(n))
                {
                    match.Spheres.Add(Spheres[n.x][n.y]);
                }
            });

            if(match.Spheres.Count != 0)
                matches.Add(match);
        }

        return matches;
    }

    public List<Region> IdentifyRegions()
    {
        var regions = new List<Region>();

        bool[,] partOfRegion = new bool[x,y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (partOfRegion[i, j] || Spheres[i][j] == null) continue;

                var type = Spheres[i][j].GetType();
                if (type == TypeEnum.Chromatic)
                {

                    //Region regionUp = new Region(type);
                    //regionUp.nodes.Add(new Vector2Int(i, j));

                    //Stack<Vector2Int> stack = new Stack<Vector2Int>();
                    //if (i + 1 < x && Spheres[i + 1, j] != null)
                    //{
                    //    stack.Push(new Vector2Int(i + 1, j));
                    //}


                    //Region regionRight = new Region(type);
                    //region.nodes.Add(new Vector2Int(i, j));
                    continue;
                    partOfRegion[i,j] = true;

                } 
                else
                {
                    Region region = new Region(type);
                    region.nodes.Add(new Vector2Int(i, j));

                    Stack<Vector2Int> stack = new Stack<Vector2Int>();

                    if (i + 1 < x && Spheres[i + 1][j] != null)
                    {
                        stack.Push(new Vector2Int(i + 1, j));
                    }
                    if (j + 1 < y && Spheres[i][j + 1] != null)
                    {
                        stack.Push(new Vector2Int(i, j + 1));
                    }

                    if (i - 1 >= 0 && Spheres[i - 1][j] != null && Spheres[i -1][j].GetType() == TypeEnum.Chromatic)
                    {
                        stack.Push(new Vector2Int(i - 1, j));
                    }
                    if (j - 1 >= 0 && Spheres[i][j - 1]  && Spheres[i][j - 1].GetType() == TypeEnum.Chromatic)
                    {
                        stack.Push(new Vector2Int(i, j - 1));
                    }

                    while (stack.Count != 0)
                    {
                        var nextPos = stack.Pop();

                        if (Spheres[nextPos.x][nextPos.y] != null && (Spheres[nextPos.x][nextPos.y].GetType() == type || Spheres[nextPos.x][nextPos.y].GetType() == TypeEnum.Chromatic))
                        {
                            if (Spheres[nextPos.x][nextPos.y].GetType() == TypeEnum.Chromatic)
                            {
                                partOfRegion[nextPos.x, nextPos.y] = false;
                            }
                            else
                            {
                                partOfRegion[nextPos.x, nextPos.y] = true;
                            }
                            region.nodes.Add(new Vector2Int(nextPos.x, nextPos.y));
                            if (nextPos.x + 1 < x && !partOfRegion[nextPos.x + 1, nextPos.y])
                            {
                                if (!region.nodes.Contains(new Vector2Int(nextPos.x + 1, nextPos.y)) && !stack.Contains(new Vector2Int(nextPos.x + 1, nextPos.y)))
                                    stack.Push(new Vector2Int(nextPos.x + 1, nextPos.y));
                            }
                            if (nextPos.y + 1 < y && !partOfRegion[nextPos.x, nextPos.y + 1])
                            {
                                if (!region.nodes.Contains(new Vector2Int(nextPos.x, nextPos.y + 1)) && !stack.Contains(new Vector2Int(nextPos.x, nextPos.y + 1)))
                                   stack.Push(new Vector2Int(nextPos.x, nextPos.y + 1));
                            }
                            if (nextPos.x > 0 && !partOfRegion[nextPos.x - 1, nextPos.y])
                            {
                                if (!region.nodes.Contains(new Vector2Int(nextPos.x - 1, nextPos.y)) && !stack.Contains(new Vector2Int(nextPos.x - 1, nextPos.y)))
                                   stack.Push(new Vector2Int(nextPos.x - 1, nextPos.y));
                            }
                            if (nextPos.y > 0 && !partOfRegion[nextPos.x, nextPos.y - 1])
                            {
                                if (!region.nodes.Contains(new Vector2Int(nextPos.x, nextPos.y - 1)) && !stack.Contains(new Vector2Int(nextPos.x, nextPos.y - 1)))
                                   stack.Push(new Vector2Int(nextPos.x, nextPos.y - 1));
                            }
                        }
                    }
                    regions.Add(region);
                }


                
            }
        }
        return regions;
    }



    //private void IdentifyRows(List<Match> matches)
    //{
    //    for(int i = 0; i < y; i++)
    //    {
    //        var type = Spheres[0, i].GetType();

    //        var SphereRow = GetRow(Spheres, i, 0, 6);

    //        if (SphereRow.TrueForAll(s => s.GetType() == type))
    //        {
    //            var match = new Match(MatchEnum.Row, SphereRow);

    //            SphereRow.ForEach(s => s.SetDestroy());

    //            matches.Add(match);
    //        }
    //    }
    //}

    //private void Identify5(List<Match> matches)
    //{
    //    for (int i = 0; i < x; i++)
    //    {
    //        var type = Spheres[i, 0].GetType();

    //        var SphereColumn = GetColumn(Spheres, i, 0, 5);

    //        if (SphereColumn.TrueForAll(s => s.GetType() == type))
    //        {
    //            bool isolated = !SphereColumn.Any(s => s.Destroy);
    //            var match = new Match(MatchEnum.Row, SphereColumn, isolated);

    //            SphereColumn.ForEach(s => s.SetDestroy());

    //            matches.Add(match);
    //        }
    //    }

    //    for (int i = 0; i < x - 4; i++)
    //    {
    //        for (int j = 0; j < y; j++)
    //        {
    //            var type = Spheres[i, j].GetType();

    //            var SphereRow = GetRow(Spheres, j, i, 5);

    //            if (SphereRow.TrueForAll(s => s.GetType() == type))
    //            {
    //                bool isolated = !SphereColumn.Any(s => s.Destroy);
    //                var match = new Match(MatchEnum.Row, SphereRow, isolated);

    //                SphereRow.ForEach(s => s.SetDestroy());

    //                matches.Add(match);
    //            }
    //        }
    //    }
    //}

    //private void Identify4(List<Match> matches)
    //{
    //    for (int i = 0; i < x - 3; i++)
    //    {
    //        for (int j = 0; j < y; j++)
    //        {
    //            var type = Spheres[i, j].GetType();

    //            var SphereRow = GetColumn(Spheres, i, j, 4);

    //            if (SphereRow.TrueForAll(s => s.GetType() == type))
    //            {
    //                var match = new Match(MatchEnum.Row, SphereRow);

    //                SphereRow.ForEach(s => s.SetDestroy());

    //                matches.Add(match);
    //            }
    //        }
    //    }

    //    for (int i = 0; i < x; i++)
    //    {
    //        for (int j = 0; j < y - 4; j++)
    //        {
    //            var type = Spheres[i, j].GetType();

    //            var SphereColumn = GetRow(Spheres, j, i, 4);

    //            if (SphereColumn.TrueForAll(s => s.GetType() == type))
    //            {
    //                var match = new Match(MatchEnum.Row, SphereColumn);

    //                SphereColumn.ForEach(s => s.SetDestroy());

    //                matches.Add(match);
    //            }
    //        }
    //    }
    //}

    //private void Identify3(List<Match> matches)
    //{
    //    for (int i = 0; i < x - 2; i++)
    //    {
    //        for (int j = 0; j < y; j++)
    //        {
    //            var type = Spheres[i, j].GetType();

    //            var SphereRow = GetRow(Spheres, j, i, 3);

    //            if (SphereRow.TrueForAll(s => s.GetType() == type))
    //            {
    //                bool isolated = !SphereRow.Any(s => s.Destroy);
    //                var match = new Match(MatchEnum.Row, SphereRow, isolated);

    //                SphereRow.ForEach(s => s.SetDestroy());

    //                matches.Add(match);
    //            }
    //        }
    //    }

    //    for (int i = 0; i < x; i++)
    //    {
    //        for (int j = 0; j < y - 2; j++)
    //        {
    //            var type = Spheres[i, j].GetType();

    //            var SphereColumn = GetColumn(Spheres, i, j, 3);

    //            if (SphereColumn.TrueForAll(s => s.GetType() == type))
    //            {
    //                bool isolated = !SphereColumn.Any(s => s.Destroy);
    //                var match = new Match(MatchEnum.Row, SphereColumn, isolated);

    //                SphereColumn.ForEach(s => s.SetDestroy());

    //                matches.Add(match);
    //            }
    //        }
    //    }
    //}

    public bool IsMoveable(int x1, int y1)
    {
        return (x1 >= 0 && x1 < x && y1 >= 0 && y1 < y);
    }

    public bool IsSelectable(int x1, int y1)
    {
        return !Spheres[x1][y1].Frozen;
    }

    public bool DestroySpheres()
    {
        bool destroyed = false;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (Spheres[i][j] != null && Spheres[i][j].destroy)
                {
                    Spheres[i][j].SetFadeOut();
                    //Destroy(Spheres[i,j].gameObject);
                    Spheres[i][j] = null;
                    destroyed = true;
                }
            }
        }
        return destroyed;
    }

    public void CascadeBoard()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (Spheres[i][j] == null)
                {
                    for(int k = j; k < y; k++)
                    {
                        if(Spheres[i][k] != null)
                        {

                            var sphere = Spheres[i][k];

                            var move = sphere.gameObject.AddComponent<Move>();
                            move.Init(new Vector3(i, j), 0.1f);


                            //sphere.gameObject.transform.position = new Vector3(i, j);

                            Spheres[i][j] = sphere;
                            Spheres[i][k] = null;
                            break;
                        }
                    }
                }
            }
        }
    }

    private List<Sphere> GetRow(Sphere[,] matrix, int columnNumber, int startPos, int length)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, columnNumber])
                .ToList().GetRange(startPos, length);
    }

    private List<Sphere> GetColumn(Sphere[,] matrix, int rowNumber, int startPos, int length)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToList().GetRange(startPos, length);
    }

    public void TransformSpheres(TypeEnum from, TypeEnum to)
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (Spheres[i][j].GetType() == from)
                {
                    Spheres[i][j].SetType(to, SphereGenerator.GetColorSprite(to));
                }
            }
        }
    }

    public void TransformRandomSpheres(TypeEnum to, int count)
    {
        var points = new List<Tuple<int, int>>();

        while (points.Count < count)
        {
            int i = UnityEngine.Random.Range(0, x);
            int j = UnityEngine.Random.Range(0, x);
            Tuple<int, int> t = new Tuple<int, int>(i, j);

            if (!points.Contains(t))
            {
                points.Add(t);
            }

        }

        foreach(Tuple<int,int> t in points)
        {
            Spheres[t.Item1][t.Item2].SetType(to, SphereGenerator.GetColorSprite(to));
            
        }
    }

    public void TransformColorToRandomSpheres(TypeEnum from, int count)
    {
        var points = new List<Tuple<int, int>>();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if(Spheres[i][j].Type == from)
                {

                    Tuple<int, int> t = new Tuple<int, int>(i, j);

                    if (!points.Contains(t))
                    {
                        points.Add(t);
                    }
                }
            }
        }

        if(points.Count > count)
        {
            while (points.Count > count)
            {
                int i = UnityEngine.Random.Range(0, points.Count);

                points.RemoveAt(i);
            }
        }

        
        foreach(var t in points)
        {
            TransformToRandom(t.Item1, t.Item2);
        }
    }


    public void HideRandomSpheres(int sourceId, int count)
    {
        var points = new List<Tuple<int, int>>();

        while (points.Count < count)
        {
            int i = UnityEngine.Random.Range(0, x);
            int j = UnityEngine.Random.Range(0, x);
            Tuple<int, int> t = new Tuple<int, int>(i, j);

            if (!points.Contains(t))
            {
                points.Add(t);
            }

        }

        foreach (Tuple<int, int> t in points)
        {
            var fog = Instantiate(HiddenFogPrefab, transform);
            fog.transform.localPosition = new Vector3(t.Item1, t.Item2);
            var originalTexture = sprite.texture;

            Texture2D copyTexture = new Texture2D(originalTexture.width, originalTexture.height);
            copyTexture.SetPixels(originalTexture.GetPixels());
            copyTexture.Apply();


            var s = Sprite.Create(
            copyTexture,
            sprite.rect,
            new Vector2(0.5f, 0.5f),
            sprite.pixelsPerUnit);
            fog.GetComponent<SpriteRenderer>().sprite = s;

            fog.GetComponent<BoardObject>().Source = sourceId;

            //Spheres[t.Item1, t.Item2].HideType();

        }
    }
    public void FreezeRandomSpheres(int sourceId, int count)
    {
        var points = new List<Tuple<int, int>>();

        while (points.Count < count)
        {
            int i = UnityEngine.Random.Range(0, x);
            int j = UnityEngine.Random.Range(0, x);
            Tuple<int, int> t = new Tuple<int, int>(i, j);

            if (!points.Contains(t))
            {
                points.Add(t);
            }

        }

        foreach (Tuple<int, int> t in points)
        {
            var ice = Instantiate(FrozenPrefab, Spheres[t.Item1][t.Item2].transform);
            ice.transform.localPosition = new Vector3(0, 0);
            ice.transform.localScale = new Vector3(1, 1);

            ice.GetComponent<BoardObject>().Source = sourceId;

            Spheres[t.Item1][t.Item2].Frozen = true;
        }
    }

    public void DestroyBoardObjectsFromSource(int sourceId)
    {
        transform.GetComponentsInChildren<BoardObject>().Where(b => b.Source == sourceId).ToList().ForEach(b => Destroy(b.gameObject));
    }

    public void MoveHidden()
    {
        transform.GetComponentsInChildren<HiddenBoardObject>().ToList().ForEach(b => b.MoveRandomDirection());
    }

    public void TriggerSpecialBoardEffects()
    {
        SpreadPlague();
        MoveHidden();
    }

    public void TriggerMovement()
    {
        transform.GetComponentsInChildren<MoveBoardObject>().OrderBy(m => m.Order).ToList().ForEach(b => b.Trigger());
    }

    public void SpreadPlague()
    {
        List<Vector2Int> spreadTargets = new List<Vector2Int>();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                var type = Spheres[i][j].GetType();
                if (type == TypeEnum.Plague)
                {
                    List<Vector2Int> possibleNextTransformations = new List<Vector2Int>();

                    if (i + 1 < x && Spheres[i + 1][j] && Spheres[i + 1][j].GetType() != TypeEnum.Plague)
                    {
                        possibleNextTransformations.Add(new Vector2Int(i + 1, j));
                    }
                    if (j + 1 < y && Spheres[i][j + 1] && Spheres[i][j + 1].GetType() != TypeEnum.Plague)
                    {
                        possibleNextTransformations.Add(new Vector2Int(i, j + 1));
                    }

                    if (i - 1 >= 0 && Spheres[i - 1][j] != null && Spheres[i - 1][j].GetType() != TypeEnum.Plague)
                    {
                        possibleNextTransformations.Add(new Vector2Int(i - 1, j));
                    }
                    if (j - 1 >= 0 && Spheres[i][j - 1] && Spheres[i][j - 1].GetType() != TypeEnum.Plague)
                    {
                        possibleNextTransformations.Add(new Vector2Int(i, j - 1));
                    }

                    if(possibleNextTransformations.Count != 0)
                    {
                        var sphere = possibleNextTransformations[UnityEngine.Random.Range(0, possibleNextTransformations.Count)];

                        spreadTargets.Add(sphere);

                    }
                }
            }
        }

        foreach(var sphere in spreadTargets)
        {
            Debug.Log("transforming" + sphere.x + ", " + sphere.y);
            Spheres[sphere.x][sphere.y].SetType(TypeEnum.Plague, SphereGenerator.GetColorSprite(TypeEnum.Plague));
        }
    }

    public void MoveRow(int index, bool right)
    {
        if (right)
        {
            for (int i = x - 1; i > 0; i--)
            {
                SwitchSpheres(i, i - 1, index, index);
            }
        } else
        {
            for (int i = 0; i < x - 1; i++)
            {
                SwitchSpheres(i, i + 1, index, index);
            }
        }
    }

    public void SpawnRandomMovement(int sourceId)
    {
        
        var dir = UnityEngine.Random.Range(0, 4);
        var index = UnityEngine.Random.Range(0, Math.Min(x, y));

        var movement = Instantiate(MovementPrefab, transform);

        movement.GetComponent<MoveBoardObject>().Init(0, index, dir, sourceId);
    }

    public void MoveColumn(int index, bool up)
    {
        if (up)
        {
            for (int i = y - 1; i > 0; i--)
            {
                SwitchSpheres(index, index, i, i - 1);
            }
        }
        else
        {
            for (int i = 0; i < y - 1; i++)
            {
                SwitchSpheres(index, index, i, i + 1);
            }
        }
    } 

    private void TransformToRandom(int x, int y)
    {
        Destroy(Spheres[x][y].gameObject);
        Spheres[x][y] = SphereGenerator.GenerateRandomSphere(x, y);
    }
}
