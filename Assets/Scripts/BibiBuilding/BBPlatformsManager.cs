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
        // 2 or less players -> Starts at platform 1, sets player to 0, platform 2 has player set at 1.
        // 3 or 4 players -> starts at platform 0, sets to 0

        // If the player isn't set, the platform doesn't consider the player to be alive
        for (int i = offset; i < pCount + offset; i++)
        {
            platforms[i].SetPlayer(i - offset);
        }
    }


}
