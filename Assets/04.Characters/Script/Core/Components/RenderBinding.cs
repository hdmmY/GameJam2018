using UnityEngine;
using Sirenix.OdinInspector;

public class RenderBinding : MonoBehaviour
{
    [SerializeField, DisableInPlayMode]
    private Transform _followerTransform;
    public Transform FollowerTransform
    {
        get { return _followerTransform; }
        set { _followerTransform = value; }
    }


    [SerializeField, DisableInPlayMode]
    private Transform _originTransform;
    public Transform OriginTransform
    {
        get { return _originTransform; }
        set { _originTransform = value; }
    }

    [SerializeField]
    private bool _bindPosition;
    public bool BindPosition
    {
        get { return _bindPosition; }
        set { _bindPosition = value; }
    }

    [SerializeField, ShowIf("_bindPosition")]
    private Vector3 _positionOffset;
    public Vector3 PositionOffset
    {
        get { return _positionOffset; }
        set { _positionOffset = value; }
    }

    [SerializeField]
    private bool _bindRotation;
    public bool BindRotation
    {
        get { return _bindRotation; }
        set { _bindRotation = value; }
    }

    [SerializeField, ShowIf("_bindRotation")]
    private Quaternion _rotationOffset;
    public Quaternion RotationOffset
    {
        get { return _rotationOffset; }
        set { _rotationOffset = value; }
    }
}
