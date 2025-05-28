using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumManager : MonoBehaviour
{

    [SerializeField] PodiumPlace[] places;

    private void Start()
    {
        
    }


    public void RaiseAll()
    {
        foreach (PodiumPlace place in places)
        {

            place.Raise();
        }
    }


}
