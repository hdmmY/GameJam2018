using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerCore
{
    public class PlayerInput : MonoBehaviour
    {
        public bool m_useKeyBoard = true;

        [ShowInInspector]
        private CharacterActions _action;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start ()
        {
            if (m_useKeyBoard)
            {
                _action = CharacterActions.CreateWithKeyboardBindings ();
            }
            else
            {
                _action = CharacterActions.CreateWithControllerBindings ();
            }
        }

        [ShowInInspector, ReadOnly]
        public bool JumpWasPressed
        {
            get
            {
                if (_action != null)
                {
                    return _action.Jump.WasPressed;
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public bool JumpWasReleaseed
        {
            get
            {
                if (_action != null)
                {
                    return _action.Jump.WasReleased;
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public bool Jump
        {
            get
            {
                if (_action != null)
                {
                    return _action.Jump;
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public bool Pick
        {
            get
            {
                if (_action != null)
                {
                    return _action.Pick;
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public bool PickWasPress
        {
            get
            {
                if (_action != null)
                {
                    return _action.Pick.WasPressed;
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public bool Attack
        {
            get
            {
                if (_action != null)
                {
                    return _action.Attack;
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public bool ThrowWasPressed
        {
            get
            {
                if (_action != null)
                {
                    return _action.Throw.WasPressed;
                }
                return false;
            }
        }

        public bool HasAnyInput
        {
            get
            {
                if (_action != null)
                {
                    return Attack ||
                        JumpWasPressed || JumpWasReleaseed || Jump ||
                        Pick || PickWasPress ||
                        ThrowWasPressed || InputUtils.ValidMove (Move);
                }
                return false;
            }
        }

        [ShowInInspector, ReadOnly]
        public Vector2 Move
        {
            get
            {
                if (_action != null)
                {
                    return _action.Move.Value;
                }
                return Vector2.zero;
            }
        }
    }
}