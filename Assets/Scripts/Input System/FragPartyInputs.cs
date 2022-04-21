/*
*  Author:             Ryan Jenkins - based on Unity Starter Asset Package
*  Creation Date:      4/19/2022
*  Last Updated:       4/20/2022
*  Description:        
*/

using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

public class FragPartyInputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool dive;
    public bool slide;
    public bool toss;
    public bool slot_1;
    public bool slot_2;
    public bool slot_3;
    public bool slot_4;
    
    [Header("Movement Settings")]
    public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if(cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnDive(InputValue value)
    {
        DiveInput(value.isPressed);
    }

    public void OnSlide(InputValue value)
    {
        SlideInput(value.isPressed);
    }

    public void OnToss(InputValue value)
    {
        TossInput(value.isPressed);
    }

    public void OnSlot_1(InputValue value)
    {
        Slot_1Input(value.isPressed);
    }
    
    public void OnSlot_2(InputValue value)
    {
        Slot_2Input(value.isPressed);
    }
    
    public void OnSlot_3(InputValue value)
    {
        Slot_3Input(value.isPressed);
    }
    
    public void OnSlot_4(InputValue value)
    {
        Slot_4Input(value.isPressed);
    }
    
#else
// old input sys if we do decide to have it (most likely wont)...
#endif


    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    } 

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }
    
    public void DiveInput(bool newDiveState)
    {
        dive = newDiveState;
    }

    public void SlideInput(bool newSlideState)
    {
        slide = newSlideState;
    }

    public void TossInput(bool newTossState)
    {
        toss = newTossState;
    }

    public void Slot_1Input(bool newSlot_1State)
    {
        slot_1 = newSlot_1State;
    }
    
    public void Slot_2Input(bool newSlot_2State)
    {
        slot_2 = newSlot_2State;
    }
    
    public void Slot_3Input(bool newSlot_3State)
    {
        slot_3 = newSlot_3State;
    }
    
    public void Slot_4Input(bool newSlot_4State)
    {
        slot_4 = newSlot_4State;
    }

#if !UNITY_IOS || !UNITY_ANDROID

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

#endif

}