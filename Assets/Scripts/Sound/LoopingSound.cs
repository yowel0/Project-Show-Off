using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingSound : MonoBehaviour
{
    [Tooltip("The sound that loops")]
    [SerializeField] SoundObject loopedSound;
    [Tooltip("Does this play as soon as this is loaded in?")]
    [SerializeField] bool playOnStart;

    [Header("Debug")]
    [SerializeField] float soundTime;
    [SerializeField] float timer;
    [SerializeField] bool isActive;

    [Header("Override values")]
    [SerializeField] bool useOverrideTime;
    [SerializeField] float overrideTime;

    private void Start()
    {
        if (overrideTime == 0) overrideTime = 10;
        soundTime = (useOverrideTime || loopedSound == null) ? overrideTime : loopedSound.GetSound().length;
        if (playOnStart)
        {
            EnableSound();
        }
    }

    public void EnableSound()
    {
        isActive = true;
        PlaySound();
    }
    public void DisableSound()
    {
        isActive = false;
    }

    void PlaySound()
    {
        timer = soundTime;
        MusicManager.Instance?.PlaySound(loopedSound);
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0) PlaySound();
        }
    }
}
