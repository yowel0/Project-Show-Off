using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    [Header("Sounds")]
    [Tooltip("Plays when you pause the game")]
    [SerializeField] SoundObject pauseSound;
    [Tooltip("Plays when you click on a button")]
    [SerializeField] SoundObject clickSound;

    [Header("Scene names")]
    [SerializeField] string mainMenuName;
    [SerializeField] string hubName;

    [Header("Debug")]
    [SerializeField] bool isPaused;
    [SerializeField] ControlScheme savedControlScheme;

    public void Pause()
    {
        Debug.Log("Pause");
        if (isPaused)
        {
            Debug.Log("Already paused though");
            if (gameObject.activeSelf) Continue();
            return;
        }
        MusicManager.Instance?.PlaySound(pauseSound);
        gameObject.SetActive(true);
        isPaused = true;
        savedControlScheme = (ControlScheme)(PlayerManager.Instance?.players[0].controlScheme);
        PlayerManager.Instance?.SetControlScheme(ControlScheme.Menu);

        Time.timeScale = 0;
    }

    public void Continue()
    {
        Debug.Log("Doing continue");
        PlayerManager.Instance?.SetControlScheme(savedControlScheme);
        gameObject.SetActive(false);
        isPaused = false;

        Time.timeScale = 1;
    }

    public void ReturnToHub()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.ChangeScene(hubName);
        }
        else SceneManager.LoadScene(hubName);
        Destroy(ScoreTransfer.Instance?.gameObject);
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        foreach(PlayerShell p in PlayerManager.Instance.players)
        {
            Destroy(p.gameObject);
        }
        Destroy(PlayerManager.Instance.gameObject);
        Destroy(SceneTransition.Instance?.gameObject);
        Destroy(ScoreTransfer.Instance?.gameObject);

        Time.timeScale = 1;

        SceneManager.LoadScene(mainMenuName);
    }


    public void PlayClickSound()
    {
        MusicManager.Instance?.PlaySound(clickSound);
    }
}
