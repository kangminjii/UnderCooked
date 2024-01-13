using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    BaseState currentState;

    public bool isGrounded;

    private void Start()
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter();
    }

    private void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic();
    }

    private void LateUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics();
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
        currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }
    private void OnGUI()
    {
        string content = currentState != null ? currentState.name : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
    }


}
