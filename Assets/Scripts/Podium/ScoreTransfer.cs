using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTransfer : MonoBehaviour
{
    [Tooltip("The leaderboard to set the scores to")]
    [SerializeField] string minigameName;

    [SerializeField] int[] scores;



    public void StoreValues()
    {
        scores = Scores.Instance.GetScores();
    }

    public string GetMinigameName()
    {
        return minigameName;
    }

    public int[] GetScores()
    {
        Destroy(gameObject);
        return (int[])scores.Clone();
    }

    // Singleton

    public static ScoreTransfer Instance;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

}
