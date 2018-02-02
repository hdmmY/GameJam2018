using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

public class RotationSystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public InputProperty m_input;

    public ApplyTorque m_mainBodyRotateTorque;

    public ApplyTorque m_footRotateTorque;

    [SerializeField, ReadOnly]
    private float _initAbsMainBodyTorqueMutiplier;

    //[SerializeField, ReadOnly]
    //private List<Vector3> _initAbsFootTorque;

    private Vector2 _lastValidMoveInput;

    private void Start()
    {
        _initAbsMainBodyTorqueMutiplier = Mathf.Abs(m_mainBodyRotateTorque.m_torqueMutiplier);

        //_initAbsFootTorque = new List<Vector3>(
        //            from torqueApplier in m_footRotateTorque.m_torqueAppliers
        //            select new Vector3(Mathf.Abs(torqueApplier.m_torque.x),
        //                               Mathf.Abs(torqueApplier.m_torque.y),
        //                               Mathf.Abs(torqueApplier.m_torque.z)));
    }

    private void Update()
    {
        if (m_input.HasMoveInput)
        {
            _lastValidMoveInput = m_input.Move;
        }

        RotateMainBody();

        //RotateFoot();
    }

    private void RotateMainBody()
    {
        // Angle between rig's velocity and rig's forward direction
        Rigidbody rig = m_mainBodyRotateTorque.m_torqueAppliers[0].m_rig;

        Vector3 xzInput = new Vector3(_lastValidMoveInput.x, 0, _lastValidMoveInput.y);
        Vector3 xzForward = new Vector3(rig.transform.forward.x, 0, rig.transform.forward.z);

        float angle = Vector3.SignedAngle(xzInput.normalized, xzForward.normalized, Vector3.up);
        float ratio = angle / 180;

        m_mainBodyRotateTorque.m_torqueMutiplier = -1f * _initAbsMainBodyTorqueMutiplier * Mathf.Sin(ratio * Mathf.PI / 2);

        if (m_character.HasState(CharacterProperty.State.Ground) ||
            m_character.HasState(CharacterProperty.State.HoldSomething) ||
            m_character.HasState(CharacterProperty.State.InAir))
        {
            m_mainBodyRotateTorque.m_enabled = true;
        }
        else
        {
            m_mainBodyRotateTorque.m_enabled = false;
        }
    }

    //private void RotateFoot()
    //{
    //    Rigidbody rig;
    //    float angle, ratio;

    //    for (int i = 0; i < m_footRotateTorque.m_torqueAppliers.Count; i++)
    //    {
    //        rig = m_footRotateTorque.m_torqueAppliers[i].m_rig;

    //        angle = Vector3.SignedAngle(rig.transform.forward, Vector3.up, Vector3.right);
    //        ratio = angle / 180;

    //        m_footRotateTorque.m_torqueAppliers[i].m_torque = -1f * _initAbsFootTorque[i] * Mathf.Sin(ratio * Mathf.PI / 2);
    //    }  

    //    if (!m_character.HasState(CharacterProperty.State.BeingGrabbed) &&
    //         m_character.HasState(CharacterProperty.State.Ground) &&
    //        !m_input.HasMoveInput)
    //    {
    //        m_footRotateTorque.m_enabled = true;
    //    }
    //    else
    //    {
    //        m_footRotateTorque.m_enabled = false;
    //    }
    //}



    private void OnDisable()
    {
        m_mainBodyRotateTorque.m_enabled = false;

        m_footRotateTorque.m_enabled = false;
    }
}
