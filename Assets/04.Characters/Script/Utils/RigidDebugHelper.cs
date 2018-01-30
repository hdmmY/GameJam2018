using UnityEngine;
using Sirenix.OdinInspector;


public class RigidDebugHelper : MonoBehaviour
{
    public Rigidbody m_rigidbody;

    [ReadOnly]
    public Vector3 m_velocity;

    [ReadOnly]
    public float m_linearDrag;

    [ReadOnly, Space]
    public Vector3 m_angleVelocity;

    [ReadOnly]
    public float m_maxAngleVelocity;

    [ReadOnly]
    public float m_angleDrag;

    private void FixedUpdate()
    {
        if (m_rigidbody != null)
        {
            m_angleVelocity = m_rigidbody.angularVelocity * Mathf.Rad2Deg;
            m_maxAngleVelocity = m_rigidbody.maxAngularVelocity;
            m_angleDrag = m_rigidbody.angularDrag;

            m_velocity = m_rigidbody.velocity;
            m_linearDrag = m_rigidbody.drag;
        }
        else
        {
            m_angleVelocity = Vector3.zero;
            m_maxAngleVelocity = 0;
            m_angleDrag = 0;

            m_velocity = Vector3.zero;
            m_linearDrag = 0;
        }
    }


}
