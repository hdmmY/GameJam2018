using UnityEngine;

public class RotationSystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public InputProperty m_input;

    public ApplyTorque m_rotationTorque;

    private float _initAbsTorqueMutiplier;

    private Vector2 _lastInput;

    private Vector2 _lastValidInput = Vector2.right;

    private void Start()
    {
        _initAbsTorqueMutiplier = Mathf.Abs(m_rotationTorque.m_torqueMutiplier);
    }

    private void Update()
    {
        Vector2 inputVector = m_input.Move;

        Rotation(inputVector, _lastInput);

        _lastInput = inputVector;

        if (inputVector != Vector2.zero) _lastValidInput = inputVector;
    }

    private void Rotation(Vector2 curInputVector, Vector2 lastInputVector)
    {
        //if (curInputVector == Vector2.zero)
        //{
        //    if (lastInputVector != Vector2.zero)
        //    {
        //        m_rotationTorque.FreezeRotation();
        //    }

        //    m_rotationTorque.m_enabled = false;
        //    return;
        //}


        // Angle between rig's velocity and rig's forward direction
        Rigidbody rig = m_rotationTorque.m_torqueAppliers[0].m_rig;

        Vector3 xzInput = new Vector3(_lastValidInput.x, 0, _lastValidInput.y);
        Vector3 xzForward = new Vector3(rig.transform.forward.x, 0, rig.transform.forward.z);

        float angle = Vector3.SignedAngle(xzInput.normalized, xzForward.normalized, Vector3.up);
        float ratio = angle / 180;

        m_rotationTorque.m_torqueMutiplier = -1f * _initAbsTorqueMutiplier * Mathf.Sin(ratio * Mathf.PI / 2);

        if (m_character.HasState(CharacterProperty.State.Ground) ||
            m_character.HasState(CharacterProperty.State.HoldSomething) ||
            m_character.HasState(CharacterProperty.State.InAir))
        {
            m_rotationTorque.m_enabled = true;
        }
        else
        {
            m_rotationTorque.m_enabled = false;
        }
    }




}
