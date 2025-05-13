using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Debug traffic light indicators")]
    [SerializeField] GameObject redLight;
    [SerializeField] GameObject orangeLight;
    [SerializeField] GameObject greenLight;

    [SerializeField] bool gameIsActive;
    [SerializeField] bool mouthIsOpen;

    public static TrafficLight Instance;

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
        yield return new WaitForSeconds(orangeTime);

        // End with red
        SetLight(0);
        mouthIsOpen = false;
        yield return new WaitForSeconds(redTime);

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
            case 0: redLight.SetActive(true); break;
            case 1: orangeLight.SetActive(true); break;
            default: greenLight.SetActive(true); break;
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
