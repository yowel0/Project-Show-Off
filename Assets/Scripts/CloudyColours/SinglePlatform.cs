using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlatform : MonoBehaviour
{
    [SerializeField] GameObject model;


    public void Disappear(float seconds)
    {
        StartCoroutine(DisappearFor(seconds));
    }

    IEnumerator DisappearFor(float seconds)
    {
        model.SetActive(false);
        yield return new WaitForSeconds(seconds);
        model.SetActive(true);
    }
}
