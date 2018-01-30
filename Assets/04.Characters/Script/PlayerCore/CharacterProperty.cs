using UnityEngine;

public class CharacterProperty : MonoBehaviour
{
    [System.Flags]
    public enum State
    {
        None = 0,
        Ground = 1,
        InAir = 2,
        Jump = 4,
        HoldSomething = 8,
        BeingGrabbed = 16,
        Wall = 32
    }

    public State m_state;






    public bool HasState(State state)
    {
        return (m_state & state) == state;
    }
}
