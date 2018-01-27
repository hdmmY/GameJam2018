using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSelfCollision : MonoBehaviour
{
    private void Awake()
    {
        var rigs = GetComponentsInChildren<Rigidbody>();

        if(rigs == null)
        foreach(var rig in rigs)
        {
            
        }
    }
}
