using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PodiumPlace : MonoBehaviour
{
    [Header("Debug")]
    [Tooltip("Is this platform supposed to go up?")]
    [SerializeField] bool isRaising;
    [Tooltip("How high does first place go?")]
    [SerializeField] float raiseHeight;
    [Tooltip("How long does it take for first place to reach the top?")]
    [SerializeField] float riseDuration;
    [Tooltip("How long has this place been rising for?")]
    [SerializeField] float timeRising;

    [SerializeField] int placement;
    [SerializeField] int score;

    [SerializeField] TextMeshProUGUI scoreText;

    Rigidbody rb;
    Vector3 startPos;
    Vector3 endPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (isRaising)
        {
            // Calculate how high to go
            float risePercent = Mathf.Min(timeRising / riseDuration, 1 - .25f * placement);
            Vector3 newPos = Vector3.Lerp(startPos, endPos, risePercent);

            // Move
            rb.MovePosition(newPos);

            // Check if the final position is reached
            if (timeRising / riseDuration >= 1 - .25f * placement)
            {
                isRaising = false;
                
                if (scoreText != null)
                {
                    scoreText.text = "Score: " + score.ToString();
                }
            }

            timeRising += Time.fixedDeltaTime;

        }
    }

    public void SetScore(int pPlacement, int pScore)
    {
        placement = pPlacement;
        score = pScore;

    }
    public void SetPosition(Transform pNewPos)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.MovePosition(pNewPos.position);
        transform.position = pNewPos.position;
        startPos = pNewPos.position;
    }

    public void Raise(float pRaiseHeight, float pRiseDuration)
    {
        isRaising = true;
        raiseHeight = pRaiseHeight;
        riseDuration = pRiseDuration;

        endPos = startPos + Vector3.up * raiseHeight;
    }

}
