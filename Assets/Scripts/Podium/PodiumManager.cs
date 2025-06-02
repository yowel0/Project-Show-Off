using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumManager : MonoBehaviour
{

    [SerializeField] PodiumPlace[] places;

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
        
    }


    public void RaiseAll()
    {

        foreach (PodiumPlace place in places)
        {
            place.Raise();
        }
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
        int[] placementsPerPlayer;
        try
        {
            placementsPerPlayer = ScoreTransfer.Instance.GetScores();
        }
        catch
        {
            placementsPerPlayer = (int[])PlaceholderPlacements().Clone();
        }

        // Index = player, value = rank
        int[] playerRanking = GetPlayerRankings(placementsPerPlayer);


        // Telling places how high they rank
        for (int i = 0; i < playerCount; i++)
        {
            places[i].placement = playerRanking[i];
        }
    }


    int[] GetPlayerRankings(int[] placementsPerPlayer)
    {
        // Index = player, value = rank
        int[] playerRanking = new int[playerCount];

        int storedScore = 10000;
        int iteration = 0;

        while (iteration < playerCount)
        {
            int currentPlacement = iteration;
            
            int checkScore = GetHighestScoreBelow(placementsPerPlayer, storedScore);

            // For all scores, if equal to checkScore, set playerRanking at same index to iteration
            for (int i = 0; i < playerCount; i++)
            {
                if (placementsPerPlayer[i] == checkScore)
                {
                    playerRanking[i] = currentPlacement;
                    iteration++;
                }
            }

            storedScore = checkScore;
        }

        return playerRanking;
    }

    int GetHighestScoreBelow(int[] placementsPerPlayer, int scoreLimit)
    {
        // Gets the highest score lower than the previous highest score
        int checkScore = 0;

        foreach (int score in placementsPerPlayer)
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

}
