using System;
using Cinemachine;
using UnityEngine;

namespace TinyAdventure
{
    public class FreeLookCameraSetup : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook freeLookCamera;

        private void Awake()
        {
            freeLookCamera.Follow = transform;
            freeLookCamera.LookAt = transform;
            freeLookCamera.OnTargetObjectWarped(
                transform,
                transform.position - freeLookCamera.transform.position - Vector3.forward
            );
        }
    }
}