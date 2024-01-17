using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chop : Grab
{

    public Chop(Player stateMachine) : base("Chop", stateMachine)
    {
        _sm = (Player)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();


        
    }
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        
    }

    //bool playerInRange()
    //{
    //    Collider playerCollider = _sm.GetComponent<Collider>();
    //    Collider chopCollider = _chop.get
    //}



    void Update()
    {
        
    }
}
