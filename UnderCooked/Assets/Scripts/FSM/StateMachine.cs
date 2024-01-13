using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    BaseState _currentState;

    //BaseState_P _currentState;

    public bool isGrounded;

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

    private void OnCollisionEnter(Collision collision)
    {
        int groundLayer = LayerMask.NameToLayer("Ground");

        if (collision.gameObject.layer == groundLayer)
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        int groundLayer = LayerMask.NameToLayer("Ground");

        if (collision.gameObject.layer == groundLayer)
            isGrounded = false;
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
    private void OnGUI()
    {
        string content = _currentState != null ? _currentState.name : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
    }


}
