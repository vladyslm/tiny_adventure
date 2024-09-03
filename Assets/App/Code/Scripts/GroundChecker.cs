using System;
using UnityEngine;

namespace TinyAdventure
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private float groundDistance = 0.08f;
        [SerializeField] LayerMask groundLayer;
        
        public bool IsGrounded { get; private set; }

        private void Update()
        {
            IsGrounded = Physics.SphereCast(transform.position, groundDistance, Vector3.down, out _, groundDistance, groundLayer);
        }
    }
}