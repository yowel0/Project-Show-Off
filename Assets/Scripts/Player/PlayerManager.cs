using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    public List<PlayerShell> players = new List<PlayerShell>();
    public List<Transform> playerPositions = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += AddPlayer;
        DontDestroyOnLoad(gameObject);
        //add all already active players (for debug)
        PlayerInput[] activePlayers = FindObjectsOfType<PlayerInput>();
        foreach (PlayerInput playerInput in activePlayers){
            AddPlayer(playerInput);
        }
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
        DontDestroyOnLoad(playerInput.gameObject);
        PlayerShell player = playerInput.GetComponent<PlayerShell>();
        players.Add(player);
        if (playerPositions.Count >= players.Count)
            player.transform.position = playerPositions[players.Count - 1].position;
    }
}
