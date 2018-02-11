using UnityEngine;

public static class StateUtils
{
    public static bool HasState (this State state, State stateToCheck)
    {
        return (state & stateToCheck) == stateToCheck;
    }

    public static bool All (this State state)
    {
        return HasState (state,
            State.Dead | State.Fall | State.Jump | State.Run |
            State.Run | State.Stand | State.Stun);
    }
}