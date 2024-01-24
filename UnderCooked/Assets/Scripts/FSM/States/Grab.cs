//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Grab : BaseState
//{
//    protected Player _playerSM;

//    public Grab(Player stateMachine) : base("Grab", stateMachine)
//    {
//        _playerSM = (Player)stateMachine;
//    }

//    public override void Enter()
//    {
//        base.Enter();
//    }

//    public override void UpdateLogic()
//    {
//        base.UpdateLogic();

//        if (Input.anyKey == false)
//            _stateMachine.ChangeState(_playerSM.GrabIdleState);

//        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
//                Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
//            _stateMachine.ChangeState(_playerSM.GrabMovingState);
//    }

//    public override void UpdatePhysics()
//    {
//        base.UpdatePhysics();
//    }

//}
