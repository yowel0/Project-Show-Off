using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitscreenManager : MonoBehaviour
{
    private SplitscreenManager Instance;

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
        Enable();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Enable()
    {
        if (!PlayerManager.Instance)
            return;
        List<PlayerShell> players = PlayerManager.Instance.players;
        //disable main camera
        Camera.main.enabled = false;
        //enable player cameras
        for (int i = 0; i < players.Count; i++) {
            PlayerShell player = players[i];
            Camera playerCam = player.GetComponentInChildren<Camera>();
            playerCam.enabled = true;
            SetCamViewpointRect(playerCam,i,players.Count);
        }
    }

    void Disable()
    {

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
            }
        }
        rect.x = playerIndex % 2 * 0.5f;
        rect.y = (int)(playerIndex / 2.0f) / 2.0f + 0.5f - (int)(playerIndex / 2.0f);
        camera.rect = rect;
    }
}
