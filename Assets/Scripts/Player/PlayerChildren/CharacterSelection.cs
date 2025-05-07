using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelection : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerShell playerShell;
    private Vector2 move;
    private Vector2 oldMove;

    public List<GameObject> characters;
    public int selectedCharacter;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerShell = GetComponentInParent<PlayerShell>();
    }

    // Update is called once per frame
    void Update()
    {
        move = playerInput.actions["Move"].ReadValue<Vector2>();

        //print(move);
        char lastChar = playerShell.userName[playerShell.userName.Length - 1];
        if (playerShell.userName.Length > 0){
            //print(lastChar);
        }

        oldMove = move;
    }
}
