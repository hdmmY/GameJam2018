using InControl;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerCoreECS
{
    public class InputProperty : BaseProperty
    {
        [ShowInInspector, ReadOnly]
        public InputDevice m_device;

        [ShowInInspector, ReadOnly]
        public CharacterActions m_actions;

        [ShowInInspector, ReadOnly]
        public bool JumpWasPressed
        {
            get
            {
                if (m_actions != null)
                {
                    return m_actions.Jump.WasPressed;
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public bool JumpWasReleaseed
        {
            get
            {
                if (m_actions != null)
                {
                    return m_actions.Jump.WasReleased;
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public bool Jump
        {
            get
            {
                if (m_actions != null)
                {
                    return m_actions.Jump;
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public bool PickWasPressed
        {
            get
            {
                if (m_actions != null)
                {
                    return m_actions.Pick.WasPressed;
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public bool Attack
        {
            get
            {
                if (m_actions != null)
                {
                    return m_actions.Attack;
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public bool ThrowWasPressed
        {
            get
            {
                if (m_actions != null)
                {
                    return m_actions.Throw.WasPressed;
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public Vector2 Move
        {
            get
            {
                if (m_actions != null)
                {
                    return m_actions.Move.Value;
                }
                return Vector2.zero;
            }
        }
    }
}