//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Scripts/Player/PlayerControls.inputactions
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

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""dee528b3-ee6d-4e06-8bb6-c7661df1cf41"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""ba335d09-b35c-4f10-b9e7-7c6d3688777b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""RangedAttack"",
                    ""type"": ""Button"",
                    ""id"": ""9287a433-221b-4d7d-9971-ae1b56536a61"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MeleeAttack"",
                    ""type"": ""Button"",
                    ""id"": ""2cf468e8-28a3-4f7b-a5b7-d607fa2741d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TogglePause"",
                    ""type"": ""Button"",
                    ""id"": ""6d5ae063-0750-49ba-a1e4-0f4e75aeb213"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b0920436-cdef-4d4e-a3f3-43eea9247dc0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""5f9e3805-1baf-4312-b109-993c09770aa4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""6ec44050-bab7-4ede-a9d6-bd537e8c3c6d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""8187f867-25c4-4187-85e6-e48c6aedddf0"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""3ada0d9f-a864-435c-b9cd-0c293f7a94c5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""fa777e38-ee8c-404d-b5ff-299f4c2a4885"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""1d6a1656-f4b2-427e-9815-17a7720b5908"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""438c773e-3a73-4a35-92f7-536e2205894b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""RangedAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""097363bc-bac2-4339-9bbc-eef866e484a8"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""MeleeAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9af55942-85fb-4d78-b4e6-aa024241a9e8"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""TogglePause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Events"",
            ""id"": ""d4cf590a-c9b4-4752-8842-c0aa42af4b5c"",
            ""actions"": [
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""a3da3d52-d014-47bd-aa22-a193ac5d4a98"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b5fd5554-715a-4e1d-8526-2f5dc6ef9bd1"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePadControlls;Player1"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8c9daeb-43fe-42e5-94cd-59a788552f00"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Player1"",
            ""bindingGroup"": ""Player1"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_RangedAttack = m_Player.FindAction("RangedAttack", throwIfNotFound: true);
        m_Player_MeleeAttack = m_Player.FindAction("MeleeAttack", throwIfNotFound: true);
        m_Player_TogglePause = m_Player.FindAction("TogglePause", throwIfNotFound: true);
        // Events
        m_Events = asset.FindActionMap("Events", throwIfNotFound: true);
        m_Events_Submit = m_Events.FindAction("Submit", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_RangedAttack;
    private readonly InputAction m_Player_MeleeAttack;
    private readonly InputAction m_Player_TogglePause;
    public struct PlayerActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @RangedAttack => m_Wrapper.m_Player_RangedAttack;
        public InputAction @MeleeAttack => m_Wrapper.m_Player_MeleeAttack;
        public InputAction @TogglePause => m_Wrapper.m_Player_TogglePause;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @RangedAttack.started += instance.OnRangedAttack;
            @RangedAttack.performed += instance.OnRangedAttack;
            @RangedAttack.canceled += instance.OnRangedAttack;
            @MeleeAttack.started += instance.OnMeleeAttack;
            @MeleeAttack.performed += instance.OnMeleeAttack;
            @MeleeAttack.canceled += instance.OnMeleeAttack;
            @TogglePause.started += instance.OnTogglePause;
            @TogglePause.performed += instance.OnTogglePause;
            @TogglePause.canceled += instance.OnTogglePause;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @RangedAttack.started -= instance.OnRangedAttack;
            @RangedAttack.performed -= instance.OnRangedAttack;
            @RangedAttack.canceled -= instance.OnRangedAttack;
            @MeleeAttack.started -= instance.OnMeleeAttack;
            @MeleeAttack.performed -= instance.OnMeleeAttack;
            @MeleeAttack.canceled -= instance.OnMeleeAttack;
            @TogglePause.started -= instance.OnTogglePause;
            @TogglePause.performed -= instance.OnTogglePause;
            @TogglePause.canceled -= instance.OnTogglePause;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Events
    private readonly InputActionMap m_Events;
    private List<IEventsActions> m_EventsActionsCallbackInterfaces = new List<IEventsActions>();
    private readonly InputAction m_Events_Submit;
    public struct EventsActions
    {
        private @PlayerControls m_Wrapper;
        public EventsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Submit => m_Wrapper.m_Events_Submit;
        public InputActionMap Get() { return m_Wrapper.m_Events; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(EventsActions set) { return set.Get(); }
        public void AddCallbacks(IEventsActions instance)
        {
            if (instance == null || m_Wrapper.m_EventsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_EventsActionsCallbackInterfaces.Add(instance);
            @Submit.started += instance.OnSubmit;
            @Submit.performed += instance.OnSubmit;
            @Submit.canceled += instance.OnSubmit;
        }

        private void UnregisterCallbacks(IEventsActions instance)
        {
            @Submit.started -= instance.OnSubmit;
            @Submit.performed -= instance.OnSubmit;
            @Submit.canceled -= instance.OnSubmit;
        }

        public void RemoveCallbacks(IEventsActions instance)
        {
            if (m_Wrapper.m_EventsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IEventsActions instance)
        {
            foreach (var item in m_Wrapper.m_EventsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_EventsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public EventsActions @Events => new EventsActions(this);
    private int m_Player1SchemeIndex = -1;
    public InputControlScheme Player1Scheme
    {
        get
        {
            if (m_Player1SchemeIndex == -1) m_Player1SchemeIndex = asset.FindControlSchemeIndex("Player1");
            return asset.controlSchemes[m_Player1SchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRangedAttack(InputAction.CallbackContext context);
        void OnMeleeAttack(InputAction.CallbackContext context);
        void OnTogglePause(InputAction.CallbackContext context);
    }
    public interface IEventsActions
    {
        void OnSubmit(InputAction.CallbackContext context);
    }
}