using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyAdventure
{
    public class DummyHealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private float startHealth = 50f;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject popup;
        [SerializeField] private Transform popupPivot;

        private float _health;
        private bool _isDown;
        private Collider _collider;

        private readonly int _pushAnimationHash = Animator.StringToHash("pushed");
        private readonly int _diedAnimationHash = Animator.StringToHash("died");

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _health = startHealth;
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;

            if (!_isDown) SpawnDamagePopup(damage);

            if (_health <= 0)
            {
                Die();
                return;
            }

            Hit();
        }

        private void Hit()
        {
            animator.Play(_pushAnimationHash, -1, 0f);
        }

        private void Die()
        {
            _isDown = true;
            HandleCollider(!_isDown);
            animator.Play(_diedAnimationHash);
            StartCoroutine(Reborn());
        }

        private void SpawnDamagePopup(float damage)
        {
            var damagePopup = Instantiate(popup, popupPivot.position, Quaternion.identity).GetComponent<DamagePopup>();
            damagePopup.SetDamage(damage);
        }

        private IEnumerator Reborn()
        {
            yield return new WaitForSeconds(2f);
            _health = startHealth;
            _isDown = false;
            HandleCollider(!_isDown);
            animator.Play(_pushAnimationHash);
        }

        private void HandleCollider(bool value)
        {
            _collider.enabled = value;
        }
    }
}