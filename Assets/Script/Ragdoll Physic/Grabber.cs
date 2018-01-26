using UnityEngine;
using Sirenix.OdinInspector;


public class Grabber : MonoBehaviour
{
    [ReadOnly]
    public ConfigurableJoint m_joint;

    private GrabHandler _grabHander;

    private Rigidbody _rb;

    private void Start()
    {
        _grabHander = transform.root.GetComponent<GrabHandler>();
        _rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (!_grabHander.m_isGrabbing && !m_joint)
        {
            Destroy(m_joint);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_grabHander && _grabHander.m_grabStrength >= 0.5f && _grabHander.m_isGrabbing && 
            !m_joint &&
            collision.gameObject.layer != LayerMask.NameToLayer("ThinBarrier") &&
            collision.gameObject.layer != LayerMask.NameToLayer("Barrier"))
        {
            if (collision.rigidbody)
            {
                foreach (var grabber in _grabHander.m_grabbers)
                {
                    if (grabber.m_joint && grabber.m_joint.connectedBody != null)
                    {
                        return;
                    }
                }

                _grabHander.StartGrab(collision.rigidbody);
            }

            m_joint = _rb.gameObject.AddComponent<ConfigurableJoint>();
            m_joint.xMotion = ConfigurableJointMotion.Locked;
            m_joint.yMotion = ConfigurableJointMotion.Locked;
            m_joint.zMotion = ConfigurableJointMotion.Locked;
            m_joint.angularXMotion = ConfigurableJointMotion.Locked;
            m_joint.angularYMotion = ConfigurableJointMotion.Locked;
            m_joint.angularZMotion = ConfigurableJointMotion.Locked;
            m_joint.projectionMode = JointProjectionMode.PositionAndRotation;
            m_joint.anchor = transform.InverseTransformPoint(collision.contacts[0].point);
            if (collision.rigidbody) m_joint.connectedBody = collision.rigidbody;
        }
    }


    public void RealseGrab()
    {
        if(m_joint)
        {
            Destroy(m_joint);
        }
    }
}
