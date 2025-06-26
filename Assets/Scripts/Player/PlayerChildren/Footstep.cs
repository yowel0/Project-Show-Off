using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    [SerializeField] PlayerAvatarMovement pam;
    // Start is called before the first frame update
    void Start()
    {
        pam = GetComponentInParent<PlayerAvatarMovement>();
    }

    public void DoFootstep()
    {
        pam.DoFootstep();
    }
}
