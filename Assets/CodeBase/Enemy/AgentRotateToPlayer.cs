using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class AgentRotateToPlayer : Follow
    {
        [SerializeField] private float _speed = default;

        private Transform _heroTransform;
        private Vector3 _positionToLook;

        public void Construct(Transform heroTransform) => 
            _heroTransform = heroTransform;

        private void Update()
        {
            if (Initialized())
                RotateTowardsHero();
        }

        private void RotateTowardsHero()
        {
            UpdatePositionToLookAt();

            transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
        }

        private void UpdatePositionToLookAt()
        {
            Vector3 positionDiff = _heroTransform.position - transform.position;
            _positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
        }

        private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) =>
            Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());

        private float SpeedFactor() => 
            _speed * Time.deltaTime;

        private Quaternion TargetRotation(Vector3 position) => 
            Quaternion.LookRotation(position);

        private bool Initialized() => 
            _heroTransform != null;
    }
}