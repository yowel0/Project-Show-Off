using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSoundPlayer : MonoBehaviour
{
    [SerializeField] SoundObject soundObject;

    public void PlaySound()
    {
        MusicManager.Instance.PlaySound(soundObject);
    }
}
