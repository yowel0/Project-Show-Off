using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CCAliveLogic : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerShell ps = other.GetComponentInParent<PlayerShell>();

        AliveManager.Instance.KillPlayer(ps, false);

    }


}
