using UnityEngine;

namespace PlayerCoreECS
{
    public static class StateUtils
    {
        public static bool HasState (this StateProperty state, State stateToCheck)
        {
            return (state.m_state & stateToCheck) == stateToCheck;
        }
    }
}