using Infrastructure.Services.Input;
using Infrastructure.StaticData.PlayerData;
using UnityEngine;
using Utilities;

namespace Infrastructure.States
{
    public class PlayerMovement
    {
        private readonly IInputService _inputService;
        private readonly CharacterController _characterController;
        
        private readonly float _movementSpeed;
        private readonly float _rotationSpeed;
        private readonly Transform _playerCamera;
        private readonly Transform _playerTransform;

        private bool _isFrozen;
        private Vector3 _movementVector;

        public PlayerMovement(IInputService inputService,
            CharacterController characterController,
            Transform playerCamera,
            Transform playerTransform, 
            PlayerStaticData playerData)
        {
            _inputService = inputService;
            _characterController = characterController;
            _playerCamera = playerCamera;
            _playerTransform = playerTransform;

            _movementSpeed = playerData.MovementSpeed;
            _rotationSpeed = playerData.RotationSpeed;
        }

        public void Rotate()
        {
            if (_movementVector != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(_movementVector, Vector3.up);

                _playerTransform.rotation = Quaternion.RotateTowards(_playerTransform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
            }
        }

        public void Move()
        {
            if (_isFrozen)
            {
                _characterController.Move(Vector3.zero);
                return;
            }

            _movementVector = Vector3.zero;

            if (_inputService.Axis.magnitude > Constants.Epsilon)
            {
                _movementVector = _playerCamera.TransformDirection(_inputService.Axis);
                _movementVector = new Vector3(_movementVector.x, 0, _movementVector.y);
                
            }

            _movementVector.y = 0;
            
            _characterController.Move(_movementVector * (_movementSpeed * Time.deltaTime));
        }

        public void FreezeMovement(bool freeze) => 
            _isFrozen = freeze;
    }
}