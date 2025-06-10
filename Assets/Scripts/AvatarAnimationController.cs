using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAnimationController : MonoBehaviour
{
    Animator animator;
    PlayerAvatarMovement playerAvatarMovement;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
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
        animator.SetBool("Grounded", playerAvatarMovement.IsGrounded());
        Vector3 vel = rb.velocity;
        Vector2 velV2 = new Vector2(vel.x, vel.z);
        animator.SetFloat("Speed", velV2.magnitude);
    }

    void TriggerJump()
    {
        print("jumpanim");
        animator.SetTrigger("Jump");
    }
}
