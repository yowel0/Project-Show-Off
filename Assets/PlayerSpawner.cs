using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    Transform[] spawnPositions;
    // Start is called before the first frame update
    void Start()
    {
        PlayerManager playerManager = FindAnyObjectByType<PlayerManager>();
        if (playerManager != null){
            for (int i = 0; i < playerManager.players.Count; i++){
                PlayerShell player = playerManager.players[i];
                if (spawnPositions[i] != null){
                    Vector3 spawnPosition = spawnPositions[i].position;
                    player.SpawnAvatar(spawnPosition);
                }
                else{
                    player.SpawnAvatar(Vector3.zero);
                }
            }
        }
        else{
            print("No player managaer found :(");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
