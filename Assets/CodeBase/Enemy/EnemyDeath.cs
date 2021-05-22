using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(EnemyAnimator))]

    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private GameObject _deathFX = null;

        private EnemyHealth _health;
        private EnemyAnimator _animator;

        public event Action Happened;
        
        private void Awake()
        {
            _health = GetComponent<EnemyHealth>();
            _animator = GetComponent<EnemyAnimator>();
        }

        private void Start() => 
            _health.HealthChanged += HealthChanged;

        private void OnDestroy()
        {
            if (_health != null) 
                _health.HealthChanged -= HealthChanged;
        }

        private void HealthChanged()
        {
            if (_health.Current <= 0) 
                Die();
        }

        private void Die()
        {
            _health.HealthChanged -= HealthChanged;
            _animator.PlayDeath();
            SpawnDeathFX();
            StartCoroutine(DestroyTimer());
            Happened?.Invoke();
        }

        private void SpawnDeathFX() => 
            Instantiate(_deathFX, transform.position, Quaternion.identity);

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}