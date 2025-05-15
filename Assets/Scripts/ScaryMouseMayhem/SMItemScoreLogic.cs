using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMItemScoreLogic : MonoBehaviour
{
    public static SMItemScoreLogic Instance;

    [Tooltip("Amount of points you gain by collecting the right item")]
    [SerializeField] int pointGain;
    [Tooltip("Amount of points to deduct for messing up")]
    [SerializeField] int pointDeduction;


    public bool ItemCaught(int itemID, GameObject pPlayer)
    {
        PlayerShell ps = pPlayer.GetComponentInParent<PlayerShell>();
        if (ps == null) return false;   // If not a player, it doesn't count

        int playerID = PlayerManager.Instance.GetPlayerID(ps);

        if (itemID-1 == playerID)
        {
            Scores.Instance.AddScore(playerID+1, pointGain);
        }
        else
        {
            Scores.Instance.AddScore(playerID+1, -pointDeduction);
        }
        
        return true;

    }
    public void ItemOnGround(int playerID)
    {
        Scores.Instance.AddScore(playerID, -pointDeduction);
    }


    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }


}
