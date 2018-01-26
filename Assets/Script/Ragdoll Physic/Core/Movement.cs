using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Movement : MonoBehaviour
{
    [BoxGroup("Move")]
    public List<RigidbodyMovement> m_rigsToMove;

    [BoxGroup("Move")]
    public float m_forceMutiplier;

    [BoxGroup("Jump")]
    public List<RigidbodyMovement> m_rigsToJump;

    [BoxGroup("Jump")]
    public float m_jumpForceMutiplier;

    [BoxGroup("Jump")]
    public float m_jumpTime;

    private Standing _standing;

    private Fighting _fighting;

    private CharacterInformation _characterInfo;

    private GrabHandler _grabHandler;

    private Controller _controller;

    private List<Rigidbody> _rbs;

    private void Start()
    {
        _fighting = GetComponent<Fighting>();
        _standing = GetComponent<Standing>();
        _characterInfo = GetComponent<CharacterInformation>();
        _controller = GetComponent<Controller>();
        _grabHandler = GetComponent<GrabHandler>();

        _rbs = new List<Rigidbody>();
        foreach (var body in GetComponentsInChildren<BodyPart>())
        {
            _rbs.Add(body.GetComponent<Rigidbody>());
        }
    }

    public void MoveRight()
    {
        Move(Vector3.right);
    }

    public void MoveLeft()
    {
        Move(Vector3.left);
    }  

    public void MoveUp()
    {
        Move(Vector3.forward);
    }

    public void MoveDown()
    {
        Move(Vector3.back);
    }

    public void Move(Vector3 direction)
    {
        if (_characterInfo.m_sinceFallen <= 0f)
        {
            return;
        }

        float strength = _grabHandler.m_isHoldSomething ? 0.1f : 1f;

        direction = direction.normalized;
        foreach (var rb in _rbs)
        {
            rb.AddForce(direction * m_forceMutiplier * _fighting.m_movementMutiplier * strength *
                        Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        foreach (var rbMovement in m_rigsToMove)
        {
            rbMovement.m_rigidbody.AddForce(direction * rbMovement.m_forceMutiplier * _fighting.m_movementMutiplier *
                        strength * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }


    public bool Jump(bool force = false, bool forceWallJump = false)
    {
        bool result = DoJump(force, forceWallJump);

        return result;
    }


    private bool DoJump(bool force = false, bool forceWallJump = false)
    {
        _standing.m_gravity = m_jumpTime * 0.5f;

        foreach (var jumpRb in m_rigsToJump)
        {
            jumpRb.m_rigidbody.velocity = new Vector3(jumpRb.m_rigidbody.velocity.x, jumpRb.m_rigidbody.velocity.y, 0f);

            float mutiplier = m_jumpForceMutiplier * jumpRb.m_forceMutiplier * Time.fixedDeltaTime;

            if (!force)
            {
                if (_characterInfo.WallNormal != Vector3.zero)
                {
                    jumpRb.m_rigidbody.AddForce(_characterInfo.WallNormal * mutiplier, ForceMode.VelocityChange);
                    jumpRb.m_rigidbody.AddForce(Vector3.up * mutiplier * 0.85f, ForceMode.VelocityChange);
                    return true;
                }
                else
                {
                    jumpRb.m_rigidbody.AddForce(Vector3.up * mutiplier, ForceMode.VelocityChange);
                    return false;
                }
            }
            else if (forceWallJump)
            {
                jumpRb.m_rigidbody.AddForce(_characterInfo.WallNormal * mutiplier * 0.75f, ForceMode.VelocityChange);
                jumpRb.m_rigidbody.AddForce(Vector3.up * mutiplier * 0.85f, ForceMode.VelocityChange);
            }
            else
            {
                jumpRb.m_rigidbody.AddForce(Vector3.up * mutiplier, ForceMode.VelocityChange);
            }
        }

        return false;
    }
}
