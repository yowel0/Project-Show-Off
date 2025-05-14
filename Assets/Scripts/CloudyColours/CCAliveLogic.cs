using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CCAliveLogic : MonoBehaviour
{
    public bool[] playerAlive;
    [SerializeField]
    TextMeshProUGUI winText;

    private int nrAlive;
    PlayerManager playerManager;
    MinigameManager minigameManager;


    private void OnTriggerEnter(Collider other)
    {
        PlayerShell ps = other.GetComponentInParent<PlayerShell>();

        for (int i = 0; i < playerManager.players.Count; i++)
        {
            if (playerManager.players[i] == ps)
            {
                playerAlive[i] = false;
                nrAlive--;
            }
        }
        if (nrAlive <= 1)
        {
            minigameManager.DoStop();
            winText.gameObject.SetActive(true);
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
        winText.gameObject.SetActive(false);
        nrAlive = playerManager.players.Count;
        for (int i = 0; i < nrAlive; i++)
        {
            playerAlive[i] = true;
            // MOVE PLAYERS TO START!!
            playerManager.players[i].GetComponentInChildren<Rigidbody>().MovePosition(new Vector3(0, .5f, -10+2*i));
        }

    }

    // Singleton
    public static CCAliveLogic Instance;
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        playerManager = FindAnyObjectByType<PlayerManager>();
        minigameManager = FindObjectOfType<MinigameManager>();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }


}
