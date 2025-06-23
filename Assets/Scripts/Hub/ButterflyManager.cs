using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyManager : MonoBehaviour
{
    [SerializeField] GameObject butterflyPrefab;

    [Tooltip("Amount of points the butterfly visits before it goes poof")]
    [SerializeField] int butterflyRouteAmount;

    [Tooltip("Manually set the starting location of the butterfly")]
    [SerializeField] Transform startLocation;

    [Tooltip("Automatically gets all children")]
    [SerializeField] Transform[] locations;

    [Header("Ignore")]
    [SerializeField] GameObject livingButterfly;


    private void OnEnable()
    {
        locations = GetComponentsInChildren<Transform>(true);

        if (locations[0] != transform)
        {
            int index = 0;
            for (int i = 0; i < locations.Length; i++)
            {
                if (locations[i] == transform)
                {
                    index = i; 
                    break;
                }
            }
            locations[index] = locations[0];
            locations[0] = transform;
        }
    }


    void CreateButterfly()
    {
        if (livingButterfly != null) return;

        // Randomize array but keep this transform in pos 0
        for (int t = 1; t < locations.Length; t++)
        {
            Transform tmp = locations[t];
            int r = Random.Range(t, locations.Length);
            locations[t] = locations[r];
            locations[r] = tmp;
        }

        Queue<Transform> route = new Queue<Transform>();

        for (int i = 1; i <= butterflyRouteAmount; i++)
        {
            route.Enqueue(locations[i]);
        }


        livingButterfly = Instantiate(butterflyPrefab, startLocation.position, Quaternion.identity);
        livingButterfly.GetComponent<Butterfly>().Initialize(route);

    }


    private void FixedUpdate()
    {
        if (livingButterfly == null)
        {
            CreateButterfly();
        }
    }






}
