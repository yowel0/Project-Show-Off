using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerAvatarMovement : MonoBehaviour
{
    public float jumpForce = 10;
    public float moveSpeed = 10;

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
        if (Physics.Raycast(transform.position, -transform.up, out var hit, 0.4f) && !hit.collider.isTrigger)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        if (playerInput.actions["Jump"].triggered && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce);
        }
        Debug.DrawRay(transform.position, -transform.up * 0.4f, Color.red,.1f);
    }
}
