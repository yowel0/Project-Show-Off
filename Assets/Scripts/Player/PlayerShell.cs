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
    public Camera SplitscreenCamera;

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
        SplitscreenCamera = gameObject.GetComponentInChildren<Camera>();
    }

    public void SwitchControlScheme(ControlScheme newControlScheme)
    {
        controlScheme = newControlScheme;
        if (playerInput == null)
        {
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
    }

    public void SpawnAvatar(Vector3 position, PlayerChild playerChild)
    {
        Rigidbody rb = GetComponentInChildren<Rigidbody>();
        if (rb)
            rb.velocity = Vector3.zero;
        GameObject playerChildObj = null;
        switch (playerChild)
        {
            case PlayerChild.PlayerAvatar:
                if (GetComponentInChildren<CharacterSelection>())
                {
                    DestroyChildren();

                    playerChildObj = Instantiate(PlayerAvatarPrefab, position, quaternion.identity, transform);
                    SetAvatarValues(playerChildObj);
                SplitscreenCamera = playerChildObj.transform.GetComponentInChildren<Camera>();
                }
                else
                {
                    TeleportAvatar(position);
                }
                break;
            case PlayerChild.CharacterSelection:
                if (GetComponentInChildren<PlayerAvatarMovement>())
                {
                    DestroyChildren();

                    playerChildObj = Instantiate(CharacterSelectionPrefab, position, quaternion.identity, transform);
                    SetAvatarValues(playerChildObj);
                    SplitscreenCamera = playerChildObj.transform.GetComponentInChildren<Camera>();
                }
                else
                {
                    CharacterSelection characterSelection = GetComponentInChildren<CharacterSelection>();
                    characterSelection.transform.position = position;
                }
                break;
        }
        if (playerChildObj != null)
        {
            SplitscreenCamera = playerChildObj.transform.GetComponentInChildren<Camera>();
            print("new splitsccreencam");
        }
    }

    public void TeleportAvatar(Vector3 position)
    {
        PlayerAvatarMovement playerAvatarMovement = GetComponentInChildren<PlayerAvatarMovement>();
        if (playerAvatarMovement != null)
        {
            playerAvatarMovement.transform.position = position;
        }
    }

    void DestroyChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void SetAvatarValues(GameObject playerChildObj)
    {
        Avatar avatar = playerChildObj.GetComponentInChildren<Avatar>();
        if (characterPrefab)
            avatar.SetCharacter(characterPrefab);
        if (hatPrefab)
            avatar.SetHat(hatPrefab);
        avatar.character.transform.eulerAngles = new Vector3(0, 180, 0);
    }
}
