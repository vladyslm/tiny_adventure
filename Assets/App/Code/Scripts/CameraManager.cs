using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace TinyAdventure
{
    public class CameraManager : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private InputReader input;

        [SerializeField] private CinemachineFreeLook freeLookCamera;

        [Header("Settings")] [SerializeField, Range(0.5f, 3f)]
        private float speedMultiplayer = 0.5f;

        private void OnEnable()
        {
            input.Look += OnLook;
        }

        private void OnDisable()
        {
            input.Look -= OnLook;
        }

        private void OnLook(Vector2 cameraMovement)
        {
            freeLookCamera.m_XAxis.m_InputAxisValue = cameraMovement.x * speedMultiplayer * Time.fixedDeltaTime;
            freeLookCamera.m_YAxis.m_InputAxisValue = cameraMovement.y * speedMultiplayer * Time.fixedDeltaTime;
        }
    }
}