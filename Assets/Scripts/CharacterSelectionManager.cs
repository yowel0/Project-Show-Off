 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    private PlayerManager playerManager;
    [SerializeField] SoundObject namesConfirmedSound;
    //[SerializeField]
    //private string nextSceneName;
    private bool transitioned = false;
    [SerializeField]
    UnityEvent OnEveryoneReady;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (playerManager == null)
        {
            playerManager = FindAnyObjectByType<PlayerManager>();
            return;
        }
        int readyPlayers = 0;
        foreach(var player in playerManager.players){
            if (player.GetComponentInChildren<CharacterSelection>().editingComponent == CharacterSelection.Component.Ready){
                readyPlayers++;
            }
        }
        if (readyPlayers == playerManager.players.Count && readyPlayers > 0)
        {
            if (!transitioned)
            {
                MusicManager.Instance?.PlaySound(namesConfirmedSound);
                // SceneTransition.Instance.ChangeScene(nextSceneName);
                OnEveryoneReady?.Invoke();
                transitioned = true;
            }
        }
    }
}
