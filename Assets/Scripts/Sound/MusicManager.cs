using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [Header("Volume sliders")]
    [SerializeField][Range(0f, 1f)] float masterVolume = 1;
    [SerializeField][Range(0f, 1f)] float bgmVolume = 1;
    [SerializeField][Range(0f, 1f)] float sfxVolume = 1;
    [SerializeField][Range(0f, 1f)] float voiceVolume = 1;

    [Header("Other")]
    [SerializeField] SoundObject[] bgmSoundObjects;
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource dialogueAudioSource;

    public static MusicManager Instance;
    private void Start()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += CheckBGM;
    }



    public void CheckBGM(Scene pCurrentScene, Scene pNextScene)
    {
        foreach (SoundObject soundObject in bgmSoundObjects)
        {
            if (soundObject.PlaysInScene(pNextScene.name))
            {
                bgmAudioSource.clip = soundObject.GetSound();
                bgmAudioSource.volume = soundObject.GetLoudness() * bgmVolume * masterVolume;
                bgmAudioSource.Play();
            }
        }
    }

    public void PlaySound(SoundObject pSound)
    {
        if (pSound == null || pSound.GetSound() == null)
        {
            Debug.LogWarning(
                (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name + " in " +
                (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().DeclaringType.Name +
                " Tried to play a sound, but no Sound(Object) was defined");
            return;
        }

        float soundVolume = pSound.GetLoudness() * GetSoundTypeMult(pSound) * masterVolume;

        if (pSound.IsCharacterDialogue())
        {
            PlayDialogue(pSound, soundVolume);
        }
        else
        {
            sfxAudioSource.PlayOneShot(pSound.GetSound(), soundVolume);
        }
    }

    float GetSoundTypeMult(SoundObject pSound)
    {
        switch (pSound.soundType)
        {
            case SoundObject.SoundType.SFX: return sfxVolume; 
            case SoundObject.SoundType.BGM: return bgmVolume; 
            case SoundObject.SoundType.VoiceOver: return voiceVolume;
        }
        return 1;
    }

    void PlayDialogue(SoundObject pSound, float pSoundTypeMult)
    {
        // If not playing, audio can play. If it has priority, audio always plays.
        if (!dialogueAudioSource.isPlaying || pSound.HasPriority())
        {
            dialogueAudioSource.clip = pSound.GetSound();
            dialogueAudioSource.volume = pSound.GetLoudness() * pSoundTypeMult * masterVolume;
            dialogueAudioSource.Play();
        }
        else
        {
            Debug.Log(
                (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name +
                " Tried to play a sound, but a dialogue was already playing");
        }


    }



    public void SetMasterVolume(float pVolume)
    {
        masterVolume = pVolume;
    }

    public void SetSFXVolume(float pVolume)
    {
        sfxVolume = pVolume;
    }

    public void SetMusicVolume(float pVolume)
    {
        float mult = pVolume / bgmVolume;
        bgmVolume = pVolume;
        bgmAudioSource.volume *= mult;
    }

    public void SetVoiceVolume(float pVolume)
    {
        voiceVolume = pVolume;
    }

    public float GetMasterVolume()  { return masterVolume; }
    public float GetSFXVolume()     { return sfxVolume; }
    public float GetBGMVolume()     { return bgmVolume; }
    public float GetVoiceVolume()   { return voiceVolume; }

}
