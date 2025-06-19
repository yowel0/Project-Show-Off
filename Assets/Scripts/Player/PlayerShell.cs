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
    public GameObject PlayerAvatarPrefab;
    public GameObject CharacterSelectionPrefab;

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

    public void SpawnAvatar(Vector3 position, PlayerChild playerChild){
        Rigidbody rb = GetComponentInChildren<Rigidbody>();
        if (rb)
            rb.velocity = Vector3.zero;
        switch (playerChild)
        {
            case PlayerChild.PlayerAvatar:
                if (GetComponentInChildren<CharacterSelection>())
                {
                    DestroyChildren();

                    GameObject playerAvatar = Instantiate(PlayerAvatarPrefab, position, quaternion.identity, transform);
                    Avatar avatar = playerAvatar.GetComponentInChildren<Avatar>();
                    avatar.SetCharacter(characterPrefab);
                    avatar.SetHat(hatPrefab);
                    //Instantiate(hat, _PlayerAvatar.transform);
                }
                else
                {
                    TeleportAvatar(position);
                }
                break;
            case PlayerChild.CharacterSelection:
                if (GetComponentInChildren<Avatar>())
                {
                    DestroyChildren();

                    GameObject characterSelection = Instantiate(CharacterSelectionPrefab, position, quaternion.identity, transform);
                    Avatar avatar = characterSelection.GetComponentInChildren<Avatar>();
                    if(characterPrefab)
                        avatar.SetCharacter(characterPrefab);
                    if(hatPrefab)
                        avatar.SetHat(hatPrefab);
                }
                else
                {
                    CharacterSelection characterSelection = GetComponentInChildren<CharacterSelection>();
                    characterSelection.transform.position = position;
                }
                break;
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
