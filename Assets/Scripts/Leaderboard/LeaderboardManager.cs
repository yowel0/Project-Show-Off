using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    static string fileName = "leaderboards";
    [SerializeField]
    TMP_InputField leaderboardInput;
    [SerializeField]
    TMP_InputField nameInput;
    [SerializeField]
    TMP_InputField scoreInput;

    public static List<Leaderboard> leaderboards = new List<Leaderboard>();
    // Start is called before the first frame update
    void Start()
    {
        leaderboards = FileHandler.ReadFromJSON<Leaderboard>(fileName);
    }

    public static void Submit(string leaderboardName, string userName, int score)
    {
        Leaderboard leaderboard = GetLeaderboard(leaderboardName);
        leaderboard.entries.Add(new LeaderboardEntry(userName, score));
        leaderboard.SortEntries();
        FileHandler.SaveToJSON(leaderboards, fileName);
    }

    public void Submit()
    {
        if (int.TryParse(scoreInput.text, out int score))
        {
            string leaderboardName = leaderboardInput.text;
            string userName = nameInput.text;


            Submit(leaderboardName, userName, score);


            nameInput.text = "";
            scoreInput.text = "";
        }
        else
        {
            throw new Exception("Submitted score is not an int");
        }
    }

    public static Leaderboard GetLeaderboard(string leaderboardName)
    {
        leaderboards = FileHandler.ReadFromJSON<Leaderboard>(fileName);
        leaderboardName = leaderboardName.ToLower();
        foreach (var leaderboard in leaderboards)
        {
            if (leaderboard.name == leaderboardName)
            {
                return leaderboard;
            }
        }
        Leaderboard _leaderboard = new Leaderboard(leaderboardName); 
        leaderboards.Add(_leaderboard);
        return _leaderboard;
    }
}
