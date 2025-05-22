using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundTime", menuName = "Cloudy/RoundTime")]
public class CCRoundTime : ScriptableObject
{
    [Tooltip("After surviving ELEMENT rounds, next round time will be VALUE")]
    public float[] roundTime;

    public float GetTime(int round)
    {
        if (round >= roundTime.Length)
        {
            return roundTime[roundTime.Length - 1];
        }
        return roundTime[round];
    }
}
