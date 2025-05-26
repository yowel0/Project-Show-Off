using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] SoundObject enterMinigameSFX;

    [Tooltip("Name of the scene for the minigame this portal leads to")]
    [SerializeField] string sceneName;

    [Tooltip("This is the popup that shows where the portal leads to")]
    [SerializeField] GameObject gameInfoPopup;

    [SerializeField] int playersNear = 0;


    public void Interact()
    {
        if (gameInfoPopup.activeSelf)
        {
            MusicManager.Instance.PlaySound(enterMinigameSFX);
            SceneManager.LoadScene(sceneName);
        }
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
