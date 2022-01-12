using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager im;

    [HideInInspector] public bool moving = false;

    //private variables below
    Vector2 moveDirection = Vector2.zero;
    bool movePressed = false;
    bool jumpPressed = false;
    bool interactPressed = false;
    bool pausePressed = false;

    void Awake()
    {
        if(im != null)
        {
            Debug.LogError("More than one Input Manager");
        }
        if(im == null)
            im = this.GetComponent<InputManager>();
    }

    public void MovedButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moving = true;
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moving = false;
            moveDirection = context.ReadValue<Vector2>();
        } 
    }

    public void JumpButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
        else if (context.canceled)
        {
            jumpPressed = false;
        }
    }

    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }

    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pausePressed = true;
        }
        else if (context.canceled)
        {
            pausePressed = false;
        }
    }

    public Vector2 GetMoveDirection() 
    {
        return moveDirection;
    }

    public bool GetJumpPressed() 
    {
        bool result = jumpPressed;
        jumpPressed = false;
        return result;
    }

    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public bool GetPausePressed()
    {
        bool result = pausePressed;
        pausePressed = false;
        return result;
    }

}
