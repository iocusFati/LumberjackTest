using Infrastructure;
using Infrastructure.States;
using Infrastructure.StaticData.PlayerData;
using UnityEngine;
using Tree = Gameplay.TreeFolder.Tree;

namespace Gameplay.Player.Lumberjack
{
    public class PlayerLumberjack : ILateTickable
    {
        private readonly PlayerAnimation _playerAnimation;
        
        private readonly GameObject _axe;
        private readonly Transform _playerTransform;
        private readonly ParticleSystem _woodHitParticle;
        
        private readonly Quaternion _angleBetweenTreeAndPlayer;

        private Tree _currentlyChoppedTree;
        private Vector3 _currentChopPosition;
        private bool _isChopping;

        public PlayerLumberjack(PlayerAnimation playerAnimation,
            GameObject axe,
            Transform player,
            ParticleSystem woodHitParticle,
            ITicker ticker,
            PlayerStaticData playerData)
        {
            _playerAnimation = playerAnimation;
            
            _axe = axe;
            _playerTransform = player;
            _woodHitParticle = woodHitParticle;
            
            _angleBetweenTreeAndPlayer = Quaternion.Euler(playerData.AngleBetweenTreeAndPlayer);
            
            ticker.AddLateTickable(this);
        }

        public void LateTick()
        {
            if (_isChopping) 
                _playerTransform.position = _currentChopPosition;
        }

        public void Chop(Tree tree)
        {
            _currentChopPosition = _playerTransform.position;
            _currentlyChoppedTree = tree;
            _isChopping = true;
            
            _axe.SetActive(true);

            RotateWithOffsetTo(tree);

            _playerAnimation.SetTreeChopping(true);
        }

        private void RotateWithOffsetTo(Tree tree)
        {
            RotateTowards(tree);
            _playerTransform.rotation *= _angleBetweenTreeAndPlayer;
        }

        private void RotateTowards(Tree tree)
        {
            Vector3 directionToTheTree = Vector3.Normalize(tree.transform.position - _playerTransform.position);
            float currentAngleBetweenTreeAndPlayer = Vector3.Angle(_playerTransform.forward, directionToTheTree);
            // Debug.Log("Before: " + _playerTransform.rotation.eulerAngles);
            
            _playerTransform.rotation *= Quaternion.AngleAxis(-currentAngleBetweenTreeAndPlayer, Vector3.up);
            
            // Debug.Log("After: " + _playerTransform.rotation.eulerAngles);
        }

        public void DamageTree(out bool treeDestroyed)
        {
            _currentlyChoppedTree.TakeDamage(out treeDestroyed);
            
            if (treeDestroyed) 
                StopChopping();

            _woodHitParticle.gameObject.SetActive(true);
            _woodHitParticle.transform.SetParent(null);
        }

        private void StopChopping()
        {
            _isChopping = false;
            _playerAnimation.SetTreeChopping(false);
        }
    }
}