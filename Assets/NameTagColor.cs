using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTagColor : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        
        //set name tag color
        Animator animator = GetComponent<Animator>();
        animator.SetInteger("playerID",PlayerManager.Instance.GetPlayerID(GetComponentInParent<PlayerShell>()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
