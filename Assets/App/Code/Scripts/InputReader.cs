using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputAction;

namespace TinyAdventure
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "TinyAdventure/Input/InputReader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2> Look = delegate { };
        public event UnityAction Attack = delegate { };
        public event UnityAction<bool> Jump = delegate { };
        
        public event UnityAction<bool> Dash = delegate { }; 

        private PlayerInputAction _inputAction;

        public Vector3 Direction => _inputAction.Player.Move.ReadValue<Vector2>();


        private void OnEnable()
        {
            if (_inputAction == null)
            {
                _inputAction = new PlayerInputAction();
                _inputAction.Player.SetCallbacks(this);
            }
            
        }

        public void EnablePlayerActions()
        {
            _inputAction.Enable();
        }


        public void OnMove(InputAction.CallbackContext context)
        {
            Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look.Invoke(context.ReadValue<Vector2>());
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Attack.Invoke();
                    break;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Jump.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump.Invoke(false);
                    break;
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Dash.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Dash.Invoke(false);
                    break;
            }
        }
    }
}