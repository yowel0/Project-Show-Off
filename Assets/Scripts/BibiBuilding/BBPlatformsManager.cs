using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBPlatformsManager : MonoBehaviour
{

    [SerializeField] BBPlatform[] platforms;

    public void OnSetup()
    {
        int offset = 0;
        int pCount = PlayerManager.Instance.GetPlayerCount();
        if (pCount <= 2)
        {
            offset = 1;
        }

        // Activating platforms, accounting for number of players if needed
        for (int i = offset; i < pCount + offset; i++)
        {
            platforms[i].SetPlayer(i - offset);
        }
    }


}
