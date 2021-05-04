using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float _rotationAngleX = 55.0f;
        [SerializeField] private float _distance = 10.0f;
        [SerializeField] private float _offsetY = 0.5f;
        [SerializeField] private Transform _following = null;

        private void LateUpdate()
        {
            if (_following == null)
            {
                return;
            }

            Quaternion rotation = Quaternion.Euler(_rotationAngleX, 0, 0);
            
            var position = rotation * new Vector3(0, 0, -_distance) 
                           + FollowingPointPosition();

            transform.rotation = rotation;
            transform.position = position;
        }

        public void Follow(GameObject following) => 
            _following = following.transform;

        private Vector3 FollowingPointPosition()
        {
            Vector3 followingPosition = _following.position;
            followingPosition.y += _offsetY;
            
            return followingPosition;
        }
    }
}