using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private PlayerInputManager playerInputManager;
    public List<PlayerShell> players = new List<PlayerShell>();
    public List<Transform> playerPositions = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += AddPlayer;
        DontDestroyOnLoad(gameObject);
        //add all already active players (for debug)
        PlayerInput[] activePlayers = FindObjectsOfType<PlayerInput>();
        foreach (PlayerInput playerInput in activePlayers){
            AddPlayer(playerInput);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
 
    public int GetPlayerCount()
    {
        return players.Count;
    }
    public int GetPlayerID(PlayerShell pPlayerShell)
    {
        return players.IndexOf(pPlayerShell);
    }

    void AddPlayer(PlayerInput playerInput){
        print("hi");
        DontDestroyOnLoad(playerInput.gameObject);
        PlayerShell player = playerInput.GetComponent<PlayerShell>();
        players.Add(player);
        if (playerPositions.Count >= players.Count)
            player.transform.position = playerPositions[players.Count - 1].position;
    }
}
