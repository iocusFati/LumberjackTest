using Infrastructure.StaticData.PlayerData;
using UnityEngine;

namespace Infrastructure.States
{
    public class PlayerAnimation
    {
        private readonly Animator _animator;
        private readonly CharacterController _characterController;

        private readonly float _minMoveInputMagnitude;
        private readonly float _minVelocityMagnitude;

        private readonly int _moveHash = Animator.StringToHash("Move");
        private readonly int _chopHash = Animator.StringToHash("Chop");

        public PlayerAnimation(Animator animator, CharacterController characterController, PlayerStaticData playerData)
        {
            _characterController = characterController;
            _animator = animator;

            _minMoveInputMagnitude = playerData.MinMoveInputMagnitude;
            _minVelocityMagnitude = playerData.MinVelocityMagnitude;
        }

        public void SetRunAnimation()
        {
            float move;
            float velocityMagnitude = _characterController.velocity.magnitude * 0.1f;

            if (velocityMagnitude > _minMoveInputMagnitude)
                move = velocityMagnitude < _minVelocityMagnitude
                    ? _minVelocityMagnitude
                    : velocityMagnitude;
            else
            {
                move = 0;
            }

            _animator.SetFloat(_moveHash, move);
        }

        public void SetTreeChopping(bool chop) => 
            _animator.SetBool(_chopHash ,chop);
    }
}