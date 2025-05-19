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
        SpawnAvatars();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnAvatars(){
        //after spawning Avatars, no more new players allowed
        //PlayerManager playerManager = FindAnyObjectByType<PlayerManager>();
        //PlayerManager.Instance
        //playerManager.GetComponent<PlayerInputManager>().joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        if (PlayerManager.Instance != null){
            for (int i = 0; i < PlayerManager.Instance.GetPlayerCount(); i++){
                PlayerShell player = PlayerManager.Instance.players[i];
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
}
