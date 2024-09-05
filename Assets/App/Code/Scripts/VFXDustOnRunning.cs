using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    public class VFXDustOnRunning : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GameObject dustContainer;
        [SerializeField] private ParticleSystem dustParticles;

        private void OnEnable()
        {
            playerController.OnPlayerRun += OnRunning;
        }

        private void OnDisable()
        {
            playerController.OnPlayerRun -= OnRunning;
        }

        private void OnRunning(bool isRunning)
        {
            if (isRunning)
            {
                dustParticles.Play();
                return;
            }
            dustParticles.Stop();
        }
    }
}
