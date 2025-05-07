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
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    void AddPlayer(PlayerInput playerInput){
        players.Add(playerInput.GetComponent<PlayerShell>());
    }
}
