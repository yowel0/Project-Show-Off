using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Tooltip("Name of the scene for the minigame this portal leads to")]
    [SerializeField] string sceneName;

    [Tooltip("This is the popup that shows where the portal leads to")]
    [SerializeField] GameObject gameInfoPopup;

    // REPLACE THIS WITH PLAYERMANAGER PLAYERS TOTAL
    public int totalPlayers = 1;

    [SerializeField] private int playersNear = 0;


    public void Interact()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playersNear++;
            CheckPopup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playersNear--;
            CheckPopup();
        }
    }

    private void CheckPopup()
    {
        // If majority of players is near
        if (playersNear*2 > PlayerManager.Instance.GetPlayerCount())
        {
            gameInfoPopup.SetActive(true);
        }
        else
        {
            gameInfoPopup.SetActive(false);
        }
    }
}
