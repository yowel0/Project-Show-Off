using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider voiceSlider;


    private void OnEnable()
    {
        masterSlider.value = (float)MusicManager.Instance?.GetMasterVolume();
        sfxSlider.value = (float)MusicManager.Instance?.GetSFXVolume();
        bgmSlider.value = (float)MusicManager.Instance?.GetBGMVolume();
        voiceSlider.value = (float)MusicManager.Instance?.GetVoiceVolume();
    }


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
