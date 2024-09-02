using System;
using Cinemachine;
using UnityEngine;

namespace TinyAdventure
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private InputReader input;

        [SerializeField] private CharacterController controller;
        [SerializeField] private CinemachineFreeLook freeLookCamera;
        [SerializeField] private Animator animator;

        [Header("Settings")] [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float smoothTime = .2f;


        private Transform _mainCamera;
        private float _currentSpeed;
        private float _velocity;


        private const float ZeroF = 0;

        private

            // Animator parameters
            static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            if (Camera.main != null) _mainCamera = Camera.main.transform;
            freeLookCamera.Follow = transform;
            freeLookCamera.LookAt = transform;
            freeLookCamera.OnTargetObjectWarped(
                transform,
                transform.position - freeLookCamera.transform.position - Vector3.forward
            );
        }

        private void Start()
        {
            input.EnablePlayerActions();
        }

        private void Update()
        {
            HandleMovement();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(Speed, _currentSpeed);
        }

        private void HandleMovement()
        {
            var movementDirection = new Vector3(input.Direction.x, 0, input.Direction.y);

            var adjustedDirection = Quaternion.AngleAxis(_mainCamera.eulerAngles.y, Vector3.up) * movementDirection;
            if (movementDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);
                HandleCharacterController(adjustedDirection);

                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZeroF);
            }
        }

        private void HandleCharacterController(Vector3 adjustedDirection)
        {
            var adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
            controller.Move(adjustedMovement);
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation =
                Quaternion.RotateTowards(targetRotation, transform.rotation, rotationSpeed * Time.deltaTime);
        }

        private void SmoothSpeed(float value)
        {
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, value, ref _velocity, smoothTime);
        }
    }
}