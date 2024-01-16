using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : BaseState
{
    protected Player _sm;

    public Dash(Player stateMachine) : base("Dash", stateMachine)
    {
        _sm = (Player)stateMachine;
    }
}
