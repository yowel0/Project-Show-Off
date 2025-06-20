using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideSound : MonoBehaviour
{

    [SerializeField] SoundObject collideSound;
    [Tooltip("Time before this sound can be played again")]
    [SerializeField] float totalCooldown;
    [Header("Debug")]
    [SerializeField] float cooldownTimer;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayCollideSound();
        }
    }

    void PlayCollideSound()
    {
        if (cooldownTimer <= 0)
        {
            cooldownTimer = totalCooldown;
            MusicManager.Instance.PlaySound(collideSound);
        }
    }

    private void FixedUpdate()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.fixedDeltaTime;
        }
    }
}
