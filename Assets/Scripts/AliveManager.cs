using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveManager : MonoBehaviour
{
    [Tooltip("True = Minigame ends when everyone died \n False = Minigame ends when only one player remains")]
    [SerializeField] bool noSurvivors;

    public bool[] playerAlive = new bool[4];

    private int nrAlive;


    public void KillPlayer(int pPlayer)
    {
        if (playerAlive[pPlayer])
        {
            nrAlive--;
            playerAlive[pPlayer] = false;
            CheckAlive();
        }
    }
    public void KillPlayer(PlayerShell pPlayer)
    {
        KillPlayer(PlayerManager.Instance.GetPlayerID(pPlayer));
    }

    private void CheckAlive()
    {
        if (noSurvivors)
        {
            if (nrAlive <= 0) MinigameManager.Instance.DoStop();
        }
        else
        {
            if (nrAlive <= 1) MinigameManager.Instance.DoStop();
        }
    }

    public void GivePoint(int pPlayer)
    {
        if (playerAlive[pPlayer])
        {
            Scores.Instance.AddScore(pPlayer, 1);
        }
    }


    public void NewGame()
    {
        nrAlive = PlayerManager.Instance.GetPlayerCount();
        for (int i = 0; i < nrAlive; i++)
        {
            playerAlive[i] = true;
        }
    }

    // Singleton
    public static AliveManager Instance;
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
