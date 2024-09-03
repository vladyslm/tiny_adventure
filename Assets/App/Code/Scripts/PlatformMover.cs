using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TinyAdventure
{
    public class PlatformMover : MonoBehaviour
    {
        [SerializeField] private Vector3 moveTo = Vector3.zero;
        [SerializeField] private float moveTime = 1f;
        [SerializeField] private Ease ease = Ease.InOutQuad;

        private Vector3 _startPos;

        private void Start()
        {
            _startPos = transform.position;
            Move();
        }

        private void Move()
        {
            transform.DOMove(_startPos + moveTo, moveTime).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
        }
    }
}