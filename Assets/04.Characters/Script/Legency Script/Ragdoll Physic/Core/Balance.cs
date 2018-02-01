using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Balance : MonoBehaviour
{
    public float m_balanceForce = 1f;

    private CharacterInformation _characterInfo;

    [ShowInInspector, ReadOnly]
    private Rigidbody _leftKnee;

    [ShowInInspector, ReadOnly]
    private Rigidbody _rightKnee;

    [ShowInInspector, ReadOnly]
    private Rigidbody _leftLeg;

    [ShowInInspector, ReadOnly]
    private Rigidbody _rightLeg;

    private void Start()
    {
        _characterInfo = GetComponent<CharacterInformation>();

        _leftKnee = GetRigidBody(GetComponentInChildren<LeftKnee>());

        _rightKnee = GetRigidBody(GetComponentInChildren<RightKnee>());

        _leftLeg = GetRigidBody(GetComponentInChildren<LeftLeg>());

        _rightLeg = GetRigidBody(GetComponentInChildren<RightLeg>());
    }


    private void FixedUpdate()
    {
        if(_characterInfo.m_sinceFallen > 0f && _characterInfo.m_isGround)
        {
            StayBalanced();
        }
    }


    // Bad code, will restruct later
    private void StayBalanced()
    {
        if(!_rightLeg && !_leftLeg)
        {
            return;
        }

        Rigidbody rb = _leftKnee;
        if(Vector3.Angle(_leftLeg.transform.up, Vector3.up) < Vector3.Angle(_rightLeg.transform.up, Vector3.up))
        {
            rb = _rightKnee;
        }

        Vector3 direction = _leftLeg.transform.up + _rightLeg.transform.up;
        direction.y = 0f;
        direction.z = 0f;

        float fator = _characterInfo.m_paceState == 0 ? 1.5f : 1f;

        rb.AddForce(direction * 1000f * m_balanceForce * fator * Time.fixedDeltaTime, ForceMode.Acceleration);
    }



    private Rigidbody GetRigidBody(Component component)
    {
        if (component == null) return null;
        return component.GetComponent<Rigidbody>();
    }
}
