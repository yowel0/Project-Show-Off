using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [Tooltip("Player collides with butterfly")]
    [SerializeField] SoundObject caughtSound;
    [Tooltip("DISCUSS WHEN TO PLAY THIS SOUND!!! ALWAYS OR ONLY WHEN MOVING, AND LOOPING?")]
    [SerializeField] SoundObject moveSound;
    [Tooltip("When the wings appear")]
    [SerializeField] SoundObject finalCatchSound;

    [Header("Debug")]
    [SerializeField] bool isActive;

    [SerializeField] Transform nextLocation;
    Queue<Transform> path;


    private void FixedUpdate()
    {
        if (!isActive) return;

        Vector3 diff = nextLocation.position - transform.position;

        Vector3 moveDir = diff.normalized;

        Vector3 movement = moveDir * moveSpeed * Time.fixedDeltaTime;

        if (diff.magnitude < moveSpeed * Time.fixedDeltaTime)
        {
            movement = moveDir * diff.magnitude;
            isActive = false;
        }

        transform.Translate(movement);


    }

    public void Initialize(Queue<Transform> pPath)
    {
        path = pPath;
    }

    void SetNextLocation()
    {
        MusicManager.Instance.PlaySound(caughtSound);
        if (path.Count > 0)
        {
            nextLocation = path.Dequeue();
            isActive = true;
        }
        else
        {
            // DO THE REWARD
            MusicManager.Instance.PlaySound(finalCatchSound);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SetNextLocation();
        }
    }

}
