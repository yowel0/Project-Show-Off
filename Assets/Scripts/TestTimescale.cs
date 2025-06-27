using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimescale : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] float timeScale;

    private void OnValidate()
    {
        Time.timeScale = timeScale;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        Time.timeScale = timeScale;
    }

}
