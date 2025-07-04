using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldBasketManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] basketPrefabs;


    private void OnDestroy()
    {
        DestroyBoxes();
    }

    public void SpawnBoxes()
    {
        print("spawnboxes");
        List<PlayerShell> players = PlayerManager.Instance.players;
        for (int i = 0; i < players.Count; i++)
        {
            PlayerShell player = players[i];
            Instantiate(basketPrefabs[i], player.GetComponentInChildren<Avatar>().character.transform.Find("jnt_spine_1/jnt_spine_2/jnt_spine_3/jnt_spine_4/jnt_L_wing_1/jnt_L_wing_2/jnt_L_wing_3"));
        }
        PlayerManager.Instance.PlayersHoldBoxAnimation(true);
    }

    public void DestroyBoxes()
    {
        List<PlayerShell> players = PlayerManager.Instance.players;
        foreach (PlayerShell p in players)
        {
            Transform t = p.transform.GetComponentInChildren<Avatar>().character.transform.Find("jnt_spine_1/jnt_spine_2/jnt_spine_3/jnt_spine_4/jnt_L_wing_1/jnt_L_wing_2/jnt_L_wing_3");
            foreach (Transform child in t)
            {
                if (child.CompareTag("Held Box"))
                {
                    Destroy(child.gameObject);
                    break;
                }
            }
        }
        PlayerManager.Instance.PlayersHoldBoxAnimation(false);
    }
}
