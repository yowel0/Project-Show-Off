using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PopUp : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] UnityEvent OnCountDownDone;
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] TextMeshProUGUI minigameNameText;

    [Tooltip("Reference to inner hitbox")]
    [SerializeField] PlayerProximity playerProximity;
    [SerializeField] GameObject[] readyIcons;
    [SerializeField] GameObject[] readyIconOutlines;

    [Tooltip("Total time to count down")]
    [SerializeField] float countdownTime;
    [Tooltip("The value that ticks down")]
    [SerializeField] float countdownTimer;
    [SerializeField] bool isCountingDown;

    private void OnEnable()
    {
        int pCount = (int)PlayerManager.Instance?.GetPlayerCount();
        for (int i = 0; i < readyIconOutlines.Length; i++)
        {
            readyIconOutlines[i].SetActive(i < pCount);
        }
    }

    public void StartCountdown()
    {
        isCountingDown = true;
        countdownTimer = countdownTime;
        countdownText.gameObject.SetActive(true);
        minigameNameText.gameObject.SetActive(false);
    }

    public void StopCountdown()
    {
        isCountingDown = false;
        countdownText.gameObject.SetActive(false);
        minigameNameText.gameObject.SetActive(true);
    }


    public void CheckReady()
    {
        for (int i = 0; i < readyIcons.Length; i++)
        {
            readyIcons[i].gameObject.SetActive(playerProximity.NearbyPlayers[i]);
        }
    }

    private void FixedUpdate()
    {
        if (!isCountingDown) return;

        countdownTimer -= Time.fixedDeltaTime;

        countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();

        if (countdownTimer <= 0)
        {
            isCountingDown = false;
            //SceneTransition.Instance.ChangeScene(sceneName);
            OnCountDownDone?.Invoke();
        }
    }

}
