using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Base : MonoBehaviour
{
    TestInputActions inputActions;

    private void Awake()
    {
        inputActions = new TestInputActions();
    }

    private void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Action1.performed += Test_Action1;
        inputActions.Test.Action2.performed += Test_Action2;
        inputActions.Test.Action3.performed += Test_Action3;
        inputActions.Test.Action4.performed += Test_Action4;
        inputActions.Test.Action5.performed += Test_Action5;
    }

    private void OnDisable()
    {
        inputActions.Test.Action5.performed -= Test_Action5;
        inputActions.Test.Action4.performed -= Test_Action4;
        inputActions.Test.Action3.performed -= Test_Action3;
        inputActions.Test.Action2.performed -= Test_Action2;
        inputActions.Test.Action1.performed -= Test_Action1;
        inputActions.Test.Disable();
    }

    protected virtual void Test_Action1(InputAction.CallbackContext _)
    {
    }

    protected virtual void Test_Action2(InputAction.CallbackContext _)
    {
    }

    protected virtual void Test_Action3(InputAction.CallbackContext _)
    {
    }

    protected virtual void Test_Action4(InputAction.CallbackContext _)
    {
    }

    protected virtual void Test_Action5(InputAction.CallbackContext _)
    {
    }
}
