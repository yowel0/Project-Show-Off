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
        for (int i = s; i > 0; i--)
        {
            countdownText.text = i.ToString();
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
    }

    public void DoPrepareStop()
    {
        OnPrepareStop?.Invoke();
        Invoke(nameof(DoStop), extendedTime);
    }

    public void DoStop()
    {
        OnStop?.Invoke();
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
