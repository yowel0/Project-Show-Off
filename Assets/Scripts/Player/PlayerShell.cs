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
    NoMove
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
    public GameObject hat;

    // Start is called before the first frame update
    void Start()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();
        SwitchControlScheme(controlScheme);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
        ;
    }

    public void SpawnAvatar(Vector3 position){
        if (GetComponentInChildren<CharacterSelection>() != null)
        {
            DestroyChildren();

            GameObject _PlayerAvatar = Instantiate(PlayerAvatar, position, quaternion.identity, transform);
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
