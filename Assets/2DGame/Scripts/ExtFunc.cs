using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtFunc
{
    public static bool Scope(this int v1, int rengeMin, int rengeMax)
    {
        return v1 >= rengeMin && v1 <= rengeMax;
    }

    public static float TakeDecimalPoint(this float v, int whichValue)
    {
        return Mathf.Round(v * (10 ^ whichValue)) / (10 ^ whichValue);
    }
}
