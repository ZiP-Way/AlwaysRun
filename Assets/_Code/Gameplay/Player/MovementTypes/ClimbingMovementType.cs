using System;
using UnityEngine;

namespace PlayerSystems.MovementSystem.MovementTypes
{
    [Serializable]
    public class ClimbingMovementType : MovementType
    {
        public override Vector3 GetMovementDirection(float inputedXValue, bool isGrounded)
        {
            Vector3 direction = Vector3.zero;

            direction.z = Vector3.forward.z;
            direction.x = inputedXValue * XMovementSpeed;
            direction.y = Vector3.forward.z * MovementSpeed;

            return direction;
        }

        public override Quaternion GetAngle(float inputedXValue, float deltaTime, out float rotationSpeed)
        {
            Quaternion rotation = Quaternion.identity;
            rotationSpeed = RotationSetting.RotationSpeed * deltaTime;

            if (inputedXValue > 0)
            {
                rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, RotationSetting.MirrorTargetAngel), inputedXValue);
            }
            else if (inputedXValue < 0)
            {
                rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, RotationSetting.TargetAngel), -inputedXValue);
            }
            else
            {
                rotation = Quaternion.Euler(0, 0, 0);
            }

            return rotation;
        }
    }
}