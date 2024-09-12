using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    public interface IMovementController
    {
        void HandleMovement(float speed);
    }

    public class MovementController : IMovementController
    {
        public MovementController(PlayerController playerController)
        {
            _playerController = playerController;
        }


        private readonly PlayerController _playerController;
        
        private float _velocity;

        private const float ZeroF = 0f;


        public void HandleMovement(float speed)
        {
            var adjustedDirection = Quaternion.AngleAxis(_playerController.MainCameraTransform.eulerAngles.y, Vector3.up) *
                                    _playerController.Movement;
            if (adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);
                HandleHorizontalVelocity(adjustedDirection, speed);

                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZeroF);

                _playerController.Rb.velocity = new Vector3(ZeroF, _playerController.Rb.velocity.y, ZeroF);
            }
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            _playerController.transform.rotation =
                Quaternion.RotateTowards(targetRotation, _playerController.transform.rotation,
                    _playerController.Stats.rotationSpeed * Time.deltaTime);
        }

        private void HandleHorizontalVelocity(Vector3 adjustedDirection, float speed)
        {
            // var velocity = adjustedDirection * (_playerController.Stats.moveSpeed * Time.fixedDeltaTime);
            var velocity = adjustedDirection * (speed * Time.fixedDeltaTime);
            _playerController.Rb.velocity = new Vector3(velocity.x, _playerController.Rb.velocity.y, velocity.z);
        }

        private void SmoothSpeed(float value)
        {
            if (value == 0)
            {
                
                _playerController.CurrentSpeed = 0;
                return;
            }

            _playerController.CurrentSpeed =
                Mathf.SmoothDamp(_playerController.CurrentSpeed, value, ref _velocity, _playerController.Stats.smoothTime);
        }
    }
}