using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingYarnUI : MonoBehaviour
{
    [SerializeField]
    float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0,0,-speed);
    }
}
