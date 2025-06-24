using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The sound that randomly plays")]
    [SerializeField] SoundObject soundToPlay;

    [Tooltip("Minimum time in seconds before random dialogue plays")]
    [Range(0f, 100f)] [SerializeField] float minInterval;
    [Tooltip("Maximum time in seconds before random dialogue plays")]
    [Range(0f, 100f)] [SerializeField] float maxInterval;

    [Header("Debug")]
    [SerializeField] bool isActive;
    [SerializeField] float timer;


    public void StartTimer()
    {
        timer = Mathf.Lerp(minInterval, maxInterval, Random.value);
        isActive = true;
    }

    public void StopTimer()
    {
        isActive = false;
    }

    private void FixedUpdate()
    {
        if (!isActive) return;
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            MusicManager.Instance?.PlaySound(soundToPlay);
            timer += Mathf.Lerp(minInterval, maxInterval, Random.value);
        }
    }
}
