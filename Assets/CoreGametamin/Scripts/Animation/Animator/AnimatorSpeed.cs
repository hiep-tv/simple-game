using Gametamin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MergeGame
{
    public class AnimatorSpeed : MonoBehaviour
    {
        [SerializeField] private float _speed = 1.0f;

        Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            SetSpeed(_speed);
        }

        private void SetSpeed(float speed)
        {
            if (_animator == null) return;
            _animator.speed = speed;
        }

#if DEBUG_MODE
        private void OnValidate()
        {
            if (_animator == null) return;
            _animator.speed = _speed;
        }
#endif
    }
}
