using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldBasketManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] basketPrefabs;

    public void SpawnBoxes()
    {
        print("spawnboxes");
        List<PlayerShell> players = PlayerManager.Instance.players;
        for (int i = 0; i < players.Count; i++)
        {
            PlayerShell player = players[i];
            Instantiate(basketPrefabs[i], player.GetComponentInChildren<Avatar>().character.transform);
        }
    }

    public void DestroyBoxes()
    {
        List<PlayerShell> players = PlayerManager.Instance.players;
        foreach (PlayerShell p in players)
        {
            Transform t = p.GetComponentInChildren<Avatar>().character.transform;
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
