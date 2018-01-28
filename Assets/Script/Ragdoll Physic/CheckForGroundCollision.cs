using UnityEngine;
using Sirenix.OdinInspector;


public class CheckForGroundCollision : MonoBehaviour
{
    [ShowInInspector, ReadOnly]
    public bool IsGrounded { get; private set; }

    [ShowInInspector, ReadOnly]
    public Vector3 CollisionPosition { get; private set; }

    private bool _justSwitched;

    private Controller _controller;

    private GrabHandler _grabHandler;

    private CharacterInformation _characterInfo;

    private bool _isVitalPart;

    private void Start()
    {
        _controller = transform.root.GetComponent<Controller>();
        _characterInfo = transform.root.GetComponent<CharacterInformation>();
        _grabHandler = transform.root.GetComponent<GrabHandler>();
        _isVitalPart = GetComponent<Torso>() || GetComponent<Head>() || GetComponent<LeftLeg>() || GetComponent<RightLeg>();
    }

    private void Update()
    {
        if(!_justSwitched)
        {
            IsGrounded = false;
        }
        else
        {
            _justSwitched = false;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        DealCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        DealCollision(collision);
    }                            

    private void DealCollision(Collision collision)
    {
        if (collision.transform.root != transform.root)
        {
            if (collision.transform.root.GetComponent<Controller>() &&
               !collision.transform.root.GetComponent<Walkable>())
            {
                return;
            }

            if (collision.gameObject.layer != LayerMask.NameToLayer("Barrier"))
            {
                float angle = Vector3.Angle(Vector3.up, collision.contacts[0].normal);
                
                if(angle <= 75)
                {
                    _justSwitched = true;
                    IsGrounded = true;
                    CollisionPosition = collision.contacts[0].point;
                }
                else if(angle <= 95)
                {
                    _characterInfo.m_sinceWall = 0f;
                    _characterInfo.WallNormal = collision.contacts[0].normal;
                }
            }
        }

    }



}
