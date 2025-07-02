using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveManager : MonoBehaviour
{
    [Tooltip("Plays when a player dies")]
    [SerializeField] SoundObject playerDeathSound;

    [Tooltip("True = Minigame ends when everyone died \n False = Minigame ends when only one player remains")]
    [SerializeField] bool noSurvivors;

    public bool[] playerAlive = new bool[4];

    private int nrAlive;


    public void KillPlayer(int pPlayer, bool pCheckAlive = true)
    {
        if (playerAlive[pPlayer])
        {
            nrAlive--;
            playerAlive[pPlayer] = false;
            MusicManager.Instance?.PlaySound(playerDeathSound);
            if (pCheckAlive) CheckAlive();
        }
    }
    public void KillPlayer(PlayerShell pPlayer, bool pCheckAlive = true)
    {
        KillPlayer(PlayerManager.Instance.GetPlayerID(pPlayer), pCheckAlive);
    }

    public void CheckAlive()
    {
        if (noSurvivors || PlayerManager.Instance?.GetPlayerCount() <= 1)
        {
            if (nrAlive <= 0) MinigameManager.Instance.DoStop();
        }
        else
        {
            if (nrAlive <= 1)
            {
                //GiveEveryonePoint(); // Making sure there's no tie
                MinigameManager.Instance.DoStop();
            }
        }
    }

    public void GivePoint(int pPlayer)
    {
        if (playerAlive[pPlayer])
        {
            Scores.Instance.AddScore(pPlayer, 1);
        }
    }

    public void GiveEveryonePoint()
    {
        for (int i = 0; i < playerAlive.Length; i++)
        {
            GivePoint(i);
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
