using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public List<Vector2Int> nodes;
    public TypeEnum Type;
    public Region(TypeEnum type)
    {
        nodes = new List<Vector2Int>();
        Type = type;
    }

    public bool PartOfDestroy(Vector2Int nodePos)
    {
        for(int i = 0; i < 3; i++)
        {
            if(nodes.Contains(new Vector2Int(nodePos.x + i, nodePos.y)) && nodes.Contains(new Vector2Int(nodePos.x + i - 1, nodePos.y)) && nodes.Contains(new Vector2Int(nodePos.x + i - 2, nodePos.y)))
            {
                return true;
            }
            if(nodes.Contains(new Vector2Int(nodePos.x, nodePos.y + i)) && nodes.Contains(new Vector2Int(nodePos.x, nodePos.y + i - 1)) && nodes.Contains(new Vector2Int(nodePos.x, nodePos.y + i - 2)))
            {
                return true;
            }
        }
        return false;
    }
}
