using UnityEngine;

namespace CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float _rotationAngleX;
        [SerializeField] private float _distance;
        [SerializeField] private float _offsetY;

        private Transform _following;
        private bool _isFollowing;

        private void LateUpdate()
        {
            if (_following == null || !_isFollowing)
                return;

            Quaternion rotation = Quaternion.Euler(_rotationAngleX, 0, 0);
            Vector3 position = rotation * new Vector3(0, 0, -_distance) + FollowingPointPosition();

            transform.rotation = rotation;
            transform.position = position;
        }

        public void Follow(GameObject following)
        {
            _isFollowing = true;
            _following = following.transform;
        }

        public void FreezeFollowing(bool freeze) => 
            _isFollowing = !freeze;

        private Vector3 FollowingPointPosition()
        {
            Vector3 followingPosition = _following.position;
            followingPosition.y += _offsetY;

            return followingPosition;
        }
    }
}