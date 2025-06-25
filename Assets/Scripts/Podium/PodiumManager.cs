using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PodiumManager : MonoBehaviour
{
    [Header("Magic numbers")]
    [Tooltip("Enable to raise places after time has passed, disable to require manual input")]
    [SerializeField] bool isTimeBased;
    [Tooltip("Once this scene is loaded in, how many seconds until the platforms start rising?")]
    [Range(0f, 10f)] [SerializeField] float timeBeforeRaising;
    [Tooltip("How long will it take for the platforms to rise?")]
    [Range(0f, 10f)] [SerializeField] float riseDuration;
    [Tooltip("How high will the platforms rise? \n n = value \n 1st -> 1 x n \n 2nd -> .75 x n \n 3rd -> .5 x n \n 4th -> .25 x n")]
    [SerializeField] float riseHeight;
    [Tooltip("Podium going up sound")]
    [SerializeField] SoundObject podiumRiseSound;

    [Header("Ignorable")]

    public UnityEvent OnAllPlatformsRaised;

    [SerializeField] PodiumPlace[] places;

    [SerializeField] TextMeshProUGUI winnerText;

    [SerializeField] string minigameName;

    [SerializeField] int[] placeholder;
    [SerializeField] int playerCount;

    [Tooltip("Debug. If playerCount is 0, sets playerCount to this")]
    [SerializeField] int overridePlayerCount;

    private void Start()
    {
        try
        {
            playerCount = PlayerManager.Instance.GetPlayerCount();
        }
        catch { }

        if (playerCount == 0) playerCount = overridePlayerCount;

        PreparePlaces();
        
        if (isTimeBased)
        {
            StartCoroutine(RaisePlatformsIn(timeBeforeRaising));
        }
    }


    IEnumerator RaisePlatformsIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        RaiseAll();
    }

    IEnumerator PlatformsAreRaised()
    {
        yield return new WaitForSeconds(riseDuration);
        OnAllPlatformsRaised?.Invoke();
    }

    public void RaiseAll()
    {
        foreach (PodiumPlace place in places)
        {
            place.Raise(riseHeight, riseDuration);
        }

        MusicManager.Instance?.PlaySound(podiumRiseSound);

        StartCoroutine(PlatformsAreRaised());
    }



    public void PreparePlaces()
    {
        try
        {
            minigameName = ScoreTransfer.Instance.GetMinigameName();
        }
        catch { }

        PrepareAppearance();

        PreparePlacements();

    }

    private void PrepareAppearance()
    {
        // Hide unused platforms
        if (playerCount < 4)
        {
            for (int i = playerCount; i < 4; i++)
            {
                places[i].gameObject.SetActive(false);
            }
        }
        // Shift platforms to right if few players
        if (playerCount <= 2)
        {
            places[0].SetPosition(places[1].transform);
            places[1].SetPosition(places[2].transform);
        }
    }

    private void PreparePlacements()
    {
        // Set height for positions
        int[] playerScores;
        try
        {
            playerScores = ScoreTransfer.Instance.GetScores();
        }
        catch
        {
            playerScores = (int[])PlaceholderPlacements().Clone();
        }

        NameWinner(playerScores);

        // Index = player, value = rank
        int[] playerRanking = GetPlayerRankings(playerScores);

        int[] sortedScores = (int[])playerScores.Clone();

        Array.Sort(sortedScores);


        // Telling places how high they rank
        for (int i = 0; i < playerCount; i++)
        {
            places[i].SetScore(playerRanking[i], playerScores[i], sortedScores);
        }
    }


    int[] GetPlayerRankings(int[] playerScores)
    {
        // Index = player, value = rank
        int[] playerRanking = new int[playerCount];

        int storedScore = int.MaxValue;
        int iteration = 0;

        while (iteration < playerCount)
        {
            int currentPlacement = iteration;
            
            int checkScore = GetHighestScoreBelow(playerScores, storedScore);

            // For all scores, if equal to checkScore, set playerRanking at same index to iteration
            for (int i = 0; i < playerCount; i++)
            {
                if (playerScores[i] == checkScore)
                {
                    playerRanking[i] = currentPlacement;
                    // IF A PLAYER HAS 0 POINTS, THE PODIUM DOES NOT RAISE AT ALL
                    if (checkScore == 0) playerRanking[i] = 4;

                    iteration++;
                }
            }

            storedScore = checkScore;
        }

        return playerRanking;
    }

    int GetHighestScoreBelow(int[] playerScores, int scoreLimit)
    {
        // Gets the highest score lower than the previous highest score
        int checkScore = 0;

        foreach (int score in playerScores)
        {
            if (score > checkScore && score < scoreLimit)
            {
                checkScore = score;
            }
        }

        return checkScore;
    }


    int[] PlaceholderPlacements()
    {

        return placeholder;
    }


    public void ShowWinner()
    {
        winnerText.gameObject.SetActive(true);
    }
    public void NameWinner(int[] pPlayerScores)
    {
        string winText = "";

        int winner = GetWinner(pPlayerScores);

        if (winner < 0) winText = "It's a tie!";
        else
        {
            winText = PlayerManager.Instance?.players[winner].userName;
            // Element 0 is player 1
            if (winText == string.Empty || winText == null) winText = "Player " + (winner + 1);
            winText += " won!";
        }


        winnerText.text = winText;
    }

    private int GetWinner(int[] pPlayerScores)
    {
        // Returns -1 if it's a tie
        int winner = 0;
        int highestScore = 0;
        for (int i = 0; i < pPlayerScores.Length; i++)
        {
            if (pPlayerScores[i] > highestScore)
            {
                winner = i;
                highestScore = pPlayerScores[i];
            }
            else if (pPlayerScores[i] == highestScore) winner = -1;
        }

        return winner;
    }
}
