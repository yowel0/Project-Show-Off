using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformLogic : MonoBehaviour
{
    [Header("Variable settings per round")]

    [Tooltip("Scriptable object holding an array. Element = round, value = time in seconds")]
    [SerializeField] CCRoundTime timeDecreaseRounds;
    [Tooltip("Scriptable object holding an array. Element = nr of falling platforms + 1, value = what round this applies to")]
    [SerializeField] CCPlatformFallCount fallCount;


    [Header("Consistent settings per round")]

    [Tooltip("How long a platform will disappear for")]
    [SerializeField] float platformDisappearTime;
    [Tooltip("Time between platform returning and next one blinking")]
    [SerializeField] float restTime;
    [Tooltip("Number of times a blink switches state. \nEven = disappear after no blink (White -> Colour -> Gone) \nUneven = disappear after a blink (Colour -> White -> Gone")]
    [Range(1, 20)] [SerializeField] int blinkStateChangeAmount;


    [Header("Misc")]
    [Tooltip("Plays every time platforms start flickering (dialogue 1)")]
    [SerializeField] SoundObject cloudyDontFall;
    [Tooltip("Plays every time a platform goes poof")]
    [SerializeField] SoundObject platformFallSound;
    [Tooltip("Plays every time a platform reappears")]
    [SerializeField] SoundObject platformBackSound;
    [Tooltip("Plays every time the flicker appears")]
    [SerializeField] SoundObject platformFlickerSound;

    [Tooltip("Triggers when a round has been completed")]
    public UnityEvent OnNewRound;
    [Tooltip("Reference to the falling platforms in the scene")]
    [SerializeField] SinglePlatform[] platforms;
    [Tooltip("The blink")]
    [SerializeField] GameObject WhiteSection;
    [SerializeField] int round;


    bool isPlaying;

    public void StartGame()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            round = 0;
            StartRound();
        }
    }
    public void StopGame()
    {
        isPlaying = false;
    }
    public void StartRound()
    {
        if (!isPlaying) return;
        // Might want to add logic if this gets annoying
        MusicManager.Instance.PlaySound(cloudyDontFall);
        StartCoroutine(BlinkPlatformFor(timeDecreaseRounds.GetTime(round)));
        round++;
    }
    IEnumerator BlinkPlatformFor(float pSeconds)
    {
        // Determining values
        int count = fallCount.GetFallCount(round);
        int[] selectedPlatforms = GetRandomPlatforms(count);
        GameObject[] blinkingPlatforms = CreateBlinkPlatforms(count, selectedPlatforms);
        float blinkStateDuration = pSeconds / blinkStateChangeAmount;

        // Blinking
        for (float i = pSeconds; i > 0.01; i -= blinkStateDuration)
        {
            SwitchBlinkState(blinkingPlatforms);
            yield return new WaitForSeconds(blinkStateDuration);
        }

        // Platform gone
        MakePlatformsDisappear(selectedPlatforms, blinkingPlatforms);
        

        // Time until next platform starts blinking
        yield return new WaitForSeconds(platformDisappearTime);
        MusicManager.Instance.PlaySound(platformBackSound);

        // Cooldown time between platforms returning and next round
        yield return new WaitForSeconds(restTime);
        OnNewRound?.Invoke();
        StartRound();

    }


    private void MakePlatformsDisappear(int[] pSelectedPlatforms, GameObject[] pBlinkingPlatforms)
    {
        foreach (GameObject g in pBlinkingPlatforms) Destroy(g);
        for (int i = 0; i < pSelectedPlatforms.Length; i++)
        {
            platforms[pSelectedPlatforms[i]].Disappear(platformDisappearTime);
            MusicManager.Instance.PlaySound(platformFallSound);
        }
    }

    private void SwitchBlinkState(GameObject[] pBlinkPlatforms)
    {
        foreach (GameObject g in pBlinkPlatforms)
        {
            g.SetActive(!g.activeSelf);
            if (g.activeSelf) MusicManager.Instance.PlaySound(platformFlickerSound);
        }
    }

    private GameObject[] CreateBlinkPlatforms(int pCount, int[] pSelectedPlatforms)
    {
        GameObject[] blinkingPlatforms = new GameObject[pCount];

        for (int i = 0; i < pCount; i++)
        {
            blinkingPlatforms[i] = Instantiate(WhiteSection, transform);
            blinkingPlatforms[i].transform.Rotate(new Vector3(0, pSelectedPlatforms[i] * 60, 0));
            blinkingPlatforms[i].transform.Translate(new Vector3(0, .1f, 0), Space.World);

            blinkingPlatforms[i].SetActive(false);
        }

        return blinkingPlatforms;
    }

    private int[] GetRandomPlatforms(int pCount)
    {
        List<int> allPlatforms = new List<int>();
        for (int i = 0; i < platforms.Length; i++) allPlatforms.Add(i);

        int[] selectedPlatforms = new int[pCount];
        for (int i = 0; i < pCount; i++)
        {
            int selPlatform = Random.Range(0, allPlatforms.Count);

            selectedPlatforms[i] = allPlatforms[selPlatform];
            allPlatforms.RemoveAt(selPlatform);
        }

        return selectedPlatforms;
    }

}
