using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    public List<PlayerShell> players = new List<PlayerShell>();
    // Start is called before the first frame update
    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += AddPlayer;
        DontDestroyOnLoad(gameObject);
    }

    // void OnEnable()
    // {
    //     if (!playerInputManager)
    //     playerInputManager = GetComponent<PlayerInputManager>();
    // }

    // void OnDisable()
    // {
    //     playerInputManager.onPlayerJoined -= AddPlayer;
    // }

    void AddPlayer(PlayerInput playerInput){
        print("hi");
        players.Add(playerInput.GetComponent<PlayerShell>());
    }
}
