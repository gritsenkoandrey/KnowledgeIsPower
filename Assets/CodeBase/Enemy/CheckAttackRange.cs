using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Attack))]
    public class CheckAttackRange : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        
        private Attack _attack;

        private void Awake() => 
            _attack = GetComponent<Attack>();

        private void Start()
        {
            _triggerObserver.TriggerEnter += TriggerEnter;
            _triggerObserver.TriggerExit += TriggerExit;

            _attack.DisableAttack();
        }

        private void TriggerExit(Collider collider) => 
            _attack.DisableAttack();

        private void TriggerEnter(Collider collider) => 
            _attack.EnableAttack();
    }
}