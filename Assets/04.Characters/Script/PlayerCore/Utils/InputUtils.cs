using InControl;
using UnityEngine;

public static class InputUtils
{
    public static void BindActions (this InputProperty input, CharacterActions actions, InputDevice device)
    {
        input.m_actions = actions;
        input.m_device = device;
    }

    public static bool ValidMove (InputProperty input)
    {
        return ValidMove (input.Move);
    }

    public static bool ValidMove (Vector2 move)
    {
        float tolerance = 0.01f;

        return (move.x < -tolerance || move.x > tolerance) &&
            (move.y < -tolerance || move.y > tolerance);
    }

    public static bool HasAnyInput (InputProperty input)
    {
        return input.Attack || input.JumpWasPressed || input.PickWasPressed ||
            input.ThrowWasPressed || ValidMove (input);
    }
}