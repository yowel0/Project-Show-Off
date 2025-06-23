using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBPlatform : MonoBehaviour
{
    [SerializeField] float riseSpeed;

    [Header("Set by code")]
    [Tooltip("What player this platform belongs to (0 = player 1, 1 = player 2, etc.)")]
    [SerializeField] int playerNr;

    [SerializeField] PlayerAvatarMovement pam;

    [SerializeField] bool hasJumpedOverFire;

    [SerializeField] bool playerDead;


    private void DoAliveCheck()
    {
        if (pam != null && pam.IsGrounded())
        {
            AliveManager.Instance.KillPlayer(playerNr);
            playerDead = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DoAliveCheck();
    }
    private void OnTriggerExit(Collider other)
    {
        DoAliveCheck();
        hasJumpedOverFire = true;
    }

    private void Update()
    {
        if (hasJumpedOverFire && pam != null)
        {
            if (pam.IsGrounded())
            {
                // Give point, disable point gain for jumping over fire
                AliveManager.Instance.GivePoint(playerNr);
                hasJumpedOverFire = false;
            }
        }
        if (playerDead)
        {
            if (GetComponent<Rigidbody>() != null)
            {
                GetComponent<Rigidbody>().MovePosition(transform.position + Vector3.up * riseSpeed);
            }
            else
            {
                transform.Translate(Vector3.up * riseSpeed);
            }
        }
    }

    public void SetPlayer(int pPlayerNr)
    {
        playerNr = pPlayerNr;
        hasJumpedOverFire = false;

        pam = PlayerManager.Instance.players[playerNr].GetComponentInChildren<PlayerAvatarMovement>();
    }

}
