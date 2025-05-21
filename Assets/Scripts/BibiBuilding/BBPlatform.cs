using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBPlatform : MonoBehaviour
{

    [Tooltip("What player this platform belongs to (0 = player 1, 1 = player 2, etc.)")]
    [SerializeField] int playerNr;

    [SerializeField] PlayerAvatarMovement pam;

    [SerializeField] bool playerIsAlive;
    [SerializeField] bool hasJumpedOverFire;


    private void DoAliveCheck()
    {
        if (pam != null)
        {
            if (pam.IsGrounded())
            {
                AliveManager.Instance.KillPlayer(playerNr);
                // PLAYER DIES
                playerIsAlive = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DoAliveCheck();
    }
    private void OnTriggerExit(Collider other)
    {
        DoAliveCheck();
        if (playerIsAlive)
        {
            // Enable point gain for jumping over fire
            hasJumpedOverFire = true;
        }
    }

    private void Update()
    {
        if (playerIsAlive && hasJumpedOverFire && pam != null)
        {
            if (pam.IsGrounded())
            {
                // Give point, disable point gain for jumping over fire
                AliveManager.Instance.GivePoint(playerNr);
                //Scores.Instance.AddScore(playerNr, 1);
                hasJumpedOverFire = false;
            }
        }
    }

    public void SetPlayer(int pPlayerNr)
    {
        playerIsAlive = true;
        playerNr = pPlayerNr;
        hasJumpedOverFire = false;

        pam = PlayerManager.Instance.players[playerNr].GetComponentInChildren<PlayerAvatarMovement>();
    }

}
