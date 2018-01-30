using UnityEngine;
using Sirenix.OdinInspector;


public class EnvironmentCollisionChecker : MonoBehaviour
{
    /// <summary>
    /// Is this checker has collison with environment this frame ?
    /// </summary>
    [ShowInInspector, ReadOnly]
    public bool HasCollision
    {
        get; private set;
    }

    [ShowInInspector, ReadOnly]
    public Vector3 CollisionPoint
    {
        get; private set;
    }

    [ShowInInspector, ReadOnly]
    public float CollisionAngle
    {
        get; private set;
    }

    [ShowInInspector, ReadOnly]
    public Vector3 CollisionNormal
    {
        get; private set;
    }

    [ShowInInspector, ReadOnly]
    public Collider OtherCollider
    {
        get; private set;
    }

    private bool _justSwithed;

    private void Update()
    {
        if (!_justSwithed)
        {
            HasCollision = false;
        }
        else
        {
            _justSwithed = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Only check environment collision
        // Will ignore character collision
        if (collision.transform.GetComponent<BodyProperty>())
        {
            return;
        }

        var contactPoint = collision.contacts[0];

        CollisionPoint = contactPoint.point;
        CollisionAngle = Vector3.Angle(transform.up, contactPoint.normal);
        CollisionNormal = contactPoint.normal;
        OtherCollider = contactPoint.otherCollider;

        HasCollision = true;
        _justSwithed = true;
    }
}
