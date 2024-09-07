using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    public interface IMovementController
    {
        void HandleMovement();
    }

    public class MovementController : IMovementController
    {
        public MovementController(PlayerController playerController)
        {
            _playerController = playerController;
        }


        private PlayerController _playerController;

        // private float _moveSpeed = 300;
        // private float _rotationSpeed = 15;
        // private float _smoothTime;

        // Internal
        // private float _currentSpeed;
        private float _velocity;

        private const float ZeroF = 0f;


        public void HandleMovement()
        {
            var adjustedDirection = Quaternion.AngleAxis(_playerController._mainCamera.eulerAngles.y, Vector3.up) *
                                    _playerController._movement;
            if (adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);
                HandleHorizontalVelocity(adjustedDirection);

                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZeroF);

                _playerController.rb.velocity = new Vector3(ZeroF, _playerController.rb.velocity.y, ZeroF);
            }
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            _playerController.transform.rotation =
                Quaternion.RotateTowards(targetRotation, _playerController.transform.rotation,
                    _playerController.Stats.rotationSpeed * Time.deltaTime);
        }

        private void HandleHorizontalVelocity(Vector3 adjustedDirection)
        {
            var velocity = adjustedDirection * (_playerController.Stats.moveSpeed * Time.fixedDeltaTime);
            _playerController.rb.velocity = new Vector3(velocity.x, _playerController.rb.velocity.y, velocity.z);
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