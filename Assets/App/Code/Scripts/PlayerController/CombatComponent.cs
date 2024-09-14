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
        [SerializeField] private int maxColliderHits = 5;
        [SerializeField] private float fastAttackDamage = 10.0f;
        [SerializeField] private float stabAttackDamage = 15.0f;

        private Collider[] _hitColliders;

        private void Awake()
        {
            _hitColliders = new Collider[maxColliderHits];
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPivot.position, radius);
        }

        public void DetectFastAttackHit()
        {
            SimpleAttack(fastAttackDamage);
        }

        public void DetectStabbingAttack()
        {
            SimpleAttack(stabAttackDamage);
        }

        private void SimpleAttack(float damage)
        {
            var numColliders = Physics.OverlapSphereNonAlloc(attackPivot.position, radius, _hitColliders);
            if (numColliders == 0) return;

            for (int i = 0; i < numColliders; i++)
            {
                var target = _hitColliders[i].GetComponent<IDamageable>();
                target?.TakeDamage(damage);
            }

            ClearHitCollidersArray();
        }

        private void ClearHitCollidersArray()
        {
            Array.Clear(_hitColliders, 0, _hitColliders.Length);
        }
    }
}