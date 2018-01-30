using UnityEngine;
using InControl;
using Sirenix.OdinInspector;


public class InputProperty : MonoBehaviour
{
    [ShowInInspector, ReadOnly]
    public InputDevice Device
    {
        get; private set;
    }

    [ShowInInspector, ReadOnly]
    private CharacterActions _actions;

    [ShowInInspector, ReadOnly]
    public bool JumpWasPressed
    {
        get
        {
            if (_actions != null)
            {
                return _actions.Jump.WasPressed;
            }
            return false;
        }
    }

    [ShowInInspector, ReadOnly]
    public bool PickWasPressed
    {
        get
        {
            if (_actions != null)
            {
                return _actions.Pick.WasPressed;
            }
            return false;
        }
    }

    [ShowInInspector, ReadOnly]
    public bool Attack
    {
        get
        {
            if (_actions != null)
            {
                return _actions.Attack;
            }
            return false;
        }
    }

    [ShowInInspector, ReadOnly]
    public bool ThrowWasPressed
    {
        get
        {
            if (_actions != null)
            {
                return _actions.Throw.WasPressed;
            }
            return false;
        }
    }

    [ShowInInspector, ReadOnly]
    public Vector2 Move
    {
        get
        {
            if (_actions != null)
            {
                return _actions.Move.Vector;
            }
            return Vector2.zero;
        }
    }
}
