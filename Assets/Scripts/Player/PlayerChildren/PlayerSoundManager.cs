using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{

    [Tooltip("Plays if this player collides with another player")]
    [SerializeField] SoundObject bumpSound;

    [Tooltip("Plays when player jumps")]
    [SerializeField] SoundObject jumpSound;

    [Tooltip("Plays when player lands after a jump")]
    [SerializeField] SoundObject landSound;

    [Tooltip("Plays when player lands on a book after a jump")]
    [SerializeField] SoundObject landBookSound;

    [Tooltip("Plays when launched by a pillow")]
    [SerializeField] SoundObject pillowBounceSound;

    [Tooltip("Plays when player is walking")]
    [SerializeField] SoundObject footstepSound;

    [Tooltip("Plays when player is walking/skating on ice")]
    [SerializeField] SoundObject footstepIceSound;

    [Tooltip("Plays when player is walking on carpet")]
    [SerializeField] SoundObject footstepCarpetSound;





    public void PlayBump()
    {
        if (!MusicManager.Instance) return;
        MusicManager.Instance?.PlaySound(bumpSound);
    }

    public void PlayJump()
    {
        if (!MusicManager.Instance) return;
        MusicManager.Instance?.PlaySound(jumpSound);
    }

    public void PlayLand(string tag = "")
    {
        if (!MusicManager.Instance) return;
        // Add logic for what the player landed on
        if (tag.ToLower() == "book") MusicManager.Instance?.PlaySound(landBookSound);
        else MusicManager.Instance?.PlaySound(landSound);
    }

    public void PlayPillowBounce()
    {
        if (!MusicManager.Instance) return;
        MusicManager.Instance?.PlaySound(pillowBounceSound);
    }

    public void PlayFootstep(string tag = "")
    {
        if (!MusicManager.Instance) return;
        // Add logic for ground type (normal, carpet, grass)
        if (tag.ToLower() == "ice") MusicManager.Instance?.PlaySound(footstepIceSound);
        else if (tag.ToLower() == "carpet") MusicManager.Instance?.PlaySound(footstepCarpetSound);
        else MusicManager.Instance?.PlaySound(footstepSound);
    }


}
