using UnityEngine;
using Sirenix.OdinInspector;

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

    [ShowInInspector, ReadOnly]
    public float InAirTime
    {
        get; private set;
    }

    [ShowInInspector, ReadOnly]
    public float GroundTime
    {
        get; private set;
    }

    private void Update()
    {
        if((m_state & State.Ground) == State.Ground)
        {
            GroundTime += Time.deltaTime;
        }
        else
        {
            GroundTime = 0f;
        }

        if((m_state & State.InAir) == State.InAir)
        {
            InAirTime += Time.deltaTime;
        }
        else
        {
            InAirTime = 0f;
        }
    }   
}
