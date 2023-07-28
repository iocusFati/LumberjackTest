using System.Collections;
using UnityEngine;

namespace Gameplay.TreeFolder
{
    public class Log : MonoBehaviour
    {
        [SerializeField] private float _initialBounciness;
        [SerializeField] private float _activateTriggerDelay;
        [SerializeField] private Collider _lumberjackTrigger;
        [SerializeField] private PhysicMaterial _physicMaterial;

        private Rigidbody _rb;
        
        private int _timeCollided;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
        }

        private void OnDisable()
        {
            LogToInitial();
        }

        private void OnEnable()
        {
            _physicMaterial.bounciness = _initialBounciness;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_timeCollided == 0) 
                StartCoroutine(EnableLumberjackTrigger());

            _timeCollided++;
        }

        private IEnumerator EnableLumberjackTrigger()
        {
            yield return new WaitForSeconds(_activateTriggerDelay);
            
            _lumberjackTrigger.enabled = true;
            _physicMaterial.bounciness *= 0.5f;
        }

        private void LogToInitial()
        {
            _timeCollided = 0;
            
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            _lumberjackTrigger.enabled = false;
            
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
        }
    }
}