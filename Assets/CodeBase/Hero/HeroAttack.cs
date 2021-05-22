using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        private HeroAnimator _animator;
        private CharacterController _character;
        private IInputService _input;

        private static int _layerMask;
        private Collider[] _hits = new Collider[3];
        private Stats _stats;

        private void Awake()
        {
            _animator = GetComponent<HeroAnimator>();
            _character = GetComponent<CharacterController>();

            _input = AllServices.Container.Single<IInputService>();
            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if (_input.IsAttackButtonUp() && !_animator.IsAttacking) 
                _animator.PlayAttack();
        }

        public void OnAttack()
        {
            for (int i = 0; i < Hit(); i++) 
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
        }

        public void LoadProgress(PlayerProgress progress) => 
            _stats = progress.HeroStats;

        private int Hit() => 
            Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.Radius, _hits, _layerMask);

        private Vector3 StartPoint() => 
            new Vector3(transform.position.x, _character.center.y / 2, transform.position.z);
    }
}