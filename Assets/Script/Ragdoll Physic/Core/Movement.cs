using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Movement : MonoBehaviour
{
    [BoxGroup("Move")]
    public List<RigidbodyMovement> m_rigsToMove;         

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

    private void Start()
    {
        _fighting = GetComponent<Fighting>();
        _standing = GetComponent<Standing>();
        _characterInfo = GetComponent<CharacterInformation>();
        _controller = GetComponent<Controller>();
        _grabHandler = GetComponent<GrabHandler>();     
    }                

    public void Move(Vector3 direction)
    {
        if (_characterInfo.m_sinceFallen <= 0f)
        {
            return;
        }

        float strength = _grabHandler.m_isHoldSomething ? 0.1f : 1f;

        direction = direction.normalized;
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
            float mutiplier = m_jumpForceMutiplier * jumpRb.m_forceMutiplier * Time.fixedDeltaTime;

            if (!force)
            {
                if (_characterInfo.WallNormal != Vector3.zero)
                {
                    jumpRb.m_rigidbody.AddForce(_characterInfo.WallNormal * mutiplier, ForceMode.Force);
                    jumpRb.m_rigidbody.AddForce(Vector3.up * mutiplier * 0.85f, ForceMode.Force);
                    return true;
                }
                else
                {
                    jumpRb.m_rigidbody.AddForce(Vector3.up * mutiplier, ForceMode.Force);
                    return false;
                }
            }
            else if (forceWallJump)
            {
                jumpRb.m_rigidbody.AddForce(_characterInfo.WallNormal * mutiplier * 0.75f, ForceMode.Force);
                jumpRb.m_rigidbody.AddForce(Vector3.up * mutiplier * 0.85f, ForceMode.Force);
            }
            else
            {
                jumpRb.m_rigidbody.AddForce(Vector3.up * mutiplier, ForceMode.Force);
            }
        }

        return false;
    }
}
