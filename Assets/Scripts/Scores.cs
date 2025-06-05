using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scores : MonoBehaviour
{

    public static Scores Instance;

    [SerializeField] int scoreMult;

    [SerializeField] TextMeshProUGUI winnerText;

    [SerializeField] GameObject[] scoreElements;
    [SerializeField] int[] scores;


    public void DoSetup()
    {
        int playerCount = PlayerManager.Instance.GetPlayerCount();

        for (int i = 0; i < scoreElements.Length; i++)
        {
            scoreElements[i].SetActive(false);
        }
        for (int i = 0; i < playerCount; i++)
        {
            scoreElements[i].SetActive(true);
        }

        ResetScores();
    }

    private void UpdateText()
    {

        for (int i = 0; i < scoreElements.Length; i++)
        {
            scoreElements[i].GetComponentInChildren<TextMeshProUGUI>().SetText(scores[i].ToString());
        }
    }

    public void ShowWinner()
    {
        winnerText.gameObject.SetActive(true);
        string winText = "";

        int winner = GetWinner();

        if (winner < 0) winText = "It's a tie!";
        else
        {
            winText = PlayerManager.Instance.players[winner].userName;
            // Element 0 is player 1
            if (winText == string.Empty) winText = "Player " + (winner + 1);
            winText += " won!";
        }

        
        winnerText.text = winText;
    }

    private int GetWinner()
    {
        // Returns -1 if it's a tie
        int winner = 0;
        int highestScore = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] > highestScore)
            {
                winner = i;
                highestScore = scores[i];
            }
            else if (scores[i] == highestScore) winner = -1;
        }

        return winner;
    }


    public int[] GetScores()
    {
        return scores;
    }


    // Singleton
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void AddScore(int pPlayer, int pScoreAddition)
    {
        scores[pPlayer] += pScoreAddition * scoreMult;
        
        UpdateText();
    }

    public void ResetScores()
    {
        for (int i = 0; i < scores.Length; i++) 
        {
            scores[i] = 0;
        }
        winnerText.gameObject.SetActive(false);

        UpdateText();
    }

}
