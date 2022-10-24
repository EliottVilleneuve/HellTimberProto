///-----------------------------------------------------------------
///   Author : #DEVELOPER_NAME#                    
///   Date   : #DATE#
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HellLumber {

    enum VectorInputType { Move, Aim}
    public enum ButtonInputType { Swing, Dash, Barricade, Chain, Bomb, Skip, Pause, HoldBuild, LENGTH}
    public enum InputStatus {Idle, Down, Held, Up}
    public class Controller : MonoBehaviour {

        private static Controller instance;

        public static Vector3 MousePosition
        {
            get
            {
                if(instance == null) return Input.mousePosition;
                return instance.mousePosition;
            }
        }

        public HellTimberProto inputActions;

        public Vector2 moveVector;
        public Vector2 lookVector;

        public InputStatus[] inputStatuses;
        //public ButtonTypeToInputAction[] buttonTypeToInputActions;

        private Vector3 mousePosition;

        private void Awake()
        {
            if(instance != null)
            {
                if(instance != this) Destroy(this);
                return;
            }
            instance = this;
            //inputStatuses = new InputStatus[(int)ButtonInputType.LENGTH];
        }
        private void OnValidate()
        {
            /*if(buttonTypeToInputActions != null)
            {
                if (buttonTypeToInputActions.Length == (int)ButtonInputType.LENGTH) return;
            }
            int length = (int)ButtonInputType.LENGTH;
            buttonTypeToInputActions = new ButtonTypeToInputAction[length]; */
            if (inputStatuses != null)
            {
                if (inputStatuses.Length == (int)ButtonInputType.LENGTH) return;
            }
            int length = (int)ButtonInputType.LENGTH;
            inputStatuses = new InputStatus[length];
        }
        private void OnDestroy()
        {
            if (instance == this) instance = null;
        }

        private void LateUpdate()
        {
            for (int i = 0; i < inputStatuses.Length; i++)
            {
                if (inputStatuses[i] == InputStatus.Down) inputStatuses[i] = InputStatus.Held;
                else if (inputStatuses[i] == InputStatus.Up) inputStatuses[i] = InputStatus.Idle;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveVector = context.ReadValue<Vector2>();// .Get<Vector2>();
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            lookVector = context.ReadValue<Vector2>();//value.Get<Vector2>();
        }
        public void OnSwing(InputAction.CallbackContext context) => UpdateButton(ButtonInputType.Swing, context);
        public void Dash(InputAction.CallbackContext context) => UpdateButton(ButtonInputType.Dash, context);
        public void Barricade(InputAction.CallbackContext context) => UpdateButton(ButtonInputType.Barricade, context);
        public void Chain(InputAction.CallbackContext context) => UpdateButton(ButtonInputType.Chain, context);
        public void Bomb(InputAction.CallbackContext context) => UpdateButton(ButtonInputType.Bomb, context);
        public void Options(InputAction.CallbackContext context) => UpdateButton(ButtonInputType.Pause, context);
        public void Skip(InputAction.CallbackContext context) => UpdateButton(ButtonInputType.Skip, context);
        public void HoldBuild(InputAction.CallbackContext context) => UpdateButton(ButtonInputType.HoldBuild, context);
        

        private void UpdateButton(ButtonInputType buttonInputType, InputAction.CallbackContext context)
        {
            int i = (int)buttonInputType;
            if (context.performed) inputStatuses[i] = InputStatus.Down;
            if (context.canceled) inputStatuses[i] = InputStatus.Up;
        }

        public static Vector2 GetVector(string vectorName)
        {
            if(instance == null)
            {
                if (vectorName == "Move") return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                else return new Vector2(Input.GetAxisRaw("Horizontal2"), Input.GetAxisRaw("Vertical2"));
            }
            if (vectorName == "Move") return instance.moveVector;
            else return instance.lookVector;
        }

        /*private bool GetInputAction(ButtonInputType buttonInputType, out InputAction action)
        {
            action = default;
            /*int length = buttonTypeToInputActions.Length;
            for (int i = 0; i < length; i++)
            {
                if(buttonTypeToInputActions[i].type == buttonInputType)
                {
                    action = buttonTypeToInputActions[i].inputAction;
                    return true;
                }
            }
            return false;
        }*/

        public static bool GetButtonDown(string defaultInputName, ButtonInputType buttonInputType)
        {
            if (instance == null) return Input.GetButtonDown(defaultInputName);
            //if (!instance.GetInputAction(buttonInputType, out InputAction action)) return false;
            //return action.GetButtonDown();
            if (instance.inputStatuses[(int)buttonInputType] == InputStatus.Down) return true;
            return false;
        }

        public static bool GetButton(string defaultInputName, ButtonInputType buttonInputType)
        {
            if (instance == null) return Input.GetButton(defaultInputName);
            //if (!instance.GetInputAction(buttonInputType, out InputAction action)) return false;
            //return action.GetButton();
            if (instance.inputStatuses[(int)buttonInputType] != InputStatus.Idle) return true;
            return false;
        }

        public static bool GetButtonUp(string defaultInputName, ButtonInputType buttonInputType)
        {
            if (instance == null) return Input.GetButtonUp(defaultInputName);
            //if (!instance.GetInputAction(buttonInputType, out InputAction action)) return false;
            //return action.GetButtonUp();
            if (instance.inputStatuses[(int)buttonInputType] == InputStatus.Up) return true;
            return false;
        }
    }

    [Serializable]
    public class ButtonTypeToInputAction
    {
        public ButtonInputType type;
        public InputAction inputAction;
    }

    public static class InputActionButtonExtensions
    {
        public static bool GetButton(this InputAction action)
        {
            U.L(action.ReadValue<float>());
            return action.ReadValue<float>() > 0;
        }
        //public static bool GetButton(this InputAction action) => action.ReadValue<float>() > 0;
        public static bool GetButtonDown(this InputAction action) => action.triggered && action.ReadValue<float>() > 0;
        public static bool GetButtonUp(this InputAction action) => action.triggered && action.ReadValue<float>() == 0;
    }
}