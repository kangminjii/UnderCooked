using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    BaseState _currentState;


    private void Start()
    {
        _currentState = GetInitialState();
        if (_currentState != null)
            _currentState.Enter();
    }

    private void Update()
    {
        if (_currentState != null)
            _currentState.UpdateLogic();
    }

    private void LateUpdate()
    {
        if (_currentState != null)
            _currentState.UpdatePhysics();
    }

    public void ChangeState(BaseState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }
   
}