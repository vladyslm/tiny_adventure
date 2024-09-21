using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    public class CombatComponent : MonoBehaviour
    {
        [Header("Attack Settings")] [SerializeField]
        private Transform attackPivot;

        [SerializeField] private float radius = 0.5f;
        [SerializeField] private int maxColliderHits = 5;
        [SerializeField] private float fastAttackDamage = 10.0f;
        [SerializeField] private float stabAttackDamage = 15.0f;

        [Header("Spinning Attack")] [SerializeField]
        private Transform aoeAttackPivot;

        [SerializeField] private float spinningAttack = 7f;
        [SerializeField] private float spinningAttackRadius = 1.7f;

        private PlayerController _player;
        private Collider[] _hitColliders;
        private bool _isSpinningAttack;

        private void Awake()
        {
            _hitColliders = new Collider[maxColliderHits];
            _player = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            _player.OnSpinAttackStart += OnSpinAttackStart;
            _player.OnSpinAttackEnd += OnSpinAttackEnd;
        }

        private void OnDisable()
        {
            _player.OnSpinAttackStart -= OnSpinAttackStart;
            _player.OnSpinAttackEnd -= OnSpinAttackEnd;
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

        private void OnSpinAttackStart()
        {
            _isSpinningAttack = true;
            StartCoroutine(SpinningAttack(spinningAttack));
        }

        private void OnSpinAttackEnd()
        {
            _isSpinningAttack = false;
        }

        private IEnumerator SpinningAttack(float damage)
        {
            if (!_isSpinningAttack) yield break;
            var numColliders =
                Physics.OverlapSphereNonAlloc(aoeAttackPivot.position, spinningAttackRadius, _hitColliders);
            if (numColliders == 0) yield break;

            for (int i = 0; i < numColliders; i++)
            {
                var target = _hitColliders[i].GetComponent<IDamageable>();
                target?.TakeDamage(damage);
            }

            ClearHitCollidersArray();
            yield return new WaitForSeconds(0.3f);
            yield return SpinningAttack(damage);
        }
    }
}