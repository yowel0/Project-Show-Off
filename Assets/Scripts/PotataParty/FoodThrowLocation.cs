using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodThrowLocation : MonoBehaviour
{
    [Tooltip("What player this location belongs to (player 1, player 2, etc.)")]
    [SerializeField] int playerNr;
    [Tooltip("Place where the food is thrown to")]
    [SerializeField] GameObject foodDestination;
    [Tooltip("Food prefab")]
    [SerializeField] GameObject foodPrefab;

    private bool canThrow;
    public void Throw()
    {
        if (!canThrow) return;
        GameObject food = Instantiate(foodPrefab, transform.position, transform.rotation);
        food.GetComponent<Food>().SetValues(foodDestination, playerNr);
    }

    public void EnableThrow()
    {
        canThrow = true;
    }
    public void DisableThrow()
    {
        canThrow = false;
    }
    
    public void SetPlayer(int pPlayerNr)
    {
        playerNr = pPlayerNr;
    }
    public void SetDestination(GameObject pFoodDestination)
    {
        foodDestination = pFoodDestination;
    }
}
