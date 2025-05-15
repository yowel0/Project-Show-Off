using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FallSpeed", menuName = "ScaryMouse/FallSpeed")]
public class SMFallSpeed : ScriptableObject
{
    [Header("Time in seconds it takes to fall")]
    [SerializeField] float slowFallTime;
    [SerializeField] float mediumFallTime;
    [SerializeField] float fastFallTime;

    public float GetRandomFallTime()
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0: return slowFallTime; 
            case 1: return mediumFallTime; 
            default: return fastFallTime;
        }
    }
}
