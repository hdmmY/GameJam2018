using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class BodyColliderProperty : BaseProperty
{
    private Transform _baseTransform;
    public Transform BaseTransform
    {
        get
        {
            if (_baseTransform == null)
                _baseTransform = transform.root;
            return _baseTransform;
        }
    }

    [ReadOnly] public bool m_onGround;

    #region Events

    [ReadOnly] public bool m_hasRigistEvent = false;
    [HideInInspector] public Action<Collision, BodyColliderProperty> CollisionEnter;
    [HideInInspector] public Action<Collision, BodyColliderProperty> CollisionStay;
    [HideInInspector] public Action<Collision, BodyColliderProperty> CollisionExit;
    [HideInInspector] public Action<Collider, BodyColliderProperty> TriggerEnter;
    [HideInInspector] public Action<Collider, BodyColliderProperty> TirggerStay;
    [HideInInspector] public Action<Collider, BodyColliderProperty> TriggerExit;

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    private void OnCollisionEnter (Collision other)
    {
        CollisionEnter (other, this);
    }

    /// <summary>
    /// OnCollisionStay is called once per frame for every collider/rigidbody
    /// that is touching rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    private void OnCollisionStay (Collision other)
    {
        CollisionStay (other, this);
    }

    /// <summary>
    /// OnCollisionExit is called when this collider/rigidbody has
    /// stopped touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    private void OnCollisionExit (Collision other)
    {
        CollisionExit (other, this);
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter (Collider other)
    {
        TriggerEnter (other, this);
    }

    /// <summary>
    /// OnTriggerStay is called once per frame for every Collider other
    /// that is touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerStay (Collider other)
    {
        TirggerStay (other, this);
    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerExit (Collider other)
    {
        TriggerExit (other, this);
    }

    #endregion
}