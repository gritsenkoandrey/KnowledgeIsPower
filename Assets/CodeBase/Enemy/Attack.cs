using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        [SerializeField] private float _attackCooldown = 3f;
        [SerializeField] private float _cleavage = 0.5f;
        [SerializeField] private float _effectiveDistance = 0.5f;
        [SerializeField] private float _damage = 10f;

        private int _layerMask;
        private float _cooldown;
        private bool _isAttacking;
        private bool _attackIsActive;

        private EnemyAnimator _animator;
        private IGameFactory _factory;
        private Transform _heroTransform;
        private Collider[] _hits = new Collider[1];

        private void Awake()
        {
            _animator = GetComponent<EnemyAnimator>();
            _factory = AllServices.Container.Single<IGameFactory>();
            _factory.HeroCreated += OnHeroCreated;
            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            UpdateCooldown();
            
            if (CanAttack())
                StartAttack();
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(StartPoint(), _cleavage, 1);
                
                hit.transform.GetComponent<IHealth>().TakeDamage(_damage);
            }
        }

        private void OnAttackEnded()
        {
            _cooldown = _attackCooldown;
            _isAttacking = false;
        }

        public void EnableAttack() => 
            _attackIsActive = true;

        public void DisableAttack() => 
            _attackIsActive = false;

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _cooldown -= Time.deltaTime;
        }

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            _animator.PlayAttack();

            _isAttacking = true;
        }

        private bool CanAttack() => 
            _attackIsActive && !_isAttacking && CooldownIsUp();

        private bool CooldownIsUp() => 
            _cooldown <= 0;

        private Vector3 StartPoint() => 
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * _effectiveDistance;

        private bool Hit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), _cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitsCount > 0;
        }

        private void OnHeroCreated() => 
            _heroTransform = _factory.HeroGameObject.transform;
    }
}