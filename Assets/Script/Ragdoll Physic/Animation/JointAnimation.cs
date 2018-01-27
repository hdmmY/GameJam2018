using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class JointAnimation : MonoBehaviour
{
    [ListDrawerSettings(Expanded = true)]
    public List<AnimationInfo> m_animations;

    private Rigidbody _rb;

    private Controller _controller;

    private Rigidbody _hip;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _controller = transform.root.GetComponent<Controller>();
        _hip = (transform.root.GetComponentInChildren<Hip>()).GetComponentInChildren<Rigidbody>();
    }


    // Bad code, will be restruct later
    public void Animate(int paceState, bool leftForward)
    {
        foreach (var animInfo in m_animations)
        {
            Vector3 dir = Vector3.zero;
            switch (animInfo.m_direction)
            {
                case AnimationInfo.Direction.CharacterForward:
                    dir = _hip.transform.forward; break;
                case AnimationInfo.Direction.CharacterRight:
                    dir = _hip.transform.right; break;
                case AnimationInfo.Direction.CharacterUp:
                    dir = _hip.transform.up; break;
                case AnimationInfo.Direction.SelfRight:
                    dir = transform.right; break;
                case AnimationInfo.Direction.SelfForward:
                    dir = transform.forward; break;
                case AnimationInfo.Direction.SelfUp:
                    dir = transform.up; break;
                case AnimationInfo.Direction.WorldRight:
                    dir = Vector3.right; break;
                case AnimationInfo.Direction.WorldUp:
                    dir = Vector3.up; break;
                case AnimationInfo.Direction.WorldForward:
                    dir = Vector3.forward; break;
                default:
                    dir = _hip.velocity.normalized; break;
            }

            Debug.DrawLine(transform.position, transform.position + dir * 10f);

            if (animInfo.m_type == AnimationInfo.AnimationType.Torque)
            {
                if (leftForward == animInfo.m_isLeftSide)
                {
                    _rb.AddTorque(dir * Time.fixedDeltaTime * animInfo.m_backForceMutiplier, ForceMode.VelocityChange);
                }
                else
                {
                    _rb.AddTorque(dir * Time.fixedDeltaTime * animInfo.m_forwardForceMultiplier, ForceMode.VelocityChange);
                }
            }
            else
            {
                if (leftForward == animInfo.m_isLeftSide)
                {
                    _rb.AddForce(dir * Time.fixedDeltaTime * animInfo.m_backForceMutiplier, ForceMode.VelocityChange);
                }
                else
                {
                    _rb.AddForce(dir * Time.fixedDeltaTime * animInfo.m_forwardForceMultiplier, ForceMode.VelocityChange);
                }
            }                    
        }
    }

}
