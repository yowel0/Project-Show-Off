using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundObject", menuName = "Sound/SoundObject")]
public class SoundObject : ScriptableObject
{
    public enum SoundType { SFX, BGM, VoiceOver}

    [Tooltip("All sounds triggered by the same action")]
    [SerializeField] SoundList[] soundList;

    [Header("Other settings")]

    [Tooltip("Select what category this sound belongs to")]
    public SoundType soundType;

    [Tooltip("If this is BGM, the sound will automatically play in the scene(s) named in this array")]
    [SerializeField] string[] bgmSceneName;

    [Tooltip("If true, uses seperate AudioSource where only one sound can play at a time. If another sound is already playing, box below decides what happens.")]
    [SerializeField] bool isCharacterDialogue;

    [Tooltip("If checked, interrupts the sound that was already playing. Otherwise, it will not play at all. \n Only applies if above is true")]
    [SerializeField] bool hasPriority;

    [Header("Debug")]
    [Tooltip("Shows the last (or currently played) sound. Main use is for the correct loudness to be associated with a sound clip")]
    [SerializeField] int currentSound;

    public AudioClip GetSound()
    {
        currentSound = UnityEngine.Random.Range(0, soundList.Length);
        return soundList[currentSound].sound;
    }

    public float GetLoudness()
    {
        return soundList[currentSound].loudness;
    }

    public bool PlaysInScene(string sceneName)
    {
        if (soundType != SoundType.BGM) return false;

        foreach (string scene in bgmSceneName)
        {
            if (scene == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsCharacterDialogue()
    {
        return isCharacterDialogue;
    }

    public bool HasPriority()
    {
        return hasPriority;
    }


}

[Serializable]
public struct SoundList
{
    [Tooltip("The sound this represents")]
    [SerializeField] public AudioClip sound;

    [Tooltip("Individual sound volume. Going higher than 1 doesn't work for BGM")]
    [Range(0f, 3f)] public float loudness;


}