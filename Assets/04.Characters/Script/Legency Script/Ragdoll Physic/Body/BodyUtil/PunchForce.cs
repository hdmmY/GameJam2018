using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchForce : MonoBehaviour
{
    public Transform m_punchTarget;

    public float m_punchForce;

    public float m_punchCooldownTime;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
    }


    /// <summary>
    /// Whether push success
    /// </summary>
    /// <returns></returns>
    public bool Punch()
    {
        if (_timer <= m_punchCooldownTime) return false;

        return true;
    }

}
