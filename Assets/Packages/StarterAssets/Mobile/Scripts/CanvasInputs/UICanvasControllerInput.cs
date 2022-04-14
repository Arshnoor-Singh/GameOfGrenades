using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            starterAssetsInputs.SprintInput(virtualSprintState);
        }

        public void VirtualSlideInput(bool virtualSlideState)
        {
            starterAssetsInputs.SlideInput(virtualSlideState);
        }

        public void VirtualDiveInput(bool virtualDiveState)
        {
            starterAssetsInputs.DiveInput(virtualDiveState);
        }

        public void VirtualPrepInput(bool virtualPrepState)
        {
            starterAssetsInputs.PrepInput(virtualPrepState);
        }

        public void VirtualThrowInput(bool virtualThrowState)
        {
            starterAssetsInputs.ThrowInput(virtualThrowState);
        }
    }

}
