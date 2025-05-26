using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Leaderboard
{
    public string name = "leaderboard";

    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

    public Leaderboard(string name)
    {
        this.name = name;
    }

    public void SortEntries()
    {
        entries.Sort();
        Debug.Log("sorting leaderboard: " + name);
    }
}
