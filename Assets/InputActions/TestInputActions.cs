//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/InputActions/TestInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @TestInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @TestInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TestInputActions"",
    ""maps"": [
        {
            ""name"": ""Test"",
            ""id"": ""ee8abf0b-b868-4f65-bcaf-bd11df8a2d3c"",
            ""actions"": [
                {
                    ""name"": ""Action1"",
                    ""type"": ""Button"",
                    ""id"": ""7a8026c9-9418-4110-84c2-9442872aa6c6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action2"",
                    ""type"": ""Button"",
                    ""id"": ""ba90f737-0c00-4b81-a81e-835ff1cddb3b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action3"",
                    ""type"": ""Button"",
                    ""id"": ""47ea0d15-107a-4534-b53d-3c602acb5e88"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action4"",
                    ""type"": ""Button"",
                    ""id"": ""4de9ea8d-1b16-4876-9541-9b53a6534c4a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action5"",
                    ""type"": ""Button"",
                    ""id"": ""a0ed7311-90db-4043-8b7f-85bcfc6db70a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5d160509-ef55-4a82-b6d4-faa2f1ce96ba"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Action1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2032130-c1bb-4e8b-8b09-ea8ce8ad3894"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Action2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43133614-7efe-4f33-87ee-5109aff008d4"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Action3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc5f5c37-4d9d-4ac0-89f0-32e53f869b3f"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Action4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b1b0ef1-8702-4e69-8d13-d87c5ef4155b"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardAndMouse"",
                    ""action"": ""Action5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardAndMouse"",
            ""bindingGroup"": ""KeyboardAndMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Test
        m_Test = asset.FindActionMap("Test", throwIfNotFound: true);
        m_Test_Action1 = m_Test.FindAction("Action1", throwIfNotFound: true);
        m_Test_Action2 = m_Test.FindAction("Action2", throwIfNotFound: true);
        m_Test_Action3 = m_Test.FindAction("Action3", throwIfNotFound: true);
        m_Test_Action4 = m_Test.FindAction("Action4", throwIfNotFound: true);
        m_Test_Action5 = m_Test.FindAction("Action5", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Test
    private readonly InputActionMap m_Test;
    private ITestActions m_TestActionsCallbackInterface;
    private readonly InputAction m_Test_Action1;
    private readonly InputAction m_Test_Action2;
    private readonly InputAction m_Test_Action3;
    private readonly InputAction m_Test_Action4;
    private readonly InputAction m_Test_Action5;
    public struct TestActions
    {
        private @TestInputActions m_Wrapper;
        public TestActions(@TestInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Action1 => m_Wrapper.m_Test_Action1;
        public InputAction @Action2 => m_Wrapper.m_Test_Action2;
        public InputAction @Action3 => m_Wrapper.m_Test_Action3;
        public InputAction @Action4 => m_Wrapper.m_Test_Action4;
        public InputAction @Action5 => m_Wrapper.m_Test_Action5;
        public InputActionMap Get() { return m_Wrapper.m_Test; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestActions set) { return set.Get(); }
        public void SetCallbacks(ITestActions instance)
        {
            if (m_Wrapper.m_TestActionsCallbackInterface != null)
            {
                @Action1.started -= m_Wrapper.m_TestActionsCallbackInterface.OnAction1;
                @Action1.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnAction1;
                @Action1.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnAction1;
                @Action2.started -= m_Wrapper.m_TestActionsCallbackInterface.OnAction2;
                @Action2.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnAction2;
                @Action2.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnAction2;
                @Action3.started -= m_Wrapper.m_TestActionsCallbackInterface.OnAction3;
                @Action3.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnAction3;
                @Action3.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnAction3;
                @Action4.started -= m_Wrapper.m_TestActionsCallbackInterface.OnAction4;
                @Action4.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnAction4;
                @Action4.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnAction4;
                @Action5.started -= m_Wrapper.m_TestActionsCallbackInterface.OnAction5;
                @Action5.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnAction5;
                @Action5.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnAction5;
            }
            m_Wrapper.m_TestActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Action1.started += instance.OnAction1;
                @Action1.performed += instance.OnAction1;
                @Action1.canceled += instance.OnAction1;
                @Action2.started += instance.OnAction2;
                @Action2.performed += instance.OnAction2;
                @Action2.canceled += instance.OnAction2;
                @Action3.started += instance.OnAction3;
                @Action3.performed += instance.OnAction3;
                @Action3.canceled += instance.OnAction3;
                @Action4.started += instance.OnAction4;
                @Action4.performed += instance.OnAction4;
                @Action4.canceled += instance.OnAction4;
                @Action5.started += instance.OnAction5;
                @Action5.performed += instance.OnAction5;
                @Action5.canceled += instance.OnAction5;
            }
        }
    }
    public TestActions @Test => new TestActions(this);
    private int m_KeyboardAndMouseSchemeIndex = -1;
    public InputControlScheme KeyboardAndMouseScheme
    {
        get
        {
            if (m_KeyboardAndMouseSchemeIndex == -1) m_KeyboardAndMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardAndMouse");
            return asset.controlSchemes[m_KeyboardAndMouseSchemeIndex];
        }
    }
    public interface ITestActions
    {
        void OnAction1(InputAction.CallbackContext context);
        void OnAction2(InputAction.CallbackContext context);
        void OnAction3(InputAction.CallbackContext context);
        void OnAction4(InputAction.CallbackContext context);
        void OnAction5(InputAction.CallbackContext context);
    }
}
