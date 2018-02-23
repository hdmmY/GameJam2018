using System.Linq;
using UnityEngine;

namespace PlayerCore
{
    public class PlayerControl : MonoBehaviour
    {
        #region  Variable

        /// <summary>
        /// Current move direction
        /// </summary>
        public Vector3 m_direction;

        /// <summary>
        /// Raw move direction
        /// </summary>
        public Vector3 m_rawDirection;

        /// <summary>
        /// Head look direction
        /// </summary>
        public Vector3 m_lookDirection;

        /// <summary>
        /// Character do nothing, and not being hurt or grabbed
        /// </summary>
        public bool m_idle;

        /// <summary>
        /// Timer for idle
        /// </summary>
        public float m_idleTimer;

        /// <summary>
        /// Whether the character is grounded
        /// </summary>
        public bool m_ground;

        public float m_groundCheckDelay = 0.1f;

        /// <summary>
        /// Base force that apply to body
        /// </summary>
        public float m_applyForce;

        public bool m_jump;

        /// <summary>
        /// Hold the jump button timer
        /// </summary>
        public float m_jumpTimer;

        public float m_jumpDelay = 0.8f;

        public float m_fallTimer;

        public bool m_pickSomething;

        public float m_pickCooldownTimer;

        #endregion

        private PlayerInput _input;

        private PlayerBody _body;

        private PlayerState _state;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start ()
        {
            _input = GetComponent<PlayerInput> ();
            _body = GetComponent<PlayerBody> ();
            _state = GetComponent<PlayerState> ();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update ()
        {
            UpdateApplyForce ();

            if (_state.HasState (State.Dead) || _state.HasState (State.Unconscious))
            {
                return;
            }

            DirectionCheck ();
            GroundCheck ();
            FallCheck ();
            IdleCheck ();
            GrabCheck ();

            if (m_ground)
            {
                RunCheck ();
            }

            JumpCheck ();
        }

        private void UpdateApplyForce ()
        {
            if (_state.HasState (State.Dead) || _state.HasState (State.Unconscious) || _state.HasState (State.Fall))
            {
                m_applyForce = 0.1f;
            }
            else if (_state.HasState (State.Jump))
            {
                m_applyForce = 0.5f;
            }
            else
            {
                m_applyForce = Mathf.Clamp (m_applyForce + Time.deltaTime / 2, 0.01f, 1f);
            }
        }

        private void DirectionCheck ()
        {
            m_rawDirection = new Vector3 (_input.Move.x, 0, _input.Move.y);

            if (InputUtils.ValidMove (_input.Move))
            {
                m_direction = m_rawDirection.normalized;
            }

            if (!m_idle)
            {
                m_lookDirection = m_direction + new Vector3 (0, 0.2f, 0);
            }
        }

        private void GroundCheck ()
        {
            bool ground = _body[BodyPart.LeftKnee].BodyCollider.m_onGround ||
                _body[BodyPart.RightKnee].BodyCollider.m_onGround ||
                _body[BodyPart.Anchor].BodyCollider.m_onGround;

            if (ground)
            {
                if (m_groundCheckDelay <= 0f)
                {
                    m_ground = true;
                }
                else
                {
                    m_ground = false;
                    m_groundCheckDelay -= Time.deltaTime;
                }
            }
            else
            {
                m_ground = false;
            }
        }

        private void FallCheck ()
        {
            if (!m_ground)
            {
                m_fallTimer += Time.deltaTime;

                if (!_state.HasState (State.Fall))
                {
                    if (m_fallTimer > 0.1f && m_fallTimer < 0.6f)
                    {
                        _state.m_state = State.Jump;
                    }
                    else
                    {
                        _state.m_state = State.Fall;
                    }
                }
            }
            else
            {
                m_fallTimer = 0f;
            }
        }

        private void IdleCheck ()
        {
            if (_input.HasAnyInput || _state.HasState (State.Fall))
            {
                if (m_idle)
                {
                    m_idleTimer = 1 + Random.Range (0, 3);
                }
                m_idle = false;
            }
            else
            {
                m_idleTimer += Time.deltaTime;

                if (m_idleTimer >= 10)
                {
                    m_idle = true;
                    if (Random.Range (1, 400) == 1)
                    {
                        m_lookDirection = new Vector3 (
                            Random.Range (-1, 1), Random.Range (-0.2f, 1), Random.Range (-1, 1)).normalized;
                    }
                }
            }
        }

        private void GrabCheck ()
        {
            var pickers = _body.m_bodyInfo.Values
                .Where (x => x.BodyPicker != null)
                .Select (x => x.BodyPicker);

            m_pickSomething = false;
            m_pickCooldownTimer -= Time.deltaTime;

            foreach (var picker in pickers)
            {
                if (picker.PickSomething)
                {
                    m_pickSomething = true;
                }
            }

            if (_input.PickWasPressed && m_pickCooldownTimer < 0f)
            {
                m_pickCooldownTimer = 0.5f;
                Pick ();
            }

            if (_input.ThrowWasPressed && m_pickSomething)
            {
                foreach (var picker in pickers) Throw (picker);
            }
        }

        private void Pick ()
        {
            Debug.Log ("Pick " + Time.time);
        }

        private void Throw (PlayerBodyPicker picker)
        {
            Debug.Log ("Throw " + Time.time);
        }

        private void RunCheck ()
        {
            if (m_applyForce > 0.5f)
            {
                if (InputUtils.ValidMove (new Vector2 (m_rawDirection.x, m_rawDirection.z)))
                {
                    _state.m_state = State.Run;
                }
                else
                {
                    _state.m_state = State.Stand;
                }
            }
            else
            {
                _state.m_state = State.Stand;
            }
        }

        private void JumpCheck ()
        {
            if (m_jumpDelay > 0f)
            {
                m_jumpDelay -= Time.deltaTime;
            }

            if (_input.JumpWasPressed)
            {
                m_jumpTimer = 0f;
            }

            if (_input.Jump)
            {
                m_jumpTimer += Time.deltaTime;
            }

            if (_input.JumpWasReleaseed)
            {
                if (m_jumpTimer <= 0.8f)
                {
                    m_jump = true;

                    if (m_jumpDelay <= 0f && !_state.HasState (State.Jump) && !_state.HasState (State.Fall))
                    {
                        m_fallTimer -= 0.4f;
                        m_jumpDelay = 0.8f;
                        m_groundCheckDelay = 0.1f;
                        _state.m_state = State.Jump;
                    }
                }
            }
            else
            {
                m_jump = false;
            }
        }
    }
}