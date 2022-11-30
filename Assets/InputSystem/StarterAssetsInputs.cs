using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using System;
using Cinemachine;
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
        public float verticalMove;
		public bool jump;
		//public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED

		public delegate void InteractEvent();
        public delegate void SpecificInteractEvent();

		public event InteractEvent Interact = null;
		public event SpecificInteractEvent SpecificInteract = null;

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

		/*public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}*/

        public void OnInteract()
        {
            Interact.Invoke();
        }

        public void OnSpecificInteract()
        {
			SpecificInteract.Invoke();
        }

        public void OnVerticalMove(InputValue value)
        {
			VerticalMoveInput(value.Get<float>());
        }

        public void OnSwapTank()
        {
			LevelReferences.Instance.ChangeController(ERobotType.Tank);
        }

        public void OnSwapHacker()
        {
            LevelReferences.Instance.ChangeController(ERobotType.Hacker);
        }

        public void OnSwapDrone()
        {
            LevelReferences.Instance.ChangeController(ERobotType.Drone);
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

        public void VerticalMoveInput(float newVerticalMoveDirection)
        {
			verticalMove = newVerticalMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		/*public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}*/
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}