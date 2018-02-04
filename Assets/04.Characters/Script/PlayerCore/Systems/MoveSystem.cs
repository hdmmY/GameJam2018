using UnityEngine;
using System.Collections;

public class MoveSystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public InputProperty m_input;

    public ApplyForce m_movementForce;

    [Range(0, 1)]
    public float m_forceDampWhenHoldSomething = 0.5f;

    [Range(0, 10)]
    public float m_impulseMagnitudeWhenStopMove = 1.2f;

    [SerializeField]
    private float _initForceMutiplier;

    private Vector2 _lastInput;

    private void Start()
    {
        _initForceMutiplier = m_movementForce.m_forceMutiplier;
    }

    private void Update()
    {
        Vector2 curInputVector = m_input.Move;

        Movement(curInputVector, _lastInput);

        _lastInput = curInputVector;
    }

    private void Movement(Vector2 curMoveInput, Vector2 lastMoveInput)
    {
        if (m_character.HasState(CharacterProperty.State.BeingGrabbed))
        {
            m_movementForce.m_enabled = false;
            return;
        }

        if (m_character.HasState(CharacterProperty.State.InAir) && m_character.InAirTime > 0.2f)
        {
            m_movementForce.m_enabled = false;
            return;
        }

        if (m_character.HasState(CharacterProperty.State.HoldSomething))
        {
            if (m_input.JustStopMove)
            {
                StartCoroutine(StopMoving());
                return;
            }

            if (curMoveInput != Vector2.zero)
            {
                m_movementForce.m_forceMutiplier = _initForceMutiplier * m_forceDampWhenHoldSomething;
                m_movementForce.m_enabled = true;
                ApplyForce(curMoveInput.x, 0, curMoveInput.y);
            }
            else
            {
                m_movementForce.m_enabled = false;
            }
            return;
        }

        if (m_character.HasState(CharacterProperty.State.Ground))
        {
            if (m_input.JustStopMove)
            {
                StartCoroutine(StopMoving());
                return;
            }

            if (curMoveInput != Vector2.zero)
            {
                m_movementForce.m_forceMutiplier = _initForceMutiplier;
                m_movementForce.m_enabled = true;
                ApplyForce(curMoveInput.x, 0, curMoveInput.y);
            }
            else
            {
                m_movementForce.m_enabled = false;
            }
            return;
        }

        m_movementForce.m_enabled = false;
    }

    private void OnDisable()
    {
        m_movementForce.m_enabled = false;
    }

    private void ApplyForce(float forceX, float forceY, float forceZ)
    {
        Vector3 force = new Vector3(forceX, forceY, forceZ);
        force = force.normalized;

        foreach (var forceApplier in m_movementForce.m_forceAppliers)
        {
            forceApplier.ChangeForce(force.x, force.y, force.z);
        }
    } 

    private IEnumerator StopMoving()
    {
        for (int i = 0; i < 10; i++)
        {
            if (!m_character.HasState(CharacterProperty.State.InAir))
            {
                m_movementForce.AddInverseImplusForce(m_impulseMagnitudeWhenStopMove);
                m_movementForce.m_enabled = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
