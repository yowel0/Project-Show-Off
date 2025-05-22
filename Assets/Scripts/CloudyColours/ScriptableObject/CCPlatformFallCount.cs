using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FallCount", menuName = "Cloudy/FallCount")]
public class CCPlatformFallCount : ScriptableObject
{
    [Tooltip("After VALUE rounds are survived, another platform starts falling. \nVALUE needs to be ordered by ascending, and VALUE=0 at ELEMENT=0 means it starts with one falling platform.")]
    public int[] fallCount;


    public int GetFallCount(int pRound)
    {
        int count = 0;
        for (int i = 0; i < fallCount.Length; i++)
        {
            // If roundNr is lower than VALUE, no need to look further
            if (pRound < fallCount[i]) return count;
            count++;
        }
        return count;
    }
}
