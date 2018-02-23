using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerBodyPicker : MonoBehaviour
{
    [ShowInInspector]
    public bool TryToPick;

    [ShowInInspector]
    public bool PickSomething
    {
        get
        {
            return _pickJoint != null;
        }
    }

    [ShowInInspector]
    public Rigidbody ConnectRig
    {
        get
        {
            if (_pickJoint == null) return null;

            return _pickJoint.connectedBody;
        }
    }
    private FixedJoint _pickJoint;

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    private void OnCollisionEnter (Collision other)
    {
        if (!TryToPick || PickSomething)
        {
            return;
        }

        var item = other.gameObject.GetComponentInParent<ItemInfo> ();
        if (item != null)
        {
            _pickJoint = gameObject.AddComponent<FixedJoint> ();
            _pickJoint.connectedBody = other.rigidbody;
            _pickJoint.breakForce = item.m_breakForce;
            _pickJoint.breakTorque = item.m_breakTorque;

        }
    }

    public void Deconnected()
    {
        if(_pickJoint != null)
        {
            _pickJoint.breakForce = 0f;
            Destroy(_pickJoint);
        }
    }
}