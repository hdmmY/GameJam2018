using UnityEngine;
using InControl;
using Sirenix.OdinInspector;


public class InputProperty : BaseProperty
{
    [ShowInInspector, ReadOnly]
    public InputDevice Device
    {
        get; set;
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
                return _actions.Move.Value;
            }
            return Vector2.zero;
        }
    }

    /// <summary>
    /// Move input of the last frame
    /// </summary>
    [ShowInInspector, ReadOnly]
    public Vector2 LastMove
    {
        get
        {
            if (_actions != null)
            {
                return _actions.Move.LastValue;
            }
            return Vector2.zero;
        }
    }                  

    /// <summary>
    /// Whether this frame have valid move input
    /// </summary>
    [ShowInInspector, ReadOnly]
    public bool HasMoveInput
    {
        get
        {
            if (_actions != null)
            {
                return ValidMovementInput(_actions.Move);
            }
            return false;
        }
    }

    /// <summary>
    /// Is this frame have valid move input, but last frame doesn't have valid move input?
    /// </summary>
    [ShowInInspector, ReadOnly]
    public bool JustMove
    {
        get
        {
            if (_actions != null)
            {
                return ValidMovementInput(_actions.Move) && !ValidMovementInput(_actions.Move.LastValue);
            }
            return false;
        }
    }

    [ShowInInspector, ReadOnly]
    public bool JustStopMove
    {
        get
        {
            if(_actions != null)
            {
                return ValidMovementInput(_actions.Move.LastValue) &&
                       !ValidMovementInput(_actions.Move);
            }
            return false;
        }
    }

    public void BindActions(CharacterActions actions, InputDevice device)
    {
        _actions = actions;
        Device = device;
    }


    public static bool ValidMovementInput(Vector2 input)
    {
        return (input.x > 0.05f || input.x < -0.05f) ||
               (input.y > 0.05f || input.y < -0.05f);
    }
}
