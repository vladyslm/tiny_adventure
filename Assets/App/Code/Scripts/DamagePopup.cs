using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace TinyAdventure
{
    public class DamagePopup : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public float lifetime;
        public float minDistance;
        public float maxDistance;

        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private float _timer;
        private Transform _cameraTransform;

        private void OnEnable()
        {
            if (Camera.main != null) _cameraTransform = Camera.main.transform;
        }
        
        void Start()
        {
            if (Camera.main != null) transform.LookAt(Camera.main.transform);
        
            _startPosition = transform.position;
            var distance = Random.Range(maxDistance, minDistance);
            _targetPosition = _startPosition + new Vector3(0, distance, 0f);
        }
        
        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= lifetime) Destroy(gameObject);
            
            transform.LookAt(transform.position - _cameraTransform.position);
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, Mathf.Sin(_timer / lifetime));
        }

        public void SetDamage(float damage)
        {
            text.text = $"{damage}";
        }
    }
}