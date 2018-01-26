using UnityEngine;
using Sirenix.OdinInspector;

public class Controller : MonoBehaviour
{
    [System.Flags]
    public enum MovementState
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8,
        WallJump = 16,
        GroundJump = 32
    }

    private Movement _movement;

    private Rotation _rotation;

    private Animations _animation;

    private CharacterInformation _characterInfo;

    private Transform _torso;

    public GrabHandler grabHandler { get; private set; }

    [ShowInInspector, ReadOnly]
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
        movementState = MovementState.None;


        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput < -0.05f)
        {
            MoveLeft();
            movementState |= MovementState.Left;
        }
        else if (horizontalInput > 0.05f)
        {
            MoveRight();
            movementState |= MovementState.Right;
        }

        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput < -0.05f)
        {
            MoveDown();
            movementState |= MovementState.Down;
        }
        else if (verticalInput > 0.05f)
        {
            MoveUp();
            movementState |= MovementState.Up;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(false, false);
        }
    }


    #region Movement

    public void Jump(bool force = false, bool forceWallJump = false)
    {
        if ((_characterInfo.m_sinceGrounded < 0.2f || _characterInfo.m_sinceWall < 0.2f || grabHandler.m_isHoldSomethingAnchored) &&
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

    private void MoveLeft()
    {
        _rotation.m_lookingRight = false;
        _movement.MoveLeft();
        //_animation.Run();
        _characterInfo.m_paceState = 1;
    }

    private void MoveRight()
    {
        _rotation.m_lookingRight = true;
        _movement.MoveRight();
        //_animation.Run();
        _characterInfo.m_paceState = 1;
    }

    private void MoveUp()
    {
        _movement.MoveUp();
    }

    private void MoveDown()
    {
        _movement.MoveDown();
    }

    #endregion
}
