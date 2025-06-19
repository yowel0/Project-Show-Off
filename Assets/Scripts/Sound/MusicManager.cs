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
                (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name + 
                " Tried to play a sound, but no SoundObject was defined");
            return;
        }

        float soundTypeMult = 1;
        switch (pSound.soundType)
        {
            case SoundObject.SoundType.SFX: soundTypeMult = sfxVolume; break;
            case SoundObject.SoundType.BGM: soundTypeMult = bgmVolume; break;
            case SoundObject.SoundType.VoiceOver: soundTypeMult = voiceVolume; break;
        }

        sfxAudioSource.PlayOneShot(pSound.GetSound(), pSound.GetLoudness() * soundTypeMult * masterVolume);
    }

}
