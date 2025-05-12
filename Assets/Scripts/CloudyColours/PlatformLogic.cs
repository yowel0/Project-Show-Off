using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLogic : MonoBehaviour
{
    [Tooltip("Scriptable object holding an array. Element = round, value = time in seconds")]
    [SerializeField] CCRoundTime timeDecreaseRounds;
    [Tooltip("Scriptable object holding an array. Element = nr of falling platforms + 1, value = what round this applies to")]
    [SerializeField] CCPlatformFallCount fallCount;

    public int round;

    [SerializeField] SinglePlatform[] platforms;
    [Tooltip("How long a platform will disappear for")]
    [SerializeField] float platformDisappearTime;
    [Tooltip("Time between platform returning and next one blinking")]
    [SerializeField] float restTime;
    [SerializeField] GameObject WhiteSection;

    bool isPlaying;

    public void StartGame()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            StartRound();
            round = 0;
        }
    }
    public void StopGame()
    {
        isPlaying = false;
    }
    public void StartRound()
    {
        if (!isPlaying) return;
        StartCoroutine(BlinkPlatformFor(timeDecreaseRounds.GetTime(round)));
        round++;
    }
    IEnumerator BlinkPlatformFor(float seconds)
    {
        int count = fallCount.GetFallCount(round);

        int[] selectedPlatforms = new int[count];
        selectedPlatforms = GetRandomPlatforms(count);

        GameObject[] blinkingPlatforms = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            blinkingPlatforms[i] = Instantiate(WhiteSection, transform);
            blinkingPlatforms[i].transform.Rotate(new Vector3(0, selectedPlatforms[i] * 60, 0));
            blinkingPlatforms[i].transform.Translate(new Vector3(0, .1f, 0), Space.World);
        }


        // Blinking
        for (float i = seconds; i > 0.01; i -= seconds / 4f)
        {
            foreach (GameObject g in blinkingPlatforms) g.SetActive(true);
            yield return new WaitForSeconds(seconds / 9);
            foreach (GameObject g in blinkingPlatforms) g.SetActive(false);
            yield return new WaitForSeconds(seconds / 9);
        }
        foreach (GameObject g in blinkingPlatforms) g.SetActive(true);
        yield return new WaitForSeconds(seconds / 9);

        // Platform gone
        foreach (GameObject g in blinkingPlatforms) Destroy(g);
        for (int i = 0; i < count;i++)
        {
            platforms[selectedPlatforms[i]].Disappear(platformDisappearTime);
        }

        // Time until next platform starts blinking
        yield return new WaitForSeconds(platformDisappearTime + restTime);
        StartRound();

    }



    private int[] GetRandomPlatforms(int count)
    {
        List<int> allPlatforms = new List<int>();
        for (int i = 0; i < platforms.Length; i++) allPlatforms.Add(i);

        int[] selectedPlatforms = new int[count];
        for (int i = 0; i < count; i++)
        {
            int selPlatform = Random.Range(0, allPlatforms.Count);

            selectedPlatforms[i] = allPlatforms[selPlatform];
            allPlatforms.RemoveAt(selPlatform);

        }


        return selectedPlatforms;
    }

}
