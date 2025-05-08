using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public enum ControlScheme{
    Menu,
    Movement
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
    public GameObject skin;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchControlScheme(ControlScheme newControlScheme){
        controlScheme = newControlScheme;
        switch (controlScheme){
            case ControlScheme.Menu:
                playerInput.SwitchCurrentActionMap("Menu");
                break;
            case ControlScheme.Movement:
                playerInput.SwitchCurrentActionMap("Movement");
                break;
        };
    }

    public void SpawnAvatar(Vector3 position){
        DestroyChildren();
        GameObject _PlayerAvatar = Instantiate(PlayerAvatar, position, quaternion.identity, transform);
        Instantiate(skin, _PlayerAvatar.transform);
    }
    
    void DestroyChildren(){
        for (int i = transform.childCount - 1; i >= 0; i--){
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
