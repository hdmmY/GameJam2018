using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    public Transform m_target;

    public Vector3 m_offset;

    private void Update()
    {
        if(m_target != null)
        {
            transform.position = m_target.position + m_offset;
        }
    }
}
