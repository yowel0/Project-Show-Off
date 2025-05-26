using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public enum ControlScheme
{
    Menu,
    Movement,
    NoMove,
    NoJump,
    OnlyInteract
};

public class PlayerShell : MonoBehaviour
{
    [Header("Player Children")]
    public GameObject PlayerAvatar;

    [Header("Input")]
    public ControlScheme controlScheme = ControlScheme.Menu;
    private PlayerInput playerInput;

    [Header("User Info")]
    public string userName;
    public GameObject hatPrefab;
    public GameObject characterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();
        SwitchControlScheme(controlScheme);
    }

    public void SwitchControlScheme(ControlScheme newControlScheme){
        controlScheme = newControlScheme;
        if (playerInput == null){
            playerInput = GetComponent<PlayerInput>();
        }
        switch (controlScheme)
        {
            case ControlScheme.Menu:
                playerInput.SwitchCurrentActionMap("Menu");
                break;
            case ControlScheme.Movement:
                playerInput.SwitchCurrentActionMap("Movement");
                break;
            case ControlScheme.NoMove:
                playerInput.SwitchCurrentActionMap("NoMove");
                break;
            case ControlScheme.NoJump:
                playerInput.SwitchCurrentActionMap("NoJump");
                break;
            case ControlScheme.OnlyInteract:
                playerInput.SwitchCurrentActionMap("OnlyInteract");
                break;
        }
        ;
    }

    public void SpawnAvatar(Vector3 position){
        Rigidbody rb = GetComponentInChildren<Rigidbody>();
        if (rb)
            rb.velocity = Vector3.zero;
        if (GetComponentInChildren<CharacterSelection>() != null)
        {
            DestroyChildren();

            GameObject _PlayerAvatar = Instantiate(PlayerAvatar, position, quaternion.identity, transform);
            Avatar avatar = _PlayerAvatar.GetComponentInChildren<Avatar>();
            avatar.SetCharacter(characterPrefab);
            avatar.SetHat(hatPrefab);
            //Instantiate(hat, _PlayerAvatar.transform);
        }
        else
        {
            TeleportAvatar(position);
        }
    }

    public void TeleportAvatar(Vector3 position){
        PlayerAvatarMovement playerAvatarMovement = GetComponentInChildren<PlayerAvatarMovement>();
        if(playerAvatarMovement != null){
            playerAvatarMovement.transform.position = position;
        }
    }
    
    void DestroyChildren(){
        for (int i = transform.childCount - 1; i >= 0; i--){
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
