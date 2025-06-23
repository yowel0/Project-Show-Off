using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerProximity : MonoBehaviour
{

    [Tooltip("This event triggers when a player enters the radius of the portal")]
    public UnityEvent OnPlayerApproach;

    [Tooltip("When everyone's in")]
    public UnityEvent OnAllPlayersEnter;

    [Tooltip("When someone leaves the area")]
    public UnityEvent OnPlayerLeave;

    [Tooltip("When there's nobody in the thing anymore")]
    public UnityEvent OnAllPlayersLeft;

    [Header("Debug")]
    [SerializeField] int playersNear = 0;

    public bool[] NearbyPlayers = new bool[4];




    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playersNear++;
            int playerID = PlayerManager.Instance.GetPlayerID(other.GetComponentInParent<PlayerShell>());
            NearbyPlayers[playerID] = true;

            OnPlayerApproach?.Invoke();
            if (playersNear > 0 && playersNear == PlayerManager.Instance.GetPlayerCount()) OnAllPlayersEnter?.Invoke();

            //CheckPopup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playersNear--;
            int playerID = PlayerManager.Instance.GetPlayerID(other.GetComponentInParent<PlayerShell>());
            NearbyPlayers[playerID] = false;

            OnPlayerLeave?.Invoke();
            if (playersNear == 0) OnAllPlayersLeft?.Invoke();

            //CheckPopup();
        }
    }



}
