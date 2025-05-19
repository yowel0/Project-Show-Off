using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    [Tooltip("Movement abilities for this minigame")]
    public ControlScheme controlScheme = ControlScheme.Movement;

    public float totalTime;
    public float timer;
    public bool isTimerActive;
    [Tooltip("Does the minigame end when the time is up? (false -> time is used for when max intensity is reached")]
    public bool isTimeBased;
    [Tooltip("Makes the minigame start immediately when loaded in")]
    [SerializeField] bool startImmediately;

    public UnityEvent OnSetup;
    public UnityEvent OnStart;
    public UnityEvent OnStop;

    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI countdownText;

    private void FixedUpdate()
    {
        if (isTimerActive)
        {
            timer -= Time.deltaTime;
            UpdateTimer();
            if (timer < 0)
            {
                if (isTimeBased) DoStop();
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
        //yield return new WaitForSeconds(pSeconds);
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

        if (PlayerManager.Instance == null)
        {
            PlayerManager.ControlScheme = controlScheme;
        }
        else PlayerManager.Instance.SetControlScheme(controlScheme);

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
