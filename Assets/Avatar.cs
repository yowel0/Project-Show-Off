using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    [SerializeField]
    GameObject skin;
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

    public void SetSkin(GameObject skinPrefab){
        if(skin != null){
            Destroy(skin);
        }
        skin = Instantiate(skinPrefab,transform);
    }

    public void SetHat(GameObject hatPrefab){
        if (hat != null){
            Destroy(hat);
        }
        Transform headTop = skin.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/mixamorig:HeadTop_End");
        if (headTop != null){
            hat = Instantiate(hatPrefab,headTop);
        }

    }
}
