using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets { 


	public class StarterAssetsInputs : MonoBehaviour
	{

		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public static bool Menuopen;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED

        public void Update()
        {
			if (!Menuopen)
			{
				

				Cursor.lockState = CursorLockMode.Locked;
			}
			else if (Menuopen)
			{
				move = new Vector2(0, 0);
				Cursor.lockState = CursorLockMode.None;
			}
		}
        public void OnMove(InputValue value)
		{
            if (Menuopen)
            {
				
            }
			else if (!Menuopen)
            {
			MoveInput(value.Get<Vector2>());
		    }
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook && !Menuopen)
			{
				LookInput(value.Get<Vector2>());
			}
			else if (Menuopen)
            {
				look = new Vector2(0, 0);
            }
		}

		public void OnJump(InputValue value)
		{
			if (!Menuopen)
			{
				JumpInput(value.isPressed);
			}
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
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
			
		}

#endif

	}
	
}