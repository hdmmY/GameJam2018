using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerCore
{
    public enum State
    {
        None = 0,
        Stand = 1,
        Run = 2,
        Jump = 4,
        Fall = 8,
        Climb = 16,
        Unconscious = 32,
        Dead = 64,
        All = 127
    }

    public class PlayerState : MonoBehaviour
    {
        public State m_state;

        public State m_lastState;

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        void LateUpdate()
        {
            m_lastState = m_state;
        }
    }
}