using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestBase : MonoBehaviour
{
    TestInputActions inputActions;

    private void Awake()
    {
        inputActions = new TestInputActions();
    }

    private void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.onTest1.performed += OnTest1;
        inputActions.Test.onTest2.performed += OnTest2;
        inputActions.Test.onTest3.performed += OnTest3;
        inputActions.Test.onTest4.performed += OnTest4;
        inputActions.Test.onTest5.performed += OnTest5;
    }
    private void OnDisable()
    {
        inputActions.Test.onTest5.performed -= OnTest5;
        inputActions.Test.onTest4.performed -= OnTest4;
        inputActions.Test.onTest3.performed -= OnTest3;
        inputActions.Test.onTest2.performed -= OnTest2;
        inputActions.Test.onTest1.performed -= OnTest1;
        inputActions.Test.Disable();
    }

    protected virtual void OnTest1(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest2(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest3(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest4(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest5(InputAction.CallbackContext context)
    {
    }


}
