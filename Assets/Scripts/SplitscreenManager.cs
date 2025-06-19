using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SplitscreenManager : MonoBehaviour
{
    private SplitscreenManager Instance;
    [SerializeField]
    private bool enableOnStart = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        if(enableOnStart)
            Enable();
        if (PlayerManager.Instance != null)
        {
            PlayerInputManager playerInputManager = PlayerManager.Instance.GetComponent<PlayerInputManager>();
            playerInputManager.onPlayerJoined += PlayerJoins;
            print("playerjoins event added");
        }
        else
        {
            print("PLAYER MANAGER NOT FOUND");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Enable()
    {
        if (!PlayerManager.Instance)
            return;
        //disable main camera
        Camera.main.enabled = false;
        //enable player cameras
        SetCamViewAllPlayers();
    }

    void Disable()
    {
        //enable main camera
        Camera.main.enabled = true;
        //disable player cameras
        DisableCamAllPlayers();
    }

    void PlayerJoins(PlayerInput playerInput)
    {
        print("playerjoins");
        SetCamViewAllPlayers();
    }

    void SetCamViewAllPlayers() {
        List<PlayerShell> players = PlayerManager.Instance.players;
        for (int i = 0; i < players.Count; i++) {
            PlayerShell player = players[i];
            Camera playerCam = player.GetComponentInChildren<Camera>();
            playerCam.enabled = true;
            SetCamViewpointRect(playerCam,i,players.Count);
        }
    }

    void DisableCamAllPlayers()
    {
        List<PlayerShell> players = PlayerManager.Instance.players;
        foreach (PlayerShell player in players)
        {
            Camera playerCam = player.GetComponentInChildren<Camera>();
            playerCam.enabled = false;
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
