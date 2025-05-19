using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CCAliveLogic : MonoBehaviour
{
    public bool[] playerAlive;
    //[SerializeField]
    //TextMeshProUGUI winText;

    private int nrAlive;
    //PlayerManager playerManager;
    //MinigameManager minigameManager;


    private void OnTriggerEnter(Collider other)
    {
        PlayerShell ps = other.GetComponentInParent<PlayerShell>();

        int playerID = PlayerManager.Instance.GetPlayerID(ps);
        playerAlive[playerID] = false;
        nrAlive--;

        /*for (int i = 0; i < PlayerManager.Instance.GetPlayerCount(); i++)
        {
            if (playerManager.players[i] == ps)
            {
                playerAlive[i] = false;
                nrAlive--;
            }
        }*/
        if (nrAlive <= 1)
        {
            NewRound();
            MinigameManager.Instance.DoStop();
            //winText.gameObject.SetActive(true);
        }
    }

    public void NewRound()
    {
        for (int i = 0; i < playerAlive.Length; i++)
        {
            if (playerAlive[i])
            {
                Scores.Instance.AddScore(i+1, 1);
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
            PlayerManager.Instance.players[i].TeleportAvatar(new Vector3(0, 2, -10 + 2 * i));
        }

    }

    // Singleton
    public static CCAliveLogic Instance;
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        //minigameManager = FindObjectOfType<MinigameManager>();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }


}
