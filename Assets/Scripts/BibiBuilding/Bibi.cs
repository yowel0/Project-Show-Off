using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;

public class Bibi : MonoBehaviour
{
    [Header("Balancing curves")]
    [Tooltip("How fast to spin as the minigame goes on.\n Y = Number of full turns per second")]
    [SerializeField] AnimationCurve rotationSpeed;
    [Tooltip("Picks random number (height) between the curves to determine how long to spin before turning around")]
    [SerializeField] MinMaxCurve reverseTurnFrequency = new MinMaxCurve(1, new AnimationCurve(), new AnimationCurve());

    [Range(0f, 10f)]
    [Tooltip("Time in seconds it takes Bibi to fully turn around at full speed")]
    [SerializeField] float fullReverseTime;

    [Header("Other balancing")]
    [SerializeField] GameObject firePivot;
    [SerializeField] AnimationCurve fireRetractCurve;

    [Header("TurnAround Event")]
    [SerializeField]
    UnityEvent OnTurnAround;

    [Header("Debug: Values set by curve")]
    [SerializeField] float currentTurnsPerSecond;
    [SerializeField] float currentDegreesPerFrame;
    [SerializeField] float currentTurnFrequency;

    [Header("Debug: Values that measure")]
    [SerializeField] float forwardRotationMult = 1;
    [SerializeField] bool rotatesClockwise;
    [SerializeField] float totalDegreesTurned;
    [Tooltip("currentDegreesPerFrame's little brother")]
    [SerializeField] float actualDegreesTurned;
    [SerializeField] float totalTurns;

    [Header("Debug: Other")]
    [SerializeField] float reverseStrengthPerFrame;
    [SerializeField] bool isActive;


    public void StartGame()
    {
        currentTurnFrequency = reverseTurnFrequency.Evaluate(0, Random.value);
        totalDegreesTurned = 0;
        forwardRotationMult = 1;
        rotatesClockwise = true;
        isActive = true;
    }
    public void StopGame()
    {
        isActive = false;
    }

    void TurnAround()
    {
        currentTurnFrequency = reverseTurnFrequency.Evaluate(MinigameManager.Instance.GetTimePercent(), Random.value);
        rotatesClockwise = !rotatesClockwise;
        totalDegreesTurned = 0;
        OnTurnAround?.Invoke();
    }

    private void FixedUpdate()
    {

        if (!isActive) return;
        if (rotatesClockwise)
        {
            forwardRotationMult = Mathf.Clamp(forwardRotationMult + reverseStrengthPerFrame, -1f, 1f);
        }
        else forwardRotationMult = Mathf.Clamp(forwardRotationMult - reverseStrengthPerFrame, -1f, 1f);
        
        firePivot.transform.localScale = new Vector3(1, 1, fireRetractCurve.Evaluate(Mathf.Abs(forwardRotationMult)));

        DoRotation();
    }

    void DoRotation()
    {
        // Get turn speed
        currentTurnsPerSecond = rotationSpeed.Evaluate(MinigameManager.Instance.GetTimePercent());
        currentDegreesPerFrame = TurnsPerSecToDegPerFrame(currentTurnsPerSecond);

        // Take turning around in account
        actualDegreesTurned = currentDegreesPerFrame * forwardRotationMult;

        // Rotate and write down how much
        transform.Rotate(new Vector3(0, actualDegreesTurned, 0));
        totalDegreesTurned += actualDegreesTurned;
        totalTurns = totalDegreesTurned / 360;

        if (Mathf.Abs(totalDegreesTurned) >= currentTurnFrequency * 360) TurnAround();
    }

    float TurnsPerSecToDegPerFrame(float turnsPerSec)
    {
        return turnsPerSec * 360 * Time.fixedDeltaTime;
    }



    private void Start()
    {
        reverseStrengthPerFrame = Time.fixedDeltaTime / (fullReverseTime/2);
    }
}
