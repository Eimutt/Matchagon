using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeEnum
{
    Fire,
    Water,
    Grass,
    Dark,
    Light,
    Shield,
    Chromatic,
    Death,
    Plague
}


public static class TypeEnumGenerator
{
    static Array types = Enum.GetValues(typeof(TypeEnum));

    public static TypeEnum GetRandomType()
    {
        int r = UnityEngine.Random.Range(0, types.Length);
        return (TypeEnum)types.GetValue(r);
    }

    public static Color GetColor(TypeEnum type)
    {
        if (type == TypeEnum.Fire) return new Color(255f / 255f, 100f / 255f, 0);
        if (type == TypeEnum.Grass) return new Color(20f / 255f, 199f / 255f, 41f / 255f);
        if (type == TypeEnum.Water) return new Color(15f / 255f, 169f / 255f, 212f / 255f);
        if (type == TypeEnum.Dark) return new Color(77f / 255f, 25f / 255f, 77f / 255f);
        if (type == TypeEnum.Light) return new Color(201f / 255f, 188f / 255f, 62f / 255f);
        if (type == TypeEnum.Shield) return new Color(50f / 255f, 50f / 255f, 50f / 255f);

        return Color.white;
    }
}


