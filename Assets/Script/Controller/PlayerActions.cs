using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerActions : PlayerActionSet {
    public PlayerAction Attack;
    public PlayerAction Jump;
    public PlayerAction Pick;
    public PlayerAction Forward;
    public PlayerAction Back;
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerTwoAxisAction Move;

    public PlayerActions()
    {
        Attack = CreatePlayerAction("Attack");
        Jump = CreatePlayerAction("Jump");
        Pick = CreatePlayerAction("Pick");
        Forward = CreatePlayerAction("Forward");
        Back = CreatePlayerAction("Back");
        Left = CreatePlayerAction("Left");
        Right = CreatePlayerAction("Right");
        Move = CreateTwoAxisPlayerAction(Left, Right, Back, Forward);
    }

    public static PlayerActions CreateController()
    {
        var actions = new PlayerActions();

        actions.Attack.AddDefaultBinding(InputControlType.Action3);
        actions.Jump.AddDefaultBinding(InputControlType.Action1);
        actions.Pick.AddDefaultBinding(InputControlType.Action2);

        actions.Forward.AddDefaultBinding(InputControlType.LeftStickUp);
        actions.Back.AddDefaultBinding(InputControlType.LeftStickDown);
        actions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        actions.Right.AddDefaultBinding(InputControlType.LeftStickRight);

        return actions;
    }
}
