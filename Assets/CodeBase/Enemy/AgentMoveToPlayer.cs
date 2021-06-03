using CodeBase.Infrastructure.Factory;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoveToPlayer : Follow
    {
        private const float MinimalDistance = 1f;

        private NavMeshAgent _agent;
        private Transform _heroTransform;
        private IGameFactory _gameFactory;

        public void Construct(Transform heroTransform) => 
            _heroTransform = heroTransform;

        private void Awake() => 
            _agent = GetComponent<NavMeshAgent>();

        private void Update()
        {
            if (Initialized() && HeroNotReached()) 
                _agent.destination = _heroTransform.position;
        }

        private bool Initialized() => 
            _heroTransform != null;

        private bool HeroNotReached() => 
            (_agent.transform.position - _heroTransform.position).sqrMagnitude >= MinimalDistance;
    }
}