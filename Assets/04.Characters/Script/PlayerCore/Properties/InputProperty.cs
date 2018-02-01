﻿using UnityEngine;
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
                return _actions.Move.Value;
            }
            return Vector2.zero;
        }
    }

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

    [ShowInInspector, ReadOnly]
    public bool JustMove
    {
        get
        {
            if(_actions != null)
            {
                return ValidMovementInput(_actions.Move) && !ValidMovementInput(_actions.Move.LastValue);
            }
            return false;
        }
    }
                                      
    public void BindActions(CharacterActions actions, InputDevice device)
    {
        _actions = actions;
        Device = device;
    }


    private bool ValidMovementInput(Vector2 input)
    {
        return (input.x > 0.05f || input.x < -0.05f) ||
               (input.y > 0.05f || input.y < -0.05f);
    }
}
