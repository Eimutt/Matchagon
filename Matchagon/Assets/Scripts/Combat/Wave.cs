using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericWave<T>
{
    public GameObject Enemy;
    public int Quanitity;
    public int Turn;
}

public abstract class GenericEncounter<T, U> where T : GenericWave<U>
{
    public int Difficulty;
    public List<T> Waves;
}

[System.Serializable]
public class Wave : GenericWave<int> { }

[System.Serializable]
public class Encounter : GenericEncounter<Wave, int> { }