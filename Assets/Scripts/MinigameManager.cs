using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MinigameManager : MonoBehaviour
{
    public float totalTime;
    public float timer;
    public bool isTimerActive;

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
                DoStop();
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

}
