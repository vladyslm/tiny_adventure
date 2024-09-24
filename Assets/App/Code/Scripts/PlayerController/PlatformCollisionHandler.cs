using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace TinyAdventure
{
    public class PlatformCollisionHandler : MonoBehaviour
    {
        private PlayerController _player;
        private Transform _platform;

        private void Awake()
        {
            _player = GetComponent<PlayerController>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.CompareTag("MovingPlatform")) return;
            var contact = other.GetContact(0);
            if (contact.normal.y < 0.5f) return;
                
            _platform = other.transform;
            transform.SetParent(_platform);
            SetUpdateMethod(CinemachineBrain.UpdateMethod.LateUpdate);
        }

        private void OnCollisionExit(Collision other)
        {
            if (!other.transform.CompareTag("MovingPlatform")) return;
            transform.SetParent(null);
            _platform = null;
            SetUpdateMethod(CinemachineBrain.UpdateMethod.FixedUpdate);
        }

        private void SetUpdateMethod(CinemachineBrain.UpdateMethod updateMethod)
        {
            _player.CinemachineBrain.m_UpdateMethod = updateMethod;
        }
    }
}