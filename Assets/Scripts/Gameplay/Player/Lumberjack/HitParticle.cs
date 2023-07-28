using System;
using UnityEngine;

namespace Gameplay.Player.Lumberjack
{
    public class HitParticle : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        
        private Vector3 _initialLocalPos;

        private void Awake() => 
            _initialLocalPos = transform.localPosition;

        private void OnParticleSystemStopped()
        {
            transform.SetParent(_parent);
            transform.localPosition = _initialLocalPos;
            
            gameObject.SetActive(false);
        }
    }
}