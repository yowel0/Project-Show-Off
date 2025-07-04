using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodThrowLocation : MonoBehaviour
{
    [Tooltip("Sound that plays when food is thrown (ugh sound)")]
    [SerializeField] SoundObject throwSound;
    [Tooltip("What player this location belongs to (player 1, player 2, etc.)")]
    [SerializeField] int playerNr;

    [Tooltip("Place where food comes from. Defaults to own gameobject if none is defined")]
    [SerializeField] GameObject foodStartLocation;
    [Tooltip("Place where the food is thrown to")]
    [SerializeField] GameObject foodDestination;
    [Tooltip("Food prefab")]
    [SerializeField] GameObject foodPrefab;

    private bool canThrow;
    public void Throw()
    {
        if (!canThrow) return;
        MusicManager.Instance?.PlaySound(throwSound);
        GameObject food = Instantiate(foodPrefab, foodStartLocation.transform.position + Vector3.up * 2, transform.rotation);
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
        if (foodStartLocation == null) foodStartLocation = gameObject;
    }
}
