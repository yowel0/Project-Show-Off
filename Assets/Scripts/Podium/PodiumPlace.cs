using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumPlace : MonoBehaviour
{
    [SerializeField] bool isRaising;
    [SerializeField] float amountRaised;
    [SerializeField] float speed;
    public int placement;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isRaising)
        {
            rb.MovePosition(transform.position + Vector3.up * speed);
            amountRaised += speed;

            if (amountRaised >= (5-placement) * .5f)
            {
                isRaising = false;
            }
        }
    }




    public void Raise()
    {
        isRaising = true;
    }

}
