using PlayerSystems.States;
using SignalsFramework;
using UnityEngine;

namespace PlayerSystems.Animations
{
    public class PlayerAnimationsEvents : MonoBehaviour
    {
        public void InvokeClimbingUpDone()
        {
            ClimbingState.IsClimbingUpDone.Fire();
        }
    }
}