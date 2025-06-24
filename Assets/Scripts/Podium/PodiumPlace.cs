using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PodiumPlace : MonoBehaviour
{
    [Tooltip("If this place is first and reaches the top, trigger event")]
    public UnityEvent OnPeakReached;

    [Header("Sounds")]
    [Tooltip("Player sound when podium reaches first place (yippee)")]
    [SerializeField] SoundObject playerWinSound;
    [Tooltip("Player sound when podium reaches peak but not first place (aww)")]
    [SerializeField] SoundObject playerLoseSound;
    [Tooltip("Confetti-like sound when podium reaches first place")]
    [SerializeField] SoundObject podiumFirstSound;
    [Tooltip("poof(?)-like sound when podium reaches first place")]
    [SerializeField] SoundObject podiumLoseSound;

    [Header("Debug")]
    [Tooltip("Is this platform supposed to go up?")]
    [SerializeField] bool isRaising;
    [Tooltip("How high does first place go?")]
    [SerializeField] float raiseHeight;
    [Tooltip("How long does it take for first place to reach the top?")]
    [SerializeField] float riseDuration;
    [Tooltip("How long has this place been rising for?")]
    [SerializeField] float timeRising;

    [SerializeField] int placement;
    [SerializeField] int score;

    [SerializeField] TextMeshProUGUI scoreText;

    Rigidbody rb;
    Vector3 startPos;
    Vector3 endPos;

    int[] breakpoints;


    int reachedBreakpointAmount;
    int displayedScore;

    float maxRisePercent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (isRaising)
        {
            // Calculate how high to go
            float risePercent = Mathf.Min(timeRising / riseDuration, maxRisePercent);
            Vector3 newPos = Vector3.Lerp(startPos, endPos, risePercent);

            // Move
            rb.MovePosition(newPos);

                UpdateScoreText();
            // Check if the final position is reached
            if (risePercent >= maxRisePercent)
            {
                isRaising = false;

                // If this place ends first, trigger event
                if (placement == 0)
                {
                    OnPeakReached?.Invoke();
                    MusicManager.Instance?.PlaySound(playerWinSound);
                    MusicManager.Instance?.PlaySound(podiumFirstSound);
                }
                else
                {
                    MusicManager.Instance?.PlaySound(playerLoseSound);
                    MusicManager.Instance?.PlaySound(podiumLoseSound);
                }

            }

            timeRising += Time.fixedDeltaTime;

        }
    }

    private void UpdateScoreText()
    {
        float risePercent = Mathf.Min(timeRising / riseDuration, maxRisePercent);

        reachedBreakpointAmount = Mathf.Min(Mathf.FloorToInt(risePercent / .25f), 3);


        int currentBreakpoint = breakpoints[reachedBreakpointAmount];

        int lastBreakpoint = reachedBreakpointAmount == 0 ? 0 : breakpoints[reachedBreakpointAmount - 1];

    
        displayedScore = Mathf.FloorToInt(risePercent % .25f * 4 * (currentBreakpoint - lastBreakpoint)) + lastBreakpoint;

        if (risePercent == maxRisePercent)
        {
            displayedScore = score;
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + displayedScore.ToString();
        }

    }

    public void SetScore(int pPlacement, int pScore, int[] pSortedScores)
    {
        placement = pPlacement;
        score = pScore;

        breakpoints = pSortedScores;

        maxRisePercent = 1 - .25f * placement;


        // Correct for less than 4 players
        CorrectForLessPlayers();


        // CHECK TIES
        FixTies();

    }

    void CorrectForLessPlayers()
    {

        int playerCount = breakpoints.Length;
        if (PlayerManager.Instance != null) playerCount = PlayerManager.Instance.GetPlayerCount();

        if (playerCount < breakpoints.Length)
        {
            int nrToFix = 4 - playerCount;
            for (int i = 0; i < nrToFix; i++)
            {
                float percent = (float)(i + 1) / (5 - playerCount);
                breakpoints[i] = Mathf.FloorToInt(breakpoints[nrToFix] * percent);
            }
        }


    }

    void FixTies()
    {
        for (int i = 0; i < breakpoints.Length-1; i++)
        {
            // A number is always identical to itself, so it's at least 1
            int numberSame = GetIdenticalAmount(i);

            int breakpointBelow = i > 0 ? breakpoints[i - 1] : 0;
            int breakpointDifference = breakpoints[i] - breakpointBelow;

            // i = array index we workin with
            // j = (n-1)th tied number in the 'list' to fix
            for (int j = 1; j < numberSame; j++)
            {
                // Uses fraction of the point difference for tweening. 
                // Has the same 'formula' as correcting for less players
                float percent = (float)j / numberSame;
                int newBreakpoint = Mathf.FloorToInt(breakpointDifference * percent) + breakpointBelow;
                breakpoints[i + j - 1] = newBreakpoint;
            }
        }

    }

    int GetIdenticalAmount(int index)
    {
        int numberSame = 1;

        // Checking how many are the same
        for (int j = index + 1; j < breakpoints.Length; j++)
        {
            if (breakpoints[index] == breakpoints[j])
            {
                numberSame++;
            }
        }

        return numberSame;
    }


    public void SetPosition(Transform pNewPos)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.MovePosition(pNewPos.position);
        transform.position = pNewPos.position;
        startPos = pNewPos.position;
    }

    public void Raise(float pRaiseHeight, float pRiseDuration)
    {
        isRaising = true;
        raiseHeight = pRaiseHeight;
        riseDuration = pRiseDuration;

        endPos = startPos + Vector3.up * raiseHeight;
    }

}
