using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool slide;
		public bool dive;
		public bool _throw;
		public bool prep;

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

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnSlide(InputValue value)
		{
			SlideInput(value.isPressed);
		}

		public void OnDive(InputValue value)
		{
			DiveInput(value.isPressed);
		}

		public void OnPrep(InputValue value)
		{
			PrepInput(value.isPressed);
		}

		public void OnThrow(InputValue value)
		{
			ThrowInput(value.isPressed);
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

		public void SlideInput(bool newSlideState)
		{
			slide = newSlideState;
		}

		public void DiveInput(bool newDiveState)
		{
			dive = newDiveState;
		}

		public void PrepInput(bool newPrepState)
		{
			prep = newPrepState;
		}

		public void ThrowInput(bool newThrowState)
		{
			_throw = newThrowState;
		}

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
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
	
}