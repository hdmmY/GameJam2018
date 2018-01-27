using UnityEngine;
using Sirenix.OdinInspector;


public class Grabber : MonoBehaviour
{
    [ReadOnly]
    public ConfigurableJoint m_joint;

    private GrabHandler _grabHander;

    private Rigidbody _torso;

    private void Start()
    {
        _grabHander = transform.root.GetComponent<GrabHandler>();
        _torso = transform.root.GetComponentInChildren<Rigidbody>();
    }


    private void Update()
    {
        if (!_grabHander.m_isHoldSomething && !m_joint)
        {
            Destroy(m_joint);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _grabHander = transform.root.GetComponent<GrabHandler>();

        if (!_grabHander.m_isHoldSomething && 
            m_joint != null && 
            collision.rigidbody)
        {
            foreach (var grabber in _grabHander.m_grabbers)
            {
                if (grabber.m_joint && grabber.m_joint.connectedBody)
                {
                    return;
                }
            }

            collision.rigidbody.detectCollisions = false;
            collision.rigidbody.useGravity = false;

            m_joint = _torso.gameObject.AddComponent<ConfigurableJoint>();
            m_joint.xMotion = ConfigurableJointMotion.Locked;
            m_joint.yMotion = ConfigurableJointMotion.Locked;
            m_joint.zMotion = ConfigurableJointMotion.Locked;
            m_joint.angularXMotion = ConfigurableJointMotion.Locked;
            m_joint.angularYMotion = ConfigurableJointMotion.Locked;
            m_joint.angularZMotion = ConfigurableJointMotion.Locked;
            m_joint.projectionMode = JointProjectionMode.PositionAndRotation;
            m_joint.anchor = transform.InverseTransformPoint(collision.contacts[0].point);
            if (collision.rigidbody) m_joint.connectedBody = collision.rigidbody;

            _grabHander.StartGrab(collision.rigidbody);
        }
    }


    public void RealseGrab()
    {
        if (m_joint)
        {
            Destroy(m_joint);
        }
    }
}
