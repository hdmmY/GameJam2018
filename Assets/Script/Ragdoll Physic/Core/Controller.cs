using UnityEngine;

public class Controller : MonoBehaviour
{
    [System.Flags]
    public enum MovementState
    {
        None = 0,
        Left = 1,
        Right = 2,
        WallJump = 4,
        GroundJump = 8
    }

    private Movement _movement;

    private Rotation _rotation;

    private Animations _animation;

    private CharacterInformation _characterInfo;

    private Transform _torso;

    public GrabHandler grabHandler { get; private set; }

    public MovementState movementState { get; private set; }

    private void Start()
    {
        _rotation = GetComponent<Rotation>();
        _movement = GetComponent<Movement>();
        _characterInfo = GetComponent<CharacterInformation>();
        grabHandler = GetComponent<GrabHandler>();
        _torso = GetComponentInChildren<Torso>().transform;
        _animation = GetComponent<Animations>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(false, false);
        }

        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput < -0.05f)
        {
            Left();
            movementState |= MovementState.Left;
        }
        else if (moveInput > 0.05f)
        {
            Right();
            movementState |= MovementState.Right;
        }


    }



    public void Jump(bool force = false, bool forceWallJump = false)
    {
        if ((_characterInfo.m_sinceGrounded< 0.2f || _characterInfo.m_sinceWall < 0.2f || grabHandler.m_isHoldSomethingAnchored) &&
           _characterInfo.m_sinceFallen > 0f &&
           _characterInfo.m_sinceJumped > 0.3f)
        {
            if (_characterInfo.m_sinceWall > _characterInfo.m_sinceGrounded)
            {
                _characterInfo.WallNormal = Vector3.zero;
            }
            if (grabHandler.m_isHoldSomething)
            {
                grabHandler.EndGrab();
            }

            _characterInfo.m_sinceGrounded = 1f;
            _characterInfo.m_sinceWall = 1f;
            _characterInfo.m_sinceJumped = 0f;
            movementState = _movement.Jump(force, forceWallJump) ? MovementState.WallJump : MovementState.GroundJump;
        }
        else if (force)
        {
            if (_characterInfo.m_sinceWall > _characterInfo.m_sinceGrabbed)
            {
                _characterInfo.WallNormal = Vector3.zero;
            }
            if (grabHandler.m_isHoldSomething)
            {
                grabHandler.EndGrab();
            }

            _characterInfo.m_sinceGrounded = 1f;
            _characterInfo.m_sinceWall = 1f;
            _characterInfo.m_sinceJumped = 0f;
            movementState = _movement.Jump(force, forceWallJump) ? MovementState.WallJump : MovementState.GroundJump;
        }                                                                                                 
    }

    public void Left()
    {
        _rotation.m_lookingRight = false;
        _movement.MoveLeft();
        _animation.Run();
        _characterInfo.m_paceState = 1;
    }

    public void Right()
    {
        _rotation.m_lookingRight = true;
        _movement.MoveRight();
        _animation.Run();
        _characterInfo.m_paceState = 1;
    }

}
