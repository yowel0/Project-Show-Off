using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodThrowLocationManager : MonoBehaviour
{
    [SerializeField] GameObject foodDestination;
    [SerializeField] FoodThrowLocation[] locations;


    public void OnSetup()
    {
        int offset = 0;
        if (PlayerManager.Instance.GetPlayerCount() <= 2)
        {
            offset = -1;
        }
        for (int i = 0; i < locations.Length; i++)
        {
            locations[i].SetPlayer(i + offset);
            locations[i].SetDestination(foodDestination);
        }
    }

    public void StartGame()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            locations[i].EnableThrow();
        }
    }

    public void StopGame()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            locations[i].DisableThrow();
        }
    }

}
