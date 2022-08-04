using System;
using UnityEngine;

namespace PlayerSystems.MovementSystem.MovementTypes
{
    [Serializable]
    public class SideRunningMovementType : MovementType
    {
        [SerializeField] private AnimationCurve _gravityMultiplierCurve = default;

        #region "Fields"

        private float _curveTime = 0f;

        #endregion

        public override Vector3 GetMovementDirection(float inputedXValue, bool isGrounded)
        {
            Vector3 direction = Vector3.zero;

            direction.z = Vector3.forward.z * MovementSpeed;
            direction.x = inputedXValue * XMovementSpeed;

            _curveTime += Time.fixedDeltaTime;
            _curveTime = Mathf.Clamp(_curveTime, 0, 1f);

            direction.y = GravityHandler(_gravityMultiplierCurve.Evaluate(_curveTime), isGrounded);

            return direction;
        }

        public override void ResetValues()
        {
            base.ResetValues();
            _curveTime = 0;
        }

        public override Quaternion GetAngle(float inputedXValue, float deltaTime, out float rotationSpeed)
        {
            Quaternion rotation = Quaternion.identity;
            rotationSpeed = 1;

            rotation = Quaternion.Euler(0, 0, 0);

            return rotation;
        }
    }
}
