using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : BaseState
{
    protected Player _sm;

    public Grab(string name, Player stateMachine) : base(name, stateMachine)
    {
        _sm = (Player)stateMachine;
    }
   
}