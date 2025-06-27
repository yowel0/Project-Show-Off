using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    [SerializeField] PauseScreen pauseScreen;

    public static PauseManager Instance;
    void Start()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }


    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }



    public void PauseGame()
    {
        pauseScreen.Pause();
    }

}
