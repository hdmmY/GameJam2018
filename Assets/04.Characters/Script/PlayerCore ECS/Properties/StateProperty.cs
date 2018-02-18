using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerCoreECS
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

    public class StateProperty : BaseProperty
    {
        public State m_state;

        public State m_lastState;
    }
}