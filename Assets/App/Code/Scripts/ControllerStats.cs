using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    [CreateAssetMenu(menuName = "TinyAdventure/ControllerStats")]
    public class ControllerStats : ScriptableObject
    {
        [Header("Movement")] public float moveSpeed = 6f;
        public float rotationSpeed = 15f;
        public float smoothTime = .2f;

        [Header("Jump")] public float jumpForce = 15f;
        public float jumpMaxHeight = 2f;
        public float jumpDuration = 0.5f;
        public float jumpCoolDown = 0;
        public float gravityMultiplier = 3f;

        [Header("Dash")] public float dashForce = 2f;
        public float dashDuration = 0.2f;
        public float dashCoolDown = 0f;

        [Header("Attack")] public float attackDuration = 0.1f;
        public float attackCooldown = 0;
    }
}