using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerTargetItem : MonoBehaviour
{
    public ItemInfo m_targetItem;

    public bool m_attached = false;

    
    private Rigidbody _rig;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start ()
    {
        _rig = GetComponent<Rigidbody> ();
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate ()
    {
        if (!m_attached)
        {
            Vector3 dir = m_targetItem.transform.rotation * m_targetItem.m_grabTarget -
                transform.position + m_targetItem.transform.position;

            ApplyForceUtils.AlignToVector (
                _rig, transform.forward, dir.normalized, 0.1f, 2f);
            ApplyForceUtils.AlignToVector (
                _rig, transform.up, Vector3.up, 0.1f, 20f);
        }
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    private void OnCollisionEnter (Collision other)
    {
        if (other.transform.GetComponentInParent<ItemInfo> () == m_targetItem)
        {
            if (!m_attached)
            {
                var joint = transform.gameObject.AddComponent<FixedJoint> ();
                joint.connectedBody = other.rigidbody;
                m_attached = true;
            }
        }
    }
}