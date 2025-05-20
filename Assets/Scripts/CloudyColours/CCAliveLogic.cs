using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CCAliveLogic : MonoBehaviour
{
    public bool[] playerAlive;

    private int nrAlive;


    private void OnTriggerEnter(Collider other)
    {
        PlayerShell ps = other.GetComponentInParent<PlayerShell>();

        int playerID = PlayerManager.Instance.GetPlayerID(ps);
        playerAlive[playerID] = false;
        nrAlive--;

        if (nrAlive <= 1)
        {
            NewRound();
            MinigameManager.Instance.DoStop();
        }
    }

    public void NewRound()
    {
        for (int i = 0; i < playerAlive.Length; i++)
        {
            if (playerAlive[i])
            {
                Scores.Instance.AddScore(i, 1);
            }
        }
    }

    public void NewGame()
    {
        //winText.gameObject.SetActive(false);
        nrAlive = PlayerManager.Instance.GetPlayerCount();
        for (int i = 0; i < nrAlive; i++)
        {
            playerAlive[i] = true;
            // MOVE PLAYERS TO START!!
            //PlayerManager.Instance.players[i].GetComponentInChildren<Rigidbody>().MovePosition(new Vector3(0, .5f, -10+2*i));
            //PlayerManager.Instance.players[i].TeleportAvatar(new Vector3(0, 2, -10 + 2 * i));
        }

    }

    // Singleton
    public static CCAliveLogic Instance;
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }


}
