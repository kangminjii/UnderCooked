using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManagers
{
    public Action KeyAction = null;
    public Action<Define.KeyBoardEvent> KeyBoardAction = null;

    public void OnUpdate()
    {
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (KeyBoardAction != null) 
        {
            if (Input.GetKey(KeyCode.UpArrow))
                KeyBoardAction.Invoke(Define.KeyBoardEvent.Pressed);
            if (Input.GetKey(KeyCode.LeftArrow))
                KeyBoardAction.Invoke(Define.KeyBoardEvent.Pressed);
            if (Input.GetKey(KeyCode.DownArrow))
                KeyBoardAction.Invoke(Define.KeyBoardEvent.Pressed);
            if (Input.GetKey(KeyCode.RightArrow))
                KeyBoardAction.Invoke(Define.KeyBoardEvent.Pressed); 
        }
       

    }
}
