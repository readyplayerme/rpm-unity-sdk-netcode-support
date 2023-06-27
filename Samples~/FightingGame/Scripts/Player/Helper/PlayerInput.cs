using UnityEngine;

namespace ReadyPlayerMe.NetcodeSupport
{
    public static class PlayerInput
    {
        private const string HORIZONTAL_AXIS = "Horizontal";
        public static float HorizontalAxis => Input.GetAxis(HORIZONTAL_AXIS);
        public static bool IsHoldingSpace => Input.GetKeyUp(KeyCode.Space);
    }
}
