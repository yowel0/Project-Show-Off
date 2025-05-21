using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerAvatarMovement : MonoBehaviour
{

    [Tooltip("Is the player able to hold the jump button to jump as soon as they hit the ground?")]
    [SerializeField] bool canHoldJump;

    [Range(0f, 1f)]
    [SerializeField] float debugRayLength;

    public float moveSpeed = 10;

    [Range(0f, 20f)]
    [Tooltip("Starting y velocity when jumping")]
    [SerializeField] float injectedJumpVelocity;

    [Range(0f, 1f)]
    [Tooltip("How much the players velocity is reduced once you let go of the jump button")]
    [SerializeField] float shortJumpMult;

    [Range(-1f, 3f)]
    [Tooltip("How much g to add to the current gravity (0 = normal, -1 = no gravity, 1 = double gravity")]
    [SerializeField] float gravityMult;

    [Range(0f, 100f)]
    [Tooltip("How much g to add to the current gravity (0 = normal, -1 = no gravity, 1 = double gravity")]
    [SerializeField] float maxSpeed;

    private float extraRaycastLength = 0.121f;
    private PlayerInput playerInput;
    private PlayerShell playerShell;
    private Rigidbody rb;
    private bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerShell = GetComponentInParent<PlayerShell>();
        rb = GetComponent<Rigidbody>();
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
        rb.AddForce(new Vector3(moveInput.x, 0, moveInput.y) * Time.deltaTime * moveSpeed);

        CheckGrounded();

        rb.velocity = LimitVelocityAndJump();
    }

    private void FixedUpdate()
    {
        // Add additional gravity
        Vector3 gravity = Physics.gravity * rb.mass;
        rb.AddForce(gravity * gravityMult);
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
        maxHorVel.y = isJumping && grounded ? injectedJumpVelocity : rb.velocity.y;
        if (jump.WasReleasedThisFrame() && rb.velocity.y > 0)
        {
            maxHorVel.y = rb.velocity.y * shortJumpMult;
        }

        return maxHorVel;
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
}
