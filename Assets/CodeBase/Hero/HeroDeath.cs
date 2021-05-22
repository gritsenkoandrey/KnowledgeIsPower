using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    [RequireComponent(typeof(HeroHealth))]
    [RequireComponent(typeof(HeroAttack))]
    [RequireComponent(typeof(HeroMove))]
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private GameObject _deathFX = null;
        
        private HeroHealth _health;
        private HeroMove _move;
        private HeroAttack _attack;
        private HeroAnimator _animator;
        
        private bool _isDead;

        private void Awake()
        {
            _health = GetComponent<HeroHealth>();
            _move = GetComponent<HeroMove>();
            _attack = GetComponent<HeroAttack>();
            _animator = GetComponent<HeroAnimator>();
        }

        private void Start() => 
            _health.HealthChanged += HealthChange;

        private void OnDestroy() => 
            _health.HealthChanged -= HealthChange;

        private void HealthChange()
        {
            if (!_isDead && _health.Current <= 0f) 
                Die();
        }

        private void Die()
        {
            _isDead = true;
            
            _move.enabled = false;
            _attack.enabled = false;
            _animator.PlayDeath();

            Instantiate(_deathFX, transform.position, Quaternion.identity);
        }
    }
}