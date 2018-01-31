using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;


/*

    It's not easy to understand the logic of the movement system.
    I will restruct it later.   
                                                        -- hdmmY
     
*/


public class MoveSystem : MonoBehaviour
{
    public CharacterProperty m_character;

    public InputProperty m_input;

    public ApplyForce m_movementForce;

    [Range(0, 1)]
    public float m_forceDampWhenHoldSomething = 0.5f;

    [Range(0, 10)]
    public float m_impulseMagnitudeWhenStopMove = 1.2f;

    private float _inAirTime;

    private float _initForceMutiplier;

    private Vector2 _lastInput;

    private void Start()
    {
        _initForceMutiplier = m_movementForce.m_forceMutiplier;
    }

    private void Update()
    {
        if (m_character.HasState(CharacterProperty.State.InAir))
        {
            _inAirTime += Time.deltaTime;
        }
        else
        {
            _inAirTime = 0f;
        }
    }

    private void FixedUpdate()
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

        if (m_character.HasState(CharacterProperty.State.Ground) ||
           (m_character.HasState(CharacterProperty.State.InAir) && _inAirTime < 0.2f))
        {
            if (JustStopMove(curMoveInput, lastMoveInput))
            {
                StopMoving();
                return;
            }

            if (curMoveInput != Vector2.zero)
            {
                m_movementForce.m_forceMutiplier = _initForceMutiplier;
                m_movementForce.m_enabled = true;
                ApplyForce(m_movementForce.m_forceAppliers, curMoveInput.x, 0, curMoveInput.y);
            }
            else
            {
                m_movementForce.m_enabled = false;
            }
            return;
        }

        if (m_character.HasState(CharacterProperty.State.HoldSomething))
        {
            if (JustStopMove(curMoveInput, lastMoveInput))
            {
                StopMoving();
                return;
            }

            if (curMoveInput != Vector2.zero)
            {
                m_movementForce.m_forceMutiplier = _initForceMutiplier * m_forceDampWhenHoldSomething;
                m_movementForce.m_enabled = true;
                ApplyForce(m_movementForce.m_forceAppliers, curMoveInput.x, 0, curMoveInput.y);
            }
            else
            {
                m_movementForce.m_enabled = false;
            }
            return;
        }

        m_movementForce.m_enabled = false;
    }

    private void ApplyForce(List<ForceApplier> forceAppliers, float forceX, float forceY, float forceZ)
    {
        foreach (var forceApplier in forceAppliers)
        {
            forceApplier.ChangeForce(forceX, forceY, forceZ);
        }
    }

    private bool JustStopMove(Vector2 curInput, Vector2 lastInput)
    {
        if (curInput == Vector2.zero && lastInput != Vector2.zero)
        {
            return true;
        }
        return false;
    }

    private void StopMoving()
    {
        m_movementForce.AddInverseImplusForce(m_impulseMagnitudeWhenStopMove);
        m_movementForce.m_enabled = false;
    }
}
