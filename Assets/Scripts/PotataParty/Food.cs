using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [Tooltip("Food model, change to array later")]
    [SerializeField] GameObject model;
    [SerializeField] GameObject[] foodModels;

    [Tooltip("How fast the food is spinning")]
    [SerializeField] float rotationIntensity;
    [Tooltip("Time in seconds it takes food to travel")]
    [SerializeField] float throwTime;

    [SerializeField] int playerId;

    private float timer;
    private Rigidbody rb;
    private Vector3 startPos;
    private Vector3 endPos;

    Vector3 randomTorque;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(0, 4, 10);
        rb = GetComponent<Rigidbody>();
        randomTorque = new Vector3(Random.value-.5f, Random.value-.5f, Random.value-.5f).normalized;

        model = Instantiate(foodModels[Random.Range(0, foodModels.Length)], transform);
    }

    private void Update()
    {
        model.transform.Rotate(randomTorque, rotationIntensity);

        float percent = timer / throwTime;
        rb.MovePosition(Vector3.Lerp(startPos, endPos, percent));

        if (percent > 1)
        {
            TrafficLight.Instance.AddScore(playerId, 1);
            Destroy(gameObject);
        }

        timer += Time.deltaTime;
    }

    public void SetValues(GameObject pDestination, int pPlayerId)
    {
        endPos = pDestination.transform.position;
        playerId = pPlayerId;
    }

}
