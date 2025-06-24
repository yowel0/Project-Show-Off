using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    [Header("Pre game settings")]
    [Tooltip("Duration of the minigame (seconds)")]
    [SerializeField] float totalTime;

    [Tooltip("When the (time based) minigame ends, delay finalizing scores by this many seconds to make all points count")]
    [SerializeField] float extendedTime;

    [Tooltip("Does the minigame end when the time is up? (false -> time is used for when max intensity is reached")]
    [SerializeField] bool isTimeBased;
    [Tooltip("Makes the minigame start immediately when loaded in")]
    [SerializeField] bool startImmediately;

    [Header("Events")]
    [Tooltip("Things that should happen before the game starts")]
    public UnityEvent OnSetup;
    [Tooltip("Things that should happen to start the game")]
    public UnityEvent OnStart;
    [Tooltip("Disable everything that generates new points (Used for time based minigames)")]
    public UnityEvent OnPrepareStop;
    [Tooltip("Stopping all logic")]
    public UnityEvent OnStop;

    [Header("Sounds")]
    [Tooltip("Plays when countdown is over and players can do inputs")]
    [SerializeField] SoundObject startSound;
    [Tooltip("Plays when the scores are finalized")]
    [SerializeField] SoundObject endSound;
    [Tooltip("Plays while the countdown is counting down")]
    [SerializeField] SoundObject countdownBgSound;
    [Tooltip("A voice that says 3")]
    [SerializeField] SoundObject countdown3Sound;
    [Tooltip("A voice that says 2")]
    [SerializeField] SoundObject countdown2Sound;
    [Tooltip("A voice that says 1")]
    [SerializeField] SoundObject countdown1Sound;

    [Header("UI references")]
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI countdownText;

    [Header("Debug")]
    [SerializeField] float timer;
    [SerializeField] bool isTimerActive;

    private void FixedUpdate()
    {
        if (isTimerActive)
        {
            timer -= Time.deltaTime;
            UpdateTimer();
            if (timer < 0)
            {
                if (isTimeBased) DoPrepareStop();
                timer = 0;
                isTimerActive = false;
            }
        }
    }


    public void StartInSeconds(float pSeconds)
    {
        StartCoroutine(StartIn(pSeconds));
    }

    private IEnumerator StartIn(float pSeconds)
    {
        int s = Mathf.RoundToInt(pSeconds);
        countdownText.gameObject.SetActive(true);
        MusicManager.Instance?.PlaySound(countdownBgSound);

        for (int i = s; i > 0; i--)
        {
            countdownText.text = i.ToString();
            if (i == 3) MusicManager.Instance?.PlaySound(countdown3Sound);
            else if (i == 2) MusicManager.Instance?.PlaySound(countdown2Sound);
            else if (i == 1) MusicManager.Instance?.PlaySound(countdown1Sound);
            yield return new WaitForSeconds(1);
        }
        countdownText.gameObject.SetActive(false);
        DoStart();
    }


    public void StartTimer()
    {
        timer = totalTime;
        isTimerActive = true;
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        timeText.text = "Time left: " + Mathf.Max(timer, 0);
    }

    public void DoSetup()
    {
        OnSetup?.Invoke();
    }

    public void DoStart()
    {
        OnStart?.Invoke();
        MusicManager.Instance?.PlaySound(startSound);
    }

    public void DoPrepareStop()
    {
        OnPrepareStop?.Invoke();
        Invoke(nameof(DoStop), extendedTime);
    }

    public void DoStop()
    {
        OnStop?.Invoke();
        MusicManager.Instance?.PlaySound(endSound);
    }


    public float GetTimePercent()
    {
        // Starts at 0, ends at 1
        return (totalTime - timer) / totalTime;
    }


    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (startImmediately)
        {
            DoSetup();
            StartInSeconds(3);
        }
    }
    private void OnDestroy()
    {
        if (Instance == this) Destroy(gameObject);
    }

}
