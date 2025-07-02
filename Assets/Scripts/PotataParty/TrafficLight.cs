using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrafficLight : MonoBehaviour
{
    [Header("Traffic light times")]
    [Tooltip("Time it takes for Potata to eat")]
    [SerializeField] float redTime;
    [Tooltip("How long Potata will show they're about to eat")]
    [SerializeField] float orangeTime;
    [Tooltip("Minimum time Potata will keep their mouth open")]
    [SerializeField] float greenTimeMin;
    [Tooltip("Maximum time Potata will keep their mouth open")]
    [SerializeField] float greenTimeMax;

    [Header("Sounds")]
    [Tooltip("Plays during red light")]
    [SerializeField] SoundObject eatingSound;
    [Tooltip("Plays when switches to orange light (voice)")]
    [SerializeField] SoundObject stopIndicatorSound;
    [Tooltip("Plays when switches to green light (voice)")]
    [SerializeField] SoundObject continueIndicatorSound;
    [Tooltip("Plays when food hits while green or orange light")]
    [SerializeField] SoundObject happySound;
    [Tooltip("Plays when food hits while red light")]
    [SerializeField] SoundObject angrySound;
    [Tooltip("Plays when green light but didn't receive food for a moment")]
    [SerializeField] SoundObject anticipatingSound;

    [Header("Events for the animation")]
    public UnityEvent OnRed;
    public UnityEvent OnOrange;
    public UnityEvent OnGreen;

    [Header("Debug traffic light indicators")]
    [SerializeField] GameObject redLight;
    [SerializeField] GameObject orangeLight;
    [SerializeField] GameObject greenLight;

    [SerializeField] bool gameIsActive;
    [SerializeField] bool mouthIsOpen;

    [SerializeField] TestingParticle[] goodParticles;
    [SerializeField] TestingParticle[] badParticles;

    public static TrafficLight Instance;

    public UnityEvent annoyedChew;

    bool firstTimeGreen = true;

    public void StartGame()
    {
        mouthIsOpen = true;
        gameIsActive = true;
        NewRound();
    }

    private void NewRound()
    {
        if (!gameIsActive) return;
        StartCoroutine(StartCycle());
    }

    public void StopGame()
    {
        gameIsActive = false;
    }

    public void AddScore(int pPlayer, int pPoints)
    {
        if (!mouthIsOpen) pPoints *= -1;

        if (pPoints > 0)
        {
            foreach (var p in goodParticles) p.IncreaseIntensity();
            //goodParticles.IncreaseIntensity();
            MusicManager.Instance?.PlaySound(happySound);
        }
        else
        {
            foreach (var p in badParticles) p.IncreaseIntensity();
            //badParticles.IncreaseIntensity();
            MusicManager.Instance?.PlaySound(angrySound);
            annoyedChew?.Invoke();
        }

        Scores.Instance.AddScore(pPlayer, pPoints);
    }

    IEnumerator StartCycle()
    {
        // Start with green
        SetLight(2);
        mouthIsOpen = true;
        float greenTime = Random.Range(greenTimeMin, greenTimeMax);
        yield return new WaitForSeconds(greenTime);

        // Continue with orange
        SetLight(1);
        MusicManager.Instance?.PlaySound(stopIndicatorSound);
        yield return new WaitForSeconds(orangeTime);

        // End with red
        SetLight(0);
        MusicManager.Instance?.PlaySound(eatingSound);
        mouthIsOpen = false;
        yield return new WaitForSeconds(redTime);

        MusicManager.Instance?.PlaySound(continueIndicatorSound);

        // Repeat
        NewRound();
    }


    private void SetLight(int light)
    {
        redLight.SetActive(false);
        orangeLight.SetActive(false);
        greenLight.SetActive(false);

        switch (light)
        {
            case 0: redLight.SetActive(true);
                OnRed?.Invoke();
                print("onred");
                break;
            case 1: orangeLight.SetActive(true); 
                OnOrange?.Invoke();
                break;
            default: greenLight.SetActive(true); 
                if (!firstTimeGreen)
                {
                    OnGreen?.Invoke();
                }
                firstTimeGreen = false;
                break;
        }
    }


    // Singleton
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
