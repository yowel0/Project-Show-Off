using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerChild
{
    PlayerAvatar,
    CharacterSelection
}

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    bool spawnOnAwake = true;
 
    [Tooltip("Movement abilities for this scene")]
    public ControlScheme controlScheme = ControlScheme.Movement;

    [SerializeField]
    private PlayerChild playerChild = PlayerChild.PlayerAvatar;
    [SerializeField]
    Transform[] spawnPositions;
    [SerializeField]
    float yRotation = 180;

    // Start is called before the first frame update
    void Awake()
    {
        if (spawnOnAwake)
            SpawnAvatars();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnAvatars()
    {
        //after spawning Avatars, no more new players allowed
        PlayerManager playerManager = FindAnyObjectByType<PlayerManager>();
        //PlayerManager.Instance
        playerManager.GetComponent<PlayerInputManager>().joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        if (PlayerManager.Instance != null)
        {
            for (int i = 0; i < PlayerManager.Instance.GetPlayerCount(); i++)
            {
                PlayerShell player = PlayerManager.Instance.players[i];
                player.SwitchControlScheme(controlScheme);
                if (spawnPositions.Length >= 4 && PlayerManager.Instance.GetPlayerCount() <= 2)
                {
                    Vector3 spawnPosition = spawnPositions[i + 1].position;
                    player.SpawnAvatar(spawnPosition, playerChild, yRotation);
                }
                else if (spawnPositions[i] != null)
                {
                    Vector3 spawnPosition = spawnPositions[i].position;
                    player.SpawnAvatar(spawnPosition, playerChild, yRotation);
                }
                else
                {
                    player.SpawnAvatar(Vector3.zero, playerChild, yRotation);
                }
            }
        }
        else
        {
            Debug.LogError("No player managaer found :(");
        }
    }
}
