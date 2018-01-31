using UnityEngine;

public class CharacterProperty : MonoBehaviour
{
    [System.Flags]
    public enum State
    {
        None = 0,
        Ground = 1,
        InAir = 2,
        HoldSomething = 4,
        BeingGrabbed = 8,
        Wall = 16
    }

    public State m_state;






    public bool HasState(State state)
    {
        return (m_state & state) == state;
    }
}
