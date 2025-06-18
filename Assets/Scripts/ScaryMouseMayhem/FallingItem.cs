using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;

public class FallingItem : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] int playerID;
    [SerializeField] SMFallSpeed fallSpeeds;

    [Header("Particles")]
    [Tooltip("Player catches right item")]
    [SerializeField] GameObject succesCatchParticles;
    [Tooltip("Player catches wrong item")]
    [SerializeField] GameObject wrongCatchParticles;
    [Tooltip("The particle effect that plays when this item falls on the ground")]
    [SerializeField] GameObject fellOnGroundParticles;

    [Header("Sounds")]
    [Tooltip("Plays when an item is collected by anyone")]
    [SerializeField] SoundObject itemCollected;
    [Tooltip("Plays when an item is collected by the right player")]
    [SerializeField] SoundObject itemGoodCollect;
    [Tooltip("Plays when an item is collected by the  wrong player")]
    [SerializeField] SoundObject itemWrongCollect;
    [Tooltip("Plays when an item hits the ground")]
    [SerializeField] SoundObject itemNotCollected;

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

        if (percent > 1)
        {
            SMItemScoreLogic.Instance.ItemOnGround(playerID);
            Instantiate(fellOnGroundParticles, transform.position, fellOnGroundParticles.transform.rotation);
            MusicManager.Instance.PlaySound(itemNotCollected);
            Destroy(gameObject);
        }

        timer += Time.fixedDeltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (SMItemScoreLogic.Instance.ItemCaught(playerID, other.gameObject))
        {
            // UGLY compares if catch was good or not SHOULD MOVE SOMEWHERE ELSE 
            PlayerShell ps = other.GetComponentInParent<PlayerShell>();

            int pPlayerID = PlayerManager.Instance.GetPlayerID(ps);
            MusicManager.Instance.PlaySound(itemCollected);

            if (playerID == pPlayerID)
            {
                Instantiate(succesCatchParticles, transform.position, succesCatchParticles.transform.rotation);
                MusicManager.Instance.PlaySound(itemGoodCollect);
            }
            else
            {
                Instantiate(wrongCatchParticles, transform.position, wrongCatchParticles.transform.rotation);
                MusicManager.Instance.PlaySound(itemWrongCollect);
            }


            Destroy(gameObject);
        }
    }
}
