using System;
using UnityEngine;

namespace PlayerSystems.MovementSystem.MovementTypes
{
    [Serializable]
    public class DefaultMovementType : MovementType
    {
        [SerializeField] private float _gravityMultiplier = 1f;

        public override Vector3 GetMovementDirection(float inputedXValue, bool isGrounded)
        {
            Vector3 direction = Vector3.zero;

            direction.z = Vector3.forward.z * MovementSpeed;
            direction.x = inputedXValue * XMovementSpeed;
            direction.y = GravityHandler(_gravityMultiplier, isGrounded);

            return direction;
        }

        public override Quaternion GetAngle(float inputedXValue, float deltaTime, out float rotationSpeed)
        {
            Quaternion rotation = Quaternion.identity;
            rotationSpeed = RotationSetting.RotationSpeed * deltaTime;

            if (inputedXValue > 0)
            {
                rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, RotationSetting.TargetAngel, 0), inputedXValue * 2);
            }
            else if (inputedXValue < 0)
            {
                rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, RotationSetting.MirrorTargetAngel, 0), -inputedXValue * 2);
            }
            else
            {
                rotation = Quaternion.Euler(0, 0, 0);
            }

            return rotation;
        }
    }
}