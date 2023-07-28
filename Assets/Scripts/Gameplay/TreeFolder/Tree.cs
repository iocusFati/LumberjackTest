using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.TreeFolder
{
    public class Tree : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private float _replantTime;
        [SerializeField] private GameObject _treeWithLeaves;
        [SerializeField] private GameObject _treeWithoutLeaves;
        [SerializeField] private Collider _lumberjackTrigger;
        
        [Header("LeavesParticles")]
        [SerializeField] private List<ParticleSystem> _fallLeavesParticles;
        [SerializeField] private GameObject _fallLeavesHead;

        [Header("Log")] 
        [SerializeField] private Log _log;

        private int _currentHealth;
        
        public bool CanGrow { get; set; }
        public bool IsGrown { get; private set; } = true;

        private void Awake()
        {
            _currentHealth = _health;
        }

        public void TakeDamage(out bool treeDestroyed)
        {
            _currentHealth--;

            if (_currentHealth > 0) 
                PlayLeaveFallParticles();

            HitTree();

            treeDestroyed = !IsGrown;
        }

        private void HitTree()
        {
            if (_currentHealth < _health * 0.5 && _currentHealth > 0)
            {
                RemoveLeaves();
                DeactivateFallLeavesParticles();
            }
            else if (_currentHealth == 0)
            {
                DestroyTree();
            }
        }

        private IEnumerator GrowNewTree()
        {
            yield return new WaitForSeconds(_replantTime);
            yield return new WaitUntil(() => CanGrow);

            DeactivateLog();
            TreeToDefault();
        }

        private void TreeToDefault()
        {
            IsGrown = true;
            _treeWithLeaves.SetActive(true);
            _fallLeavesHead.SetActive(true);
            _currentHealth = _health;
        }

        private void DestroyTree()
        {
            _treeWithoutLeaves.SetActive(false);

            StartCoroutine(GrowNewTree());
            ActivatePickupLog();

            IsGrown = false;
        }

        private void ActivatePickupLog()
        {
            _log.gameObject.SetActive(true);
        }

        private void RemoveLeaves()
        {
            _treeWithLeaves.SetActive(false);
            _treeWithoutLeaves.SetActive(true);
        }

        private void DeactivateFallLeavesParticles()
        {
            _fallLeavesHead.SetActive(false);
        }

        private void PlayLeaveFallParticles()
        {
            foreach (var particle in _fallLeavesParticles)
                particle.Play();
        }

        private void DeactivateLog()
        {
            if (_log.gameObject.activeSelf)
                _log.gameObject.SetActive(false);
        }
    }
}