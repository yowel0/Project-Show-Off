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

    [Tooltip("Plays when player is walking on grass")]
    [SerializeField] SoundObject footstepGrassSound;

    [Tooltip("Plays when player is walking on carpet")]
    [SerializeField] SoundObject footstepCarpetSound;





    public void PlayBump()
    {
        if (!MusicManager.Instance) return;
        MusicManager.Instance.PlaySound(bumpSound);
    }

    public void PlayJump()
    {
        if (!MusicManager.Instance) return;
        MusicManager.Instance.PlaySound(jumpSound);
    }

    public void PlayLand(string tag = "")
    {
        if (!MusicManager.Instance) return;
        // Add logic for what the player landed on
        if (tag == "Book") MusicManager.Instance.PlaySound(landBookSound);
        MusicManager.Instance.PlaySound(landSound);
    }

    public void PlayPillowBounce()
    {
        if (!MusicManager.Instance) return;
        MusicManager.Instance.PlaySound(pillowBounceSound);
    }

    public void PlayFootstep()
    {
        if (!MusicManager.Instance) return;
        // Add logic for ground type (normal, carpet, grass)
        MusicManager.Instance.PlaySound(footstepSound);
    }


}
