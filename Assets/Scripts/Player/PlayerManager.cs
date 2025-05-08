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
        players.Add(playerInput.GetComponent<PlayerShell>());
    }

    public void SpawnPlayers(Vector3 pos1, Vector3 pos2){
        for (int i = 0; i < players.Count; i++){
                PlayerShell player = players[i];
                
                switch (i){
                    case 0:
                        player.SpawnAvatar(pos1);
                        break;
                    case 1:
                        player.SpawnAvatar(pos2);
                        break;
                }
            }
    } 
}
