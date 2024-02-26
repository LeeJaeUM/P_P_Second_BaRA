//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/09_InputSystem/EmoteInputActions.inputactions
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

public partial class @EmoteInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @EmoteInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""EmoteInputActions"",
    ""maps"": [
        {
            ""name"": ""Emote"",
            ""id"": ""791a9402-1e00-476b-b2db-7f086192ec50"",
            ""actions"": [
                {
                    ""name"": ""EmoteAble"",
                    ""type"": ""Button"",
                    ""id"": ""a2479bf4-82dd-44d0-97e2-289b8b24a4d3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Lie"",
                    ""type"": ""Button"",
                    ""id"": ""7c241b60-765d-4ae2-a693-8af85e7d7511"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4c8db1d5-3dbd-4806-b7d1-00903a9d0191"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KM"",
                    ""action"": ""Lie"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1a4d0cc-60a8-43ec-b053-8d1ed41f95fc"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KM"",
                    ""action"": ""EmoteAble"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KM"",
            ""bindingGroup"": ""KM"",
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
        // Emote
        m_Emote = asset.FindActionMap("Emote", throwIfNotFound: true);
        m_Emote_EmoteAble = m_Emote.FindAction("EmoteAble", throwIfNotFound: true);
        m_Emote_Lie = m_Emote.FindAction("Lie", throwIfNotFound: true);
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

    // Emote
    private readonly InputActionMap m_Emote;
    private List<IEmoteActions> m_EmoteActionsCallbackInterfaces = new List<IEmoteActions>();
    private readonly InputAction m_Emote_EmoteAble;
    private readonly InputAction m_Emote_Lie;
    public struct EmoteActions
    {
        private @EmoteInputActions m_Wrapper;
        public EmoteActions(@EmoteInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @EmoteAble => m_Wrapper.m_Emote_EmoteAble;
        public InputAction @Lie => m_Wrapper.m_Emote_Lie;
        public InputActionMap Get() { return m_Wrapper.m_Emote; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(EmoteActions set) { return set.Get(); }
        public void AddCallbacks(IEmoteActions instance)
        {
            if (instance == null || m_Wrapper.m_EmoteActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_EmoteActionsCallbackInterfaces.Add(instance);
            @EmoteAble.started += instance.OnEmoteAble;
            @EmoteAble.performed += instance.OnEmoteAble;
            @EmoteAble.canceled += instance.OnEmoteAble;
            @Lie.started += instance.OnLie;
            @Lie.performed += instance.OnLie;
            @Lie.canceled += instance.OnLie;
        }

        private void UnregisterCallbacks(IEmoteActions instance)
        {
            @EmoteAble.started -= instance.OnEmoteAble;
            @EmoteAble.performed -= instance.OnEmoteAble;
            @EmoteAble.canceled -= instance.OnEmoteAble;
            @Lie.started -= instance.OnLie;
            @Lie.performed -= instance.OnLie;
            @Lie.canceled -= instance.OnLie;
        }

        public void RemoveCallbacks(IEmoteActions instance)
        {
            if (m_Wrapper.m_EmoteActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IEmoteActions instance)
        {
            foreach (var item in m_Wrapper.m_EmoteActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_EmoteActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public EmoteActions @Emote => new EmoteActions(this);
    private int m_KMSchemeIndex = -1;
    public InputControlScheme KMScheme
    {
        get
        {
            if (m_KMSchemeIndex == -1) m_KMSchemeIndex = asset.FindControlSchemeIndex("KM");
            return asset.controlSchemes[m_KMSchemeIndex];
        }
    }
    public interface IEmoteActions
    {
        void OnEmoteAble(InputAction.CallbackContext context);
        void OnLie(InputAction.CallbackContext context);
    }
}
