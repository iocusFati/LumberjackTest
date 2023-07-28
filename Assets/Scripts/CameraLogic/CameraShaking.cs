using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CameraLogic
{
    public class CameraShaking : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _shakeStrengthCurve;
        [SerializeField] private Vector3 _shakeMinShift;
        [SerializeField] private float _shakeMinMagnitude;
        [SerializeField] private float _shakeTime;
        
        public void StartShaking() => 
            StartCoroutine(Shake());

        private IEnumerator Shake()
        {
            Vector3 shakePos = Random.insideUnitSphere;
            shakePos = shakePos.magnitude < _shakeMinMagnitude ? _shakeMinShift : shakePos;
            
            Vector3 startPos = transform.localPosition;
            
            float elapsedTime = 0;

            while (elapsedTime < _shakeTime)
            {
                elapsedTime += Time.deltaTime;
                float shakeStrength = _shakeStrengthCurve.Evaluate(elapsedTime / _shakeTime);
                
                shakePos *= -1;
                transform.localPosition = startPos + shakePos * shakeStrength;

                yield return null;
            }
        }
    }
}