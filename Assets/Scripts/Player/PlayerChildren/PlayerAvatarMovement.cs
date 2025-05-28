using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;

public class PlayerAvatarMovement : MonoBehaviour
{

    [Header("Moving")]
    [SerializeField] float moveForce = 6000;

    [Range(0f, 100f)]
    [Tooltip("What is the horizontal speed limit for the player?")]
    [SerializeField] float maxSpeed;

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

    [Tooltip("Is the player able to hold the jump button to jump as soon as they hit the ground?")]
    [SerializeField] bool canHoldJump;

    [Range(0f, 1f)] 
    [Tooltip("How far away from the ground can the player jump?")]
    [SerializeField] float debugRayLength;


    private float extraRaycastLength = 0.121f;
    private float originalDrag;
    private float originalMoveForce;
    private float timeSinceBounce;
    private PlayerInput playerInput;
    private Rigidbody rb;
    //public for animation
    public bool grounded;
    //On Jump event for animation
    public UnityEvent OnJump = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.drag;
        originalMoveForce = moveForce;
        timeSinceBounce = dragLossDuration;

        int playerID = PlayerManager.Instance.GetPlayerID(GetComponentInParent<PlayerShell>());
        Image image = GetComponentInChildren<Image>();
        switch (playerID)
        {
            case 0:
                image.color = Color.red;
                break;
            case 1:
                image.color = Color.blue;
                break;
            case 2:
                image.color = Color.green;
                break;
            case 3:
                image.color = Color.yellow;
                break;
        }

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
        if (!grounded)
        {
            Debug.Log(rb.velocity.y);
        }

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
            rb.drag = dragAfterBounce.Evaluate(percent) * originalDrag;
            moveForce = dragAfterBounce.Evaluate(percent) * originalMoveForce;
        }
    }

    private float Jump()
    {
        OnJump?.Invoke();
        if (MusicManager.Instance)
        {
            MusicManager.Instance.PlaySound(jumpSound);
        }
        return injectedJumpVelocity;
    }

    private void CheckGrounded()
    {
        if (Physics.Raycast(transform.position + Vector3.up * extraRaycastLength, -transform.up, out var hit, debugRayLength + extraRaycastLength) && !hit.collider.isTrigger)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        Debug.DrawRay(transform.position + Vector3.up * extraRaycastLength, -transform.up * (debugRayLength + extraRaycastLength), Color.red,.1f);
    }
    public bool IsGrounded()
    {
        return grounded;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            timeSinceBounce = 0;
        }
        if (collision.gameObject.CompareTag("Pillow"))
        {
            timeSinceBounce = 0;
            Vector3 pillowUp = collision.transform.up * pillowBounce;
            Debug.Log(pillowUp);
            rb.velocity += pillowUp;
            //rb.velocity = new Vector3(rb.velocity.x, pillowBounce, rb.velocity.z);
        }
    }
}
