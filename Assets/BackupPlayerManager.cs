using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BackupPlayerManager : PlayerManager
{
    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectsOfType<PlayerManager>().Count() > 1){
            gameObject.SetActive(false);
        }
    }
}
