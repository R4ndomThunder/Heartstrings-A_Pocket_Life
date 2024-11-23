
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class MinMaxInt
{
    public int Min;
    public int Max;

    public int GetRandom() => Random.Range(Min, Max);
}

[Serializable]
public class MinMaxFloat
{
    public float Min;
    public float Max;

    public float GetRandom() => Random.Range(Min, Max);
}
