using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class setposition : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if (target != null)
            transform.position = target.position;
    }

}
