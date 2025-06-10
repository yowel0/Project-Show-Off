using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldBoxManager : MonoBehaviour
{
    [SerializeField]
    GameObject boxPrefab;

    public void SpawnBoxes()
    {
        print("spawnboxes");
        List<PlayerShell> players = PlayerManager.Instance.players;
        foreach (PlayerShell p in players)
        {
            Instantiate(boxPrefab, p.GetComponentInChildren<PlayerAvatarMovement>().transform);
        }
    }

    public void DestroyBoxes()
    {
        List<PlayerShell> players = PlayerManager.Instance.players;
        foreach (PlayerShell p in players)
        {
            Transform t = p.GetComponentInChildren<PlayerAvatarMovement>().transform;
            foreach (Transform child in t)
            {
                if (child.CompareTag("Held Box"))
                {
                    Destroy(child.gameObject);
                    break;
                }
            }
        }
    }
}
