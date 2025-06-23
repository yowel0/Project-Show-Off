using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    [SerializeField]
    GameObject character;
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

    public void SetCharacter(GameObject characterPrefab)
    {
        if (hat != null)
            hat.transform.SetParent(gameObject.transform);
        if (character != null)
            Destroy(character);
        character = Instantiate(characterPrefab, transform);
        SetHatParent();
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
        Transform headTop = character.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/mixamorig:HeadTop_End");
        if (headTop != null && hat != null)
        {
            hat.transform.SetParent(headTop);
            hat.transform.localPosition = Vector3.zero;
        }
        else if (headTop == null && hat != null)
        {
            hat.transform.SetParent(transform, false);
            hat.transform.position += Vector3.up * 1.45f;
            print("headtop not found");
        }
    }
}
