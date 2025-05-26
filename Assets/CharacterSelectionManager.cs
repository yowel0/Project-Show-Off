using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    private PlayerManager playerManager;
    [SerializeField]
    private string nextSceneName;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        int readyPlayers = 0;
        foreach(var player in playerManager.players){
            if (player.GetComponentInChildren<CharacterSelection>().editingComponent == CharacterSelection.component.Ready){
                readyPlayers++;
            }
        }
        if (readyPlayers == playerManager.players.Count && readyPlayers > 0){
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
