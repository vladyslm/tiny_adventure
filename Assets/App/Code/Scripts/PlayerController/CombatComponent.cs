using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    public class CombatComponent : MonoBehaviour
    {
        [SerializeField] private Transform attackPivot;
        [SerializeField] private float radius = 0.5f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            // Gizmos.DrawSphere(attackPivot.position, radius);
            Gizmos.DrawWireSphere(attackPivot.position, radius);
        }

        public void DetectFastAttackHit()
        {
            var hitColliders = Physics.OverlapSphere(attackPivot.position, radius);
            foreach (var hitCollider in hitColliders)
            {
                var target = hitCollider.GetComponent<IDamageable>();
                if (target == null) return;

                target.TakeDamage(10);
            }
        }
    }
}