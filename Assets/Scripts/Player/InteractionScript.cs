using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionScript : MonoBehaviour
{
    private Interactable interactable;
    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        // CHANGE TO GetComponentInParent ONCE PLAYER HIERARCHY IS GOOD
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.actions["Interact"].triggered)
        {
            if (interactable != null)
            {
                interactable.OnInteract?.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Interactable newInteractable))
        {
            interactable = newInteractable;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Interactable newInteractable) && newInteractable == interactable)
        {
            interactable = null;
        }
    }
}
