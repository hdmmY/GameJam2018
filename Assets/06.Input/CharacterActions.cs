using UnityEngine;
using InControl;

public class CharacterActions : PlayerActionSet
{
    public PlayerAction Attack;

    public PlayerAction Jump;

    public PlayerAction Pick;

    public PlayerAction Throw;

    public PlayerAction Forward;

    public PlayerAction Back;

    public PlayerAction Left;

    public PlayerAction Right;

    public PlayerTwoAxisAction Move;

    public CharacterActions()
    {
        Attack = CreatePlayerAction("Attack");
        Jump = CreatePlayerAction("Jump");
        Pick = CreatePlayerAction("Pick");
        Throw = CreatePlayerAction("Throw");
        Forward = CreatePlayerAction("Forward");
        Back = CreatePlayerAction("Back");
        Left = CreatePlayerAction("Left");
        Right = CreatePlayerAction("Right");
        Move = CreateTwoAxisPlayerAction(Left, Right, Back, Forward);
    }


    public static CharacterActions CreateWithControllerBindings()
    {
        CharacterActions characterActions = new CharacterActions();

        characterActions.Jump.AddDefaultBinding(InputControlType.Action1);
        characterActions.Attack.AddDefaultBinding(InputControlType.Action2);
        characterActions.Pick.AddDefaultBinding(InputControlType.Action3);
        characterActions.Throw.AddDefaultBinding(InputControlType.Action3);

        characterActions.Forward.AddDefaultBinding(InputControlType.LeftStickUp);
        characterActions.Back.AddDefaultBinding(InputControlType.LeftStickDown);
        characterActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        characterActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);

        return characterActions;
    }


    public static CharacterActions CreateWithKeyboardBindings()
    {
        CharacterActions characterActions = new CharacterActions();

        characterActions.Jump.AddDefaultBinding(Key.Space);
        characterActions.Attack.AddDefaultBinding(Mouse.LeftButton);
        characterActions.Pick.AddDefaultBinding(Mouse.RightButton);
        characterActions.Throw.AddDefaultBinding(Mouse.LeftButton);

        characterActions.Forward.AddDefaultBinding(Key.W);
        characterActions.Back.AddDefaultBinding(Key.S);
        characterActions.Left.AddDefaultBinding(Key.A);
        characterActions.Right.AddDefaultBinding(Key.D);

        return characterActions;
    }
}
