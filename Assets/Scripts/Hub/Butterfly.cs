using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    [SerializeField] float moveSpeed;

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
        if (path.Count > 0)
        {
            nextLocation = path.Dequeue();
            isActive = true;
        }
        else
        {
            // DO THE REWARD
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
