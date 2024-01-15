using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManagers
{

    public Action KeyAction = null;


    public void OnUpdate()
    {
        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();
    }
}
