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


    private void UpdateText()
    {
        p1ScoreText.text = "p1 Score: " + p1Score;
        p2ScoreText.text = "p2 Score: " + p2Score;
    }


    // Singleton
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
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
    }

}
