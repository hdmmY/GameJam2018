using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ControlInfoUpdateSystemNeedProperty
{
    public ControlProperty m_control;

    public InputProperty m_input;

    public StateProperty m_state;

    public BodyProperty m_body;

    public bool Valid ()
    {
        return m_control && m_input && m_state && m_body;
    }
}

public class ControlInfoUpdateSystem : MonoBehaviour
{
    public List<ControlInfoUpdateSystemNeedProperty> m_entities;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update ()
    {
        foreach (var entity in m_entities)
        {
            if (entity.Valid ())
            {
                UpdateControlInfo (entity);
            }
        }
    }

    private void UpdateControlInfo (ControlInfoUpdateSystemNeedProperty entity)
    {
        var state = entity.m_state;
        var input = entity.m_input;
        var control = entity.m_control;
        var body = entity.m_body;

        UpdateApplyForce (control, state);

        if (state.HasState (State.Dead) || state.HasState (State.Unconscious))
        {
            ResetControl ();
            ReviveCheck ();
            return;
        }

        DirectionCheck (control, input);
        GroundCheck (control, body);
        FallCheck (control, state);
        IdleCheck (control, state, input);

        if (control.m_ground)
        {
            RunCheck (control, state);
        }
        JumpRunCheck (control, state, input);
    }

    private void UpdateApplyForce (ControlProperty control, StateProperty state)
    {
        if (state.HasState (State.Dead) || state.HasState (State.Unconscious) || state.HasState (State.Fall))
        {
            control.m_applyForce = 0.1f;
        }
        else if (state.HasState (State.Jump))
        {
            control.m_applyForce = 0.5f;
        }
        else
        {
            control.m_applyForce = Mathf.Clamp (control.m_applyForce + Time.deltaTime / 2, 0.01f, 1f);
        }
    }

    private void ResetControl ()
    {

    }

    private void ReviveCheck ()
    {

    }

    private void DirectionCheck (ControlProperty control, InputProperty input)
    {
        control.m_rawDirection = new Vector3 (input.Move.x, 0, input.Move.y);

        if (InputUtils.ValidMove (input.Move))
        {
            control.m_direction = control.m_rawDirection.normalized;
        }

        if (!control.m_idle)
        {
            control.m_lookDirection = control.m_direction + new Vector3 (0, 0.2f, 0);
        }
    }

    private void GroundCheck (ControlProperty control, BodyProperty body)
    {
        bool ground = body[BodyPart.LeftKnee].BodyCollider.m_onGround ||
            body[BodyPart.RightKnee].BodyCollider.m_onGround ||
            body[BodyPart.Anchor].BodyCollider.m_onGround;

        if (ground)
        {
            if (control.m_groundCheckDelay <= 0f)
            {
                control.m_ground = true;
            }
            else
            {
                control.m_ground = false;
                control.m_groundCheckDelay -= Time.deltaTime;
            }
        }
        else
        {
            control.m_ground = false;
        }
    }

    private void FallCheck (ControlProperty control, StateProperty state)
    {
        if (!control.m_ground)
        {
            control.m_fallTimer += Time.deltaTime;

            if (!state.HasState (State.Fall))
            {
                if (control.m_fallTimer > 0.1f && control.m_fallTimer < 0.6f)
                {
                    state.m_state = State.Jump;
                }
                else
                {
                    state.m_state = State.Fall;
                }
            }
        }
        else
        {
            control.m_fallTimer = 0f;
        }
    }

    private void IdleCheck (ControlProperty control, StateProperty state, InputProperty input)
    {
        if (InputUtils.HasAnyInput (input) || state.HasState (State.Fall))
        {
            if (control.m_idle)
            {
                control.m_idleTimer = 1 + Random.Range (0, 3);
            }
            control.m_idle = false;
        }
        else
        {
            control.m_idleTimer += Time.deltaTime;

            if (control.m_idleTimer >= 10)
            {
                control.m_idle = true;
                if (Random.Range (1, 400) == 1)
                {
                    control.m_lookDirection = new Vector3 (
                        Random.Range (-1, 1), Random.Range (-0.2f, 1), Random.Range (-1, 1)).normalized;
                }
            }
            
            if(control.m_idleTimer >= 20)
            {
                control.m_run = true;
            }
        }
    }

    private void RunCheck (ControlProperty control, StateProperty state)
    {
        if (control.m_applyForce > 0.5f)
        {
            if (InputUtils.ValidMove (new Vector2 (control.m_rawDirection.x, control.m_rawDirection.z)))
            {
                state.m_state = State.Run;
            }
            else
            {
                state.m_state = State.Stand;
            }
        }
        else
        {
            state.m_state = State.Stand;
        }
    }

    private void JumpRunCheck (ControlProperty control, StateProperty state, InputProperty input)
    {
        if (control.m_jumpDelay > 0f)
        {
            control.m_jumpDelay -= Time.deltaTime;
        }

        if (state.m_lastState == State.Stand &&
            !InputUtils.ValidMove (new Vector2 (control.m_rawDirection.x, control.m_rawDirection.z)) &&
            !control.m_idle)
        {
            control.m_run = false;
        }

        if (input.JumpWasPressed)
        {
            control.m_jumpTimer = 0f;
        }

        if (input.Jump)
        {
            if (control.m_jumpTimer > 0.4f)
            {
                control.m_run = true;
                control.m_runTimer = 1f;
            }
            control.m_jumpTimer += Time.deltaTime;
        }
        else
        {
            if (control.m_runTimer >= 0)
            {
                control.m_runTimer -= Time.deltaTime;
            }
            else
            {
                control.m_runTimer = 0;
                if (!control.m_idle)
                {
                    control.m_run = false;
                }
            }
        }

        if (input.JumpWasReleaseed)
        {
            if (control.m_jumpTimer <= 0.8f)
            {
                control.m_jump = true;

                if (control.m_jumpDelay <= 0f && !state.HasState (State.Jump) && !state.HasState (State.Fall))
                {
                    control.m_fallTimer -= 0.4f;
                    control.m_jumpDelay = 0.8f;
                    control.m_groundCheckDelay = 0.1f;
                    state.m_state = State.Jump;
                }
            }
        }
        else
        {
            control.m_jump = false;
        }
    }
}