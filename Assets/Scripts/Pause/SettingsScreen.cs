using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    [Tooltip("Plays when the settings screen opens")]
    [SerializeField] SoundObject openSettingsSound;
    [Tooltip("Plays when you click on a button")]
    [SerializeField] SoundObject clickSound;

    [Header("Adjust volume sounds")]
    [Tooltip("Also plays when adjusting master volume")]
    [SerializeField] SoundObject sfxTestSound;
    [SerializeField] SoundObject bgmTestSound;
    [SerializeField] SoundObject voiceTestSound;


    [Header("Ignore")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider voiceSlider;


    private void OnEnable()
    {
        MusicManager.Instance?.PlaySound(openSettingsSound);

        masterSlider.value = (float)MusicManager.Instance?.GetMasterVolume();
        sfxSlider.value = (float)MusicManager.Instance?.GetSFXVolume();
        bgmSlider.value = (float)MusicManager.Instance?.GetBGMVolume();
        voiceSlider.value = (float)MusicManager.Instance?.GetVoiceVolume();
    }


    public void PlayClickSound(bool playSound)
    {
        if (playSound) MusicManager.Instance?.PlaySound(clickSound);
    }

    public void SetMasterVolume(float pVolume)
    {
        MusicManager.Instance?.SetMasterVolume(pVolume);
        MusicManager.Instance?.PlaySound(sfxTestSound);
    }

    public void SetSFXVolume(float pVolume)
    {
        MusicManager.Instance?.SetSFXVolume(pVolume);
        MusicManager.Instance?.PlaySound(sfxTestSound);
    }

    public void SetMusicVolume(float pVolume)
    {
        MusicManager.Instance?.SetMusicVolume(pVolume);
        MusicManager.Instance?.PlaySound(bgmTestSound);
    }

    public void SetVoiceVolume(float pVolume)
    {
        MusicManager.Instance?.SetVoiceVolume(pVolume);
        MusicManager.Instance?.PlaySound(voiceTestSound);
    }


}
