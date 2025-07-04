using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    [SerializeField]
    public GameObject character;
    [SerializeField]
    GameObject hat;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCharacter(GameObject characterPrefab, float yRotation)
    {
        if (hat != null)
            hat.transform.SetParent(gameObject.transform);
        if (character != null)
            Destroy(character);
        character = Instantiate(characterPrefab, transform);
        SetHatParent();
        // character.transform.rotation = quaternion.EulerXYZ(0,yRotation,0);
    }

    public void SetHat(GameObject hatPrefab)
    {
        if (hat != null)
        {
            Destroy(hat);
        }
        hat = Instantiate(hatPrefab);
        SetHatParent();
    }

    void SetHatParent()
    {
        Transform headTop = character.transform.Find("jnt_spine_1/jnt_spine_2/jnt_spine_3/jnt_spine_4/jnt_head_1/jnt_head_2");
        if (headTop != null && hat != null)
        {
            hat.transform.SetParent(headTop);
            hat.transform.localPosition = Vector3.zero;
            // THIS IS THE SCALE USED IN ALL HAT PREFABS. EXPAND CODE IF SCALES AREN'T CONSISTENT
            hat.transform.localScale = Vector3.one * 1.25f; 
        }
        else if (headTop == null && hat != null)
        {
            hat.transform.SetParent(transform, false);
            hat.transform.position += Vector3.up * 1.45f;
            print("headtop not found");
        }
    }
}
