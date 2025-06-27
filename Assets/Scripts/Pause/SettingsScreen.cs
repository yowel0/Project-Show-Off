using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScreen : MonoBehaviour
{
    




    public void SetMasterVolume(float pVolume)
    {
        MusicManager.Instance?.SetMasterVolume(pVolume);
    }

    public void SetSFXVolume(float pVolume)
    {
        MusicManager.Instance?.SetSFXVolume(pVolume);
    }

    public void SetMusicVolume(float pVolume)
    {
        MusicManager.Instance?.SetMusicVolume(pVolume);
    }

    public void SetVoiceVolume(float pVolume)
    {
        MusicManager.Instance?.SetVoiceVolume(pVolume);
    }


}
