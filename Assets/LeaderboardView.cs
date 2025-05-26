using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : MonoBehaviour
{
    public string leaderboardName;
    public Transform entryContainer;
    [SerializeField]
    LeaderboardManager leaderboardManager;
    [SerializeField]
    EntryView entryViewPrefab;
    [SerializeField]
    int maxEntries = 9;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        ShowEntries();
    }

    void ShowEntries()
    {
        for (int i = entryContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(entryContainer.GetChild(i).gameObject);
        }
        print("test");
        Leaderboard leaderboard = leaderboardManager.GetLeaderboard(leaderboardName);
        for (int i = 0; i < maxEntries; i++)
        {
            print("test " + i + leaderboard.name);
            if (i >= leaderboard.entries.Count)
                break;
            LeaderboardEntry entry = leaderboard.entries[i];
            EntryView entryView = Instantiate(entryViewPrefab, entryContainer);
            entryView.SetUI(i+1,entry.userName,entry.score);
        }
    }
}
