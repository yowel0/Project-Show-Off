using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingItem : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] int playerID;
    [SerializeField] SMFallSpeed fallSpeeds;

    [Header("Ignore")]
    [SerializeField] float fallTime;
    [SerializeField] float timer;
    [SerializeField] float startHeight;
    [SerializeField] float floorHeight;

    Vector3 startPos;
    Vector3 floorPos;
    // Start is called before the first frame update
    void Start()
    {
        fallTime = fallSpeeds.GetRandomFallTime();
        startHeight = transform.position.y;

        startPos = transform.position;
        floorPos = new Vector3(startPos.x, floorHeight, startPos.z);
    }

    private void FixedUpdate()
    {

        float percent = timer / fallTime;
        transform.position = Vector3.Lerp(startPos, floorPos, percent);
        // rb.MovePosition(Vector3.Lerp(startPos, endPos, percent));

        if (percent > 1)
        {
            SMItemScoreLogic.Instance.ItemOnGround(playerID);
            Destroy(gameObject);
            // SMItemScoreLogic.FellOnGround(playerID);
        }

        timer += Time.fixedDeltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (SMItemScoreLogic.Instance.ItemCaught(playerID, other.gameObject))
        {
            Destroy(gameObject);
        }
    }
}
