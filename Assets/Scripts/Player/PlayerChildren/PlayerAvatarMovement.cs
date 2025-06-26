using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerSoundManager))]
public class PlayerAvatarMovement : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] float moveForce = 6000;

    [Range(0f, 100f)]
    [Tooltip("What is the horizontal speed limit for the player?")]
    [SerializeField] float maxSpeed;


    [Header("Bump")]
    [Tooltip("In the formula y = ax + b, where: \ny is total force added after a bump, \nthis is b")]
    [SerializeField] float constantForceBump;
    [Tooltip("In the formula y = ax + b, where: \ny is total force added after a bump, \nthis is a")]
    [SerializeField] float multForceBump;

    [Tooltip("This graph represents how much drag and move force the player has after bouncing")]
    [SerializeField] AnimationCurve dragAfterBounce;

    [Tooltip("How long does the drag loss after a bounce take? \n Directly involved with curve above")]
    [SerializeField] float dragLossDuration;


    [Header("Jump values")]
    [Range(0f, 75f)]
    [Tooltip("Starting y velocity when jumping")]
    [SerializeField] float injectedJumpVelocity;

    [Range(0f, 1f)]
    [Tooltip("How much the players velocity is reduced once you let go of the jump button")]
    [SerializeField] float shortJumpMult;

    [Range(-1f, 20f)]
    [Tooltip("How much g to add to the current gravity (0 = normal, -1 = no gravity, 1 = double gravity")]
    [SerializeField] float gravityMult;

    [Tooltip("How fast the pillow launches the player upon contact")]
    [SerializeField] float pillowBounce;


    [Header("Jump QOL")]
    [Tooltip("Jumping sound")]
    [SerializeField] SoundObject jumpSound;

    [Tooltip("Particle effect that plays when launched by pillow")]
    [SerializeField] GameObject pillowBounceParticle;

    [Tooltip("Is the player able to hold the jump button to jump as soon as they hit the ground?")]
    [SerializeField] bool canHoldJump;

    [Range(0f, 1f)] 
    [Tooltip("How far away from the ground can the player jump?")]
    [SerializeField] float debugRayLength;


    private float extraRaycastLength = 0.2f;  // 0.121f
    private float originalDrag;
    private float originalMoveForce;
    private float timeSinceBounce;
    private PlayerInput playerInput;
    private PlayerSoundManager soundManager;
    private Rigidbody rb;
    private bool grounded;
    private bool wasGrounded = false;

    [SerializeField] string groundTag;

    [Header("Events for VFX")]
    public UnityEvent OnJump;
    public UnityEvent OnLand;
    public UnityEvent OnBump;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        soundManager = GetComponent<PlayerSoundManager>();
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.drag;
        originalMoveForce = moveForce;
        timeSinceBounce = dragLossDuration;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        rb.AddForce(new Vector3(moveInput.x, 0, moveInput.y) * Time.deltaTime * moveForce);

        CheckGrounded();

        rb.velocity = LimitVelocityAndJump();
    }

    private void FixedUpdate()
    {
        // Add additional gravity
        Vector3 gravity = Physics.gravity * rb.mass;
        rb.AddForce(gravity * gravityMult);

        CheckBouncePhysics();
    }

    private Vector3 LimitVelocityAndJump()
    {
        // Get type of jumping
        InputAction jump = playerInput.actions["Jump"];
        bool isJumping = canHoldJump ? jump.IsPressed() : jump.triggered;

        // Limit horizontal speed
        Vector3 maxHorVel = rb.velocity;
        maxHorVel.y = 0;
        if (maxHorVel.magnitude > maxSpeed) maxHorVel = maxHorVel.normalized * maxSpeed;

        // Jumping
        maxHorVel.y = isJumping && grounded ? Jump() : rb.velocity.y;
        if (jump.WasReleasedThisFrame() && rb.velocity.y > 0)
        {
            maxHorVel.y = rb.velocity.y * shortJumpMult;
        }

        return maxHorVel;
    }

    private void CheckBouncePhysics()
    {

        if (timeSinceBounce < dragLossDuration)
        {
            timeSinceBounce += Time.fixedDeltaTime;
            float percent = Mathf.Min(timeSinceBounce / dragLossDuration, 1);

            if (grounded)
            {
                rb.drag = dragAfterBounce.Evaluate(percent) * originalDrag;
            }
            else
            {
                rb.drag = originalDrag;
            }

            moveForce = dragAfterBounce.Evaluate(percent) * originalMoveForce;
        }
    }

    private float Jump()
    {
        OnJump?.Invoke();
        soundManager.PlayJump();
        return injectedJumpVelocity;
    }

    private void CheckGrounded()
    {
        if (Physics.Raycast(transform.position + Vector3.up * extraRaycastLength, -transform.up, out var hit, debugRayLength + extraRaycastLength) && !hit.collider.isTrigger)
        {
            groundTag = hit.transform.tag;
            grounded = true;
            if (!wasGrounded)
            {
                OnLand?.Invoke();
                soundManager.PlayLand(hit.transform.tag);
                wasGrounded = true;
            }
        }
        else
        {
            grounded = false;
            wasGrounded = false;
        }

        Debug.DrawRay(transform.position + Vector3.up * extraRaycastLength, -transform.up * (debugRayLength + extraRaycastLength), Color.red,.1f);
    }
    public bool IsGrounded()
    {
        return grounded;
    }


    public void DoFootstep()
    {
        soundManager.PlayFootstep(groundTag);
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            timeSinceBounce = 0;
            OnBump?.Invoke();
            soundManager.PlayBump();

            ContactPoint cPoint = collision.GetContact(0);

            Vector3 relVel = collision.relativeVelocity + rb.velocity;
            float outForce = multForceBump * relVel.magnitude + constantForceBump;

            rb.AddForce(cPoint.normal * outForce);

        }
        else if (collision.gameObject.CompareTag("Pillow"))
        {
            soundManager.PlayPillowBounce();
            timeSinceBounce = 0;
            Vector3 pillowUp = collision.transform.up * pillowBounce;
            Debug.Log(pillowUp);
            rb.velocity += pillowUp;

            Instantiate(pillowBounceParticle, transform.position, pillowBounceParticle.transform.rotation);
            //rb.velocity = new Vector3(rb.velocity.x, pillowBounce, rb.velocity.z);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Ice")) timeSinceBounce = 0;
    }
}
