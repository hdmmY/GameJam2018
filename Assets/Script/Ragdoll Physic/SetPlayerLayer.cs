using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerLayer : MonoBehaviour
{
    private void Awake()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        
        if(players == null)
        {
            foreach(var rig in GetComponentsInChildren<Rigidbody>())
            {
                rig.gameObject.layer = LayerMask.GetMask("Player1");
            }
        }
        else
        {
            int layer = LayerMask.NameToLayer("Player" + players.Length);
            foreach(var rig in GetComponentsInChildren<Rigidbody>())
            {
                rig.gameObject.layer = layer;
            }
        }

    }
}
