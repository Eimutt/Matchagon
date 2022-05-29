using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match
{
    public List<Sphere> Spheres;
    public MatchEnum MatchType;
    public bool Isolated;
    public TypeEnum ElementType;

    public Match(TypeEnum elementType)
    {
        Spheres = new List<Sphere>();
        ElementType = elementType;
    }
}
