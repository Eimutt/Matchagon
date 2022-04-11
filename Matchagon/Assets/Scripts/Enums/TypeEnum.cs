using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeEnum
{
    Fire,
    Water,
    Grass,
    //Dark,
    Light
}


public static class TypeEnumGenerator
{
    static Array types = Enum.GetValues(typeof(TypeEnum));

    public static TypeEnum GetRandomType()
    {
        int r = UnityEngine.Random.Range(0, types.Length);
        return (TypeEnum)types.GetValue(r);
    }
}
