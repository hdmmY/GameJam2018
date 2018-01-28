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

    private Rigidbody _hip;

    public GrabHandler grabHandler { get; private set; }

    private PlayerInputController _playerController;

    [ShowInInspector, ReadOnly]
    public MovementState movementState { get; private set; }

    private void Start()
    {
        _movement = GetComponent<Movement>();
        _characterInfo = GetComponent<CharacterInformation>();
        grabHandler = GetComponent<GrabHandler>();
        _hip = GetComponentInChildren<Hip>().GetComponent<Rigidbody>();
        _animation = GetComponent<Animations>();
        _rotation = GetComponent<Rotation>();
        _playerController = GetComponent<PlayerInputController>();
    }

    private void FixedUpdate()
    {
        movementState = MovementState.None;


        Vector3 movement = Vector3.zero;
        float horizontalInput = _playerController.Move.x; ;
        if (horizontalInput < -0.05f)
        {
            movementState |= MovementState.Left;
            movement.x = horizontalInput;
        }
        else if (horizontalInput > 0.05f)
        {
            movementState |= MovementState.Right;
            movement.x = horizontalInput;
        }

        float verticalInput = _playerController.Move.y;
        if (verticalInput < -0.05f)
        {
            movementState |= MovementState.Down;
            movement.y = verticalInput;
        }
        else if (verticalInput > 0.05f)
        {
            movementState |= MovementState.Up;
            movement.y = verticalInput;
        }
        _movement.Move(new Vector3(horizontalInput, 0, verticalInput));

        if (_playerController.Actions.Forward ||
            _playerController.Actions.Back ||
            _playerController.Actions.Left ||
            _playerController.Actions.Right)
        {
            _animation.Run();
        }

        if (_playerController.Actions.Jump)
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

    #endregion
}
