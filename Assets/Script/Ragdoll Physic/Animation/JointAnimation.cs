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
        Debug.Log("Here!");
        foreach (var animInfo in m_animations)
        {
            Vector3 a = Vector3.zero;
            if (animInfo.m_direction == AnimationInfo.Direction.SelfRight)
            {
                a = transform.right;
            }
            else if (animInfo.m_direction == AnimationInfo.Direction.MainBodyVelocity)
            {
                a = _hip.velocity.normalized;
            }

            float magnitude = Mathf.Abs(Input.GetAxis("Horizontal"));
            if (animInfo.m_type == AnimationInfo.AnimationType.Torque)
            {    
                if (leftForward == animInfo.m_isLeftSide)
                {
                    _rb.AddTorque(a * magnitude * Time.deltaTime * (-animInfo.m_backForceMutiplier), ForceMode.Acceleration);
                }
                else
                {
                    _rb.AddTorque(a * magnitude * Time.deltaTime * (-animInfo.m_forwardForceMultiplier), ForceMode.Acceleration);
                }
            }
            else
            {
                if(leftForward == animInfo.m_isLeftSide)
                {
                    _rb.AddForce(a * magnitude * Time.deltaTime * (-animInfo.m_backForceMutiplier), ForceMode.Acceleration);
                }
                else
                {
                    _rb.AddForce(a * magnitude * Time.deltaTime * (-animInfo.m_forwardForceMultiplier), ForceMode.Acceleration);
                }
            }

        }
    }

}
