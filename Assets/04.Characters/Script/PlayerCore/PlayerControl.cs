using System.Collections;
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

        public bool m_tryToPick;

        public bool m_pickSomething;

        public float m_pickCooldownTimer;

        public float m_pickTimer;

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
            PickCheck ();

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

        private void PickCheck ()
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
            if (!m_pickSomething)
            {
                m_tryToPick = false;
            }

            if (_input.PickWasPress && m_pickCooldownTimer < 0f && !m_pickSomething)
            {
                m_pickTimer = 0f;

                foreach (var picker in pickers) picker.TryToPick = true;
            }

            if (_input.Pick && m_pickCooldownTimer < 0f)
            {
                m_pickTimer += Time.deltaTime;

                if (m_pickTimer < 3f && !m_pickSomething)
                {
                    m_tryToPick = true;
                    Pick ();
                }
                else
                {
                    m_pickCooldownTimer = 1f;

                    foreach (var picker in pickers) picker.TryToPick = false;
                }
            }

            m_pickCooldownTimer -= Time.deltaTime;

            if (_input.ThrowWasPressed && m_pickSomething)
            {
                Throw ();
            }
        }

        private void Pick ()
        {
            Vector3 leftArmDir = (_body[BodyPart.Torso].BodyTransform.right * 2 +
                _body[BodyPart.Torso].BodyTransform.forward).normalized;
            Vector3 leftDelt = (_body[BodyPart.Torso].BodyTransform.right -
                _body[BodyPart.Torso].BodyTransform.forward).normalized * 0.75f;

            ApplyForceUtils.AlignToVector (_body[BodyPart.LeftArm],
                _body[BodyPart.LeftArm].BodyTransform.up, Vector3.up, 1f, 10f);
            ApplyForceUtils.AlignToVector (_body[BodyPart.LeftElbow],
                _body[BodyPart.LeftElbow].BodyTransform.up, Vector3.up, 1f, 10f);
            ApplyForceUtils.AlignToVector (_body[BodyPart.LeftHand],
                _body[BodyPart.LeftHand].BodyTransform.up, Vector3.up, 1f, 5f);

            ApplyForceUtils.AlignToVector (_body[BodyPart.RightArm],
                _body[BodyPart.RightArm].BodyTransform.up, Vector3.up, 1f, 10f);
            ApplyForceUtils.AlignToVector (_body[BodyPart.RightElbow],
                _body[BodyPart.RightElbow].BodyTransform.up, Vector3.up, 1f, 10f);
            ApplyForceUtils.AlignToVector (_body[BodyPart.RightHand],
                _body[BodyPart.RightHand].BodyTransform.up, Vector3.up, 1f, 5f);

            ApplyForceUtils.AlignToVector (_body[BodyPart.LeftArm],
                _body[BodyPart.LeftArm].BodyTransform.forward, leftArmDir, 1f, 1f);
            ApplyForceUtils.AlignToVector (_body[BodyPart.RightArm],
                _body[BodyPart.RightArm].BodyTransform.forward, -leftArmDir, 1f, 1f);

            ApplyForceUtils.AlignToVector (_body[BodyPart.LeftElbow],
                _body[BodyPart.LeftElbow].BodyTransform.forward, leftArmDir + leftDelt, 1f, 1f);
            ApplyForceUtils.AlignToVector (_body[BodyPart.RightElbow],
                _body[BodyPart.RightElbow].BodyTransform.forward, -leftArmDir - leftDelt, 1f, 1f);

            ApplyForceUtils.AlignToVector (_body[BodyPart.LeftHand],
                _body[BodyPart.LeftHand].BodyTransform.forward, leftArmDir + 2 * leftDelt, 1f, 1f);
            ApplyForceUtils.AlignToVector (_body[BodyPart.RightHand],
                _body[BodyPart.RightHand].BodyTransform.forward, -leftArmDir - 2 * leftDelt, 1f, 1f);

            _body[BodyPart.LeftHand].BodyRigid.AddForce (
                Vector3.up + leftArmDir + 2 * leftDelt, ForceMode.Force);
            _body[BodyPart.RightHand].BodyRigid.AddForce (
                Vector3.up - leftArmDir - 2 * leftDelt, ForceMode.Force);
        }

        private void Throw ()
        {
            var pickers = _body.m_bodyInfo.Values
                .Where (x => x.BodyPicker != null)
                .Select (x => x.BodyPicker);

            foreach(var picker in pickers)
            {
                picker.Deconnected();
            }
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