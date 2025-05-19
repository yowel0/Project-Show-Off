using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scores : MonoBehaviour
{

    public static Scores Instance;
    [field: SerializeField] public static int p1Score;
    [field: SerializeField] public static int p2Score;

    [SerializeField] TextMeshProUGUI p1ScoreText;
    [SerializeField] TextMeshProUGUI p2ScoreText;

    [SerializeField] string p1Name;
    [SerializeField] string p2Name;

    private void UpdateText()
    {
        // BAD FIX FOR NOW
        if (PlayerManager.Instance != null && PlayerManager.Instance.players.Count >= 2)
        {
            p1Name = PlayerManager.Instance.players[0].userName;
            p2Name = PlayerManager.Instance.players[1].userName;
        }

        p1ScoreText.text = p1Name + " Score: " + p1Score;
        p2ScoreText.text = p2Name + " Score: " + p2Score;

        //PlayerManager.Instance.players[0].userName
    }


    // Singleton
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        // ADD CHECK FOR HOW MANY PLAYERS ARE PRESENT
        if (PlayerManager.Instance.players.Count >= 2)
        {
            p1Name = PlayerManager.Instance.players[0].userName;
            p2Name = PlayerManager.Instance.players[1].userName;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void AddScore(int pPlayer, int pScoreAddition)
    {
        if (pPlayer == 1) p1Score += pScoreAddition;
        if (pPlayer == 2) p2Score += pScoreAddition;
        
        UpdateText();
    }

    public void ResetScores()
    {
        p1Score = 0;
        p2Score = 0;

        UpdateText();
    }

}
