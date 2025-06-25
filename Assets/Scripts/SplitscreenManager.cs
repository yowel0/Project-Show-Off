using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SplitscreenManager : MonoBehaviour
{
    [SerializeField]
    private bool enableOnStart = false;

    // Start is called before the first frame update
    void Start()
    {
        
        if (PlayerManager.Instance != null)
        {
            PlayerInputManager playerInputManager = PlayerManager.Instance.GetComponent<PlayerInputManager>();
            playerInputManager.onPlayerJoined += PlayerJoins;
            SceneManager.sceneUnloaded += SceneUnloaded;
        }
        else
        {
            Debug.LogError("PLAYER MANAGER NOT FOUND");
        }
        if (enableOnStart)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }

    void OnDisable()
    {
        PlayerInputManager playerInputManager = PlayerManager.Instance?.GetComponent<PlayerInputManager>();
        if (playerInputManager != null) playerInputManager.onPlayerJoined -= PlayerJoins;
        Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Enable()
    {
        //print("enable splitscreen");
        if (!PlayerManager.Instance)
            return;
        //disable main camera
        Camera.main.enabled = false;
        //enable player cameras
        SetCamViewAllPlayers();

    }

    void SceneUnloaded(Scene scene)
    {
        //Disable();
        SceneManager.sceneUnloaded -= SceneUnloaded;
    }

    void Disable()
    {
        //enable main camera
        if (Camera.main)
            Camera.main.enabled = true;
        //disable player cameras
        DisableCamAllPlayers();
    }

    void PlayerJoins(PlayerInput playerInput)
    {
        SetCamViewAllPlayers();
    }

    void SetCamViewAllPlayers() 
    {
        List<PlayerShell> players = PlayerManager.Instance.players;
        for (int i = 0; i < players.Count; i++)
        {
            PlayerShell player = players[i];
            // Camera playerCam = player.GetComponentInChildren<Camera>();
            // playerCam.enabled = true;
            
            player.SplitscreenCamera.enabled = true;
            SetCamViewpointRect(player.SplitscreenCamera, i, players.Count);
        }
    }

    void DisableCamAllPlayers()
    {
        List<PlayerShell> players = PlayerManager.Instance?.players;
        if (players == null) return;
        foreach (PlayerShell player in players)
        {
            // Camera playerCam = player.GetComponentInChildren<Camera>();
            // playerCam.enabled = false;
            if (player != null) player.SplitscreenCamera.enabled = false;
        }
    }

    void SetCamViewpointRect(Camera camera, int playerIndex, int playerCount)
    {
        Rect rect = camera.rect;
        if (playerCount == 1)
        { //1
            rect.width = 1f;
            rect.height = 1f;
        }
        else if (playerCount > 1)
        { //2-4
            rect.width = .5f;
            rect.height = 1f;
            if (playerCount > 2)
            { //3-4
                rect.height = .5f;
                if (playerCount == 3 && playerIndex == 2) {
                    rect.width = 1.0f;
                }
            }
        }
        rect.x = playerIndex % 2 * 0.5f;
        if(playerCount > 2)
            rect.y = (int)(playerIndex / 2.0f) / 2.0f + 0.5f - (int)(playerIndex / 2.0f);
        camera.rect = rect;
    }
}
