using UnityEngine;
using Sirenix.OdinInspector;


public class RigidDebugHelper : MonoBehaviour
{
    public Rigidbody m_rigidbody;

    [ReadOnly, BoxGroup("Velocity")]
    public Vector3 m_velocity;

    [ReadOnly, BoxGroup("Velocity")]
    public float m_velocityMagnitude;

    [ReadOnly, BoxGroup("Velocity")]
    public float m_linearDrag;

    [ReadOnly, BoxGroup("Angle")]
    public Vector3 m_angleVelocity;

    [ReadOnly, BoxGroup("Angle")]
    public float m_angleVelocityMagnitude;

    [ReadOnly, BoxGroup("Angle")]
    public float m_maxAngleVelocity;

    [ReadOnly, BoxGroup("Angle")]
    public float m_angleDrag;

    private void FixedUpdate()
    {
        if (m_rigidbody != null)
        {
            m_angleVelocity = m_rigidbody.angularVelocity * Mathf.Rad2Deg;
            m_maxAngleVelocity = m_rigidbody.maxAngularVelocity;
            m_angleDrag = m_rigidbody.angularDrag;
            m_angleVelocityMagnitude = m_angleVelocity.magnitude;

            m_velocity = m_rigidbody.velocity;
            m_linearDrag = m_rigidbody.drag;
            m_velocityMagnitude = m_velocity.magnitude;
        }
        else
        {
            m_angleVelocity = Vector3.zero;
            m_maxAngleVelocity = 0;
            m_angleDrag = 0;
            m_angleVelocityMagnitude = 0;

            m_velocity = Vector3.zero;
            m_linearDrag = 0;
            m_velocityMagnitude = 0;
        }
    }


}
