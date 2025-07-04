using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarAnimationController : MonoBehaviour
{
    PlayerInput playerInput;
    Avatar avatar;
    Animator animator;
    PlayerAvatarMovement playerAvatarMovement;
    Rigidbody rb;

    public bool holdingBox = false;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        avatar = GetComponentInChildren<Avatar>();
        animator = GetComponentInChildren<Animator>();
        if (!animator)
            print("animator not found in skin");
        playerAvatarMovement = GetComponent<PlayerAvatarMovement>();
        if (!playerAvatarMovement)
            print("playeravatarmovement script not found");
        rb = GetComponent<Rigidbody>();
        if (!rb)
            print("rigidbody not found");
        playerAvatarMovement.OnJump.AddListener(TriggerJump);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.actions["Throw"].triggered)
        {
            animator.SetTrigger("Throw");
        }

        animator.SetBool("Grounded", playerAvatarMovement.IsGrounded());
        Vector3 vel = rb.velocity;
        Vector2 velV2 = new Vector2(vel.x, vel.z);
        float magnitude = velV2.magnitude;
        animator.SetFloat("Speed", magnitude);

        float yRot = Mathf.Atan2(velV2.x, velV2.y) * Mathf.Rad2Deg;
        if (magnitude > 0.1)
            avatar.character.transform.eulerAngles = new Vector3(0, yRot, 0);

        //sets the speed to > 0 for a smooth transition to idle since the animation doesn't continue with the speed being 0
        AnimatorClipInfo[] currentClips = animator.GetCurrentAnimatorClipInfo(0);
        foreach (AnimatorClipInfo clip in currentClips)
        {
            if (clip.clip.name == "Walk" && magnitude < 0.1f)
            {
                animator.SetFloat("Speed", .05f);
            }
        }

        if (holdingBox)
        {
            int layerIndex = animator.GetLayerIndex("Hold Box Layer");
            animator.SetLayerWeight(layerIndex, 1);
        }
        else
        {
            int layerIndex = animator.GetLayerIndex("Hold Box Layer");
            animator.SetLayerWeight(layerIndex, 0);
        }
    }

    void TriggerJump()
    {
        print("jumpanim");
        animator.SetTrigger("Jump");
    }
}
