using System;
using UnityEngine;

namespace PlayerSystems.MovementSystem.MovementTypes
{
    [Serializable]
    public struct RotationSetting
    {
        [SerializeField] private float _targetAngle;
        [SerializeField] private float _rotationSpeed; 

        #region "Properties"

        public float TargetAngel => _targetAngle;
        public float MirrorTargetAngel => 360 - _targetAngle;
        public float RotationSpeed => _rotationSpeed;
        #endregion
    }
    public abstract class MovementType
    {
        [SerializeField] RotationSetting _rotationSetting = default;
        [Header("Movement Settings")]
        [SerializeField] private float _movementSpeed = 10f;
        [SerializeField] private float _xMovementSpeed = 10f;

        #region "Properties"

        protected float MovementSpeed => _movementSpeed;
        protected float XMovementSpeed => _xMovementSpeed;
        protected RotationSetting RotationSetting => _rotationSetting;

        #endregion

        public abstract Vector3 GetMovementDirection(float inputedXValue, bool isGrounded);
        public abstract Quaternion GetAngle(float inputedXValue, float deltaTime, out float rotationSpeed);

        #region "Fields"

        private float _currentGravity = 0f;

        #endregion

        public virtual void ResetValues()
        {
            _currentGravity = 0;
        }

        public void DoJump(float jumpStrenght)
        {
            _currentGravity = jumpStrenght;
        }

        protected float GravityHandler(float gravityMultiplier, bool isGrounded)
        {
            if (isGrounded) _currentGravity = -1f;
            else _currentGravity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;

            return _currentGravity;
        }
    }
}