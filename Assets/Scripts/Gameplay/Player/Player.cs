using System;
using CameraLogic;
using Gameplay.Player.Lumberjack;
using Gameplay.TreeFolder;
using Infrastructure.Factories.UIFactoryFolder;
using Infrastructure.Services.Input;
using Infrastructure.StaticData.PlayerData;
using UnityEngine;
using Tree = Gameplay.TreeFolder.Tree;

namespace Infrastructure.States
{
    public class Player : MonoBehaviour, ICoroutineRunner
    {
        private const string TreeTag = "Tree";
        private const string LogTag = "Log";

        [Header("Player")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;
        
        [Header("Axe")]
        [SerializeField] private float _axeChopRadius;
        [SerializeField] private float _reloadHitTime;
        [SerializeField] private float _warmUpBeforeFirstHitTime;
        [SerializeField] private LayerMask _hitLayer;
        [SerializeField] private Transform _axeHead;
        [SerializeField] private GameObject _axe;
        [SerializeField] private ParticleSystem _woodHitParticle;

        [Header("Resources")]
        [SerializeField] private Transform _resourceTarget;

        private ITicker _ticker;
        private PlayerMovement _playerMovement;
        private PlayerAnimation _playerAnimation;
        private PlayerLumberjack _playerLumberjack;
        private CameraFollow _cameraFollow;
        private CameraShaking _cameraShaking;

        private float _startedChoppingTime;
        private float _lastHitTime;
        private bool _isChopping;
        private PlayerResourcePicker _playerResourcePicker;

        public void Construct(IInputService inputService, IUIFactory uiFactory, PlayerStaticData playerData, ITicker ticker)
        {
            Transform camera = Camera.main.transform;
            _cameraShaking = camera.GetComponent<CameraShaking>();
            _cameraFollow = camera.GetComponent<CameraFollow>();

            CameraFollowPlayer();
            
            _playerMovement = new PlayerMovement(inputService, _characterController, camera, transform, playerData);
            _playerAnimation = new PlayerAnimation(_animator, _characterController, playerData);
            _playerLumberjack = new PlayerLumberjack(_playerAnimation, _axe, transform, _woodHitParticle, ticker, playerData);
            _playerResourcePicker = new PlayerResourcePicker(this, playerData, _resourceTarget, uiFactory);
        }

        private void Update()
        {
            _playerMovement.Move();
            _playerMovement.Rotate();
            
            _playerAnimation.SetRunAnimation();
        }

        private void FixedUpdate()
        {
            if (!_isChopping) 
                return;
            if (Time.time - _startedChoppingTime < _warmUpBeforeFirstHitTime)
                return;

            if (HasHitTheTree() && Reloaded())
            {
                _lastHitTime = Time.time;
                _playerLumberjack.DamageTree(out bool treeDestroyed);
                _cameraShaking.StartShaking();

                if (treeDestroyed) 
                    StopChopping();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TreeTag))
            {
                Tree tree = other.GetComponent<Tree>();
                tree.CanGrow = false;
                
                if (!tree.IsGrown)
                    return;

                Chop(tree);
            }
            else if (other.CompareTag(LogTag))
            {
                _playerResourcePicker.Pick(other.GetComponent<Log>());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TreeTag))
            {
                Tree tree = other.GetComponent<Tree>();
                tree.CanGrow = true;
            }
        }

        private void Chop(Tree tree)
        {
            _isChopping = true;
            _startedChoppingTime = Time.time;
            _playerLumberjack.Chop(tree);
            
            FreezePlayer(true);
        }

        private bool HasHitTheTree()
        {
            Collider[] colliders = new Collider[5];
            int size = Physics.OverlapSphereNonAlloc(_axeHead.position, _axeChopRadius, colliders, _hitLayer);

            return size > 0;
        }

        private void FreezePlayer(bool freeze)
        {
            _playerMovement.FreezeMovement(freeze);
            _cameraFollow.FreezeFollowing(freeze);
        }

        private void StopChopping()
        {
            _isChopping = false;
            FreezePlayer(false);
        }

        private void CameraFollowPlayer() => 
            _cameraFollow.Follow(gameObject);

        private bool Reloaded() => 
            Time.time - _lastHitTime > _reloadHitTime;
    }
}