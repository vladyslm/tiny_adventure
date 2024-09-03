using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    public class PlatformCollisionHandler : MonoBehaviour
    {
        private Transform _platform;

        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.CompareTag("MovingPlatform")) return;
            var contact = other.GetContact(0);
            if (contact.normal.y < 0.5f) return;
                
            _platform = other.transform;
            transform.SetParent(_platform);
        }

        private void OnCollisionExit(Collision other)
        {
            if (!other.transform.CompareTag("MovingPlatform")) return;
            transform.SetParent(null);
            _platform = null;
        }
    }
}