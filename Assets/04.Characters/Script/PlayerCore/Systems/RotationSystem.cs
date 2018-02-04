using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

public class RotationSystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public InputProperty m_input;

    public ApplyTorque m_mainBodyRotateTorque;

    [SerializeField, ReadOnly]
    private float _initAbsMainBodyTorqueMutiplier;     

    private void Start()
    {
        _initAbsMainBodyTorqueMutiplier = Mathf.Abs(m_mainBodyRotateTorque.m_torqueMutiplier);
    }

    private void Update()
    {
        if (!m_input.HasMoveInput)
        {
            m_mainBodyRotateTorque.m_enabled = false;
            return;
        }                        

        RotateMainBody();
    }

    private void RotateMainBody()
    {
        // Angle between rig's velocity and rig's forward direction
        Rigidbody rig = m_mainBodyRotateTorque.m_torqueAppliers[0].m_rig;

        Vector3 xzInput = new Vector3(m_input.Move.x, 0, m_input.Move.y);
        Vector3 xzForward = new Vector3(rig.transform.forward.x, 0, rig.transform.forward.z);

        float angle = Vector3.SignedAngle(xzInput.normalized, xzForward.normalized, Vector3.up);
        float ratio = angle / 180;

        m_mainBodyRotateTorque.m_torqueMutiplier = -1f * _initAbsMainBodyTorqueMutiplier * Mathf.Sin(ratio * Mathf.PI / 2);

        if (m_character.HasState(CharacterProperty.State.Ground) ||
            m_character.HasState(CharacterProperty.State.HoldSomething))
        {
            m_mainBodyRotateTorque.m_enabled = true;
        }
        else
        {
            m_mainBodyRotateTorque.m_enabled = false;
        }
    } 


    private void OnDisable()
    {
        m_mainBodyRotateTorque.m_enabled = false;
    }
}
