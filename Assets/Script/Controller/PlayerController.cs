using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using InControl;

public class PlayerController:MonoBehaviour
{
    public InputDevice Device;
    public PlayerActions Actions;
    public bool Attack
    {
        get
        {
            return Actions.Attack;
        }
    }

    public bool Jump
    {
        get
        {
            return Actions.Jump;
        }
    }

    public bool Pick
    {
        get
        {
            return Actions.Pick;
        }
    }

    public Vector2 Move
    {
        get
        {
            return Actions.Move.Vector;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}