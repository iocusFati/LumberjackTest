using System;
using System.Collections;
using Gameplay.TreeFolder;
using Infrastructure.Factories.UIFactoryFolder;
using Infrastructure.StaticData.PlayerData;
using UI.HUDFolder;
using UnityEngine;

namespace Infrastructure.States
{
    public class PlayerResourcePicker
    {
        private readonly ICoroutineRunner _coroutineRunner;
        
        private readonly float _giveAwayDuration;
        private readonly Transform _resourceTarget;
        private HUD _hud;

        public PlayerResourcePicker(ICoroutineRunner coroutineRunner, 
            PlayerStaticData playerData,
            Transform resourceTarget, 
            IUIFactory uiFactory)
        {
            _coroutineRunner = coroutineRunner;
            _resourceTarget = resourceTarget;

            _giveAwayDuration = playerData.GiveAwayResourceDuration;

            uiFactory.OnHUDCreated += hud => _hud = hud;
        }

        public void Pick(Log log)
        {
            _coroutineRunner.StartCoroutine(RotateTo(log.transform, _resourceTarget));
            _coroutineRunner.StartCoroutine(MoveTo(log.transform, _resourceTarget,
                () =>
                {
                    log.gameObject.SetActive(false);
                    _hud.RaiseScore();
                }));
        }
        
        private IEnumerator MoveTo(Transform product, Transform target, Action onComplete = null)
        {
            float timeElapsed = 0f;
            Vector3 startPos = product.position;

            while (timeElapsed < _giveAwayDuration)
            {
                float t = timeElapsed / _giveAwayDuration;
                product.position = Vector3.Lerp(startPos, target.position, t);

                timeElapsed += Time.deltaTime;
                yield return null;
            }
            
            product.position = target.position;
            onComplete.Invoke();
        }
        
        private IEnumerator RotateTo(Transform product, Transform target)
        {
            float timeElapsed = 0f;
            Quaternion startRot = product.rotation;

            while (timeElapsed < _giveAwayDuration)
            {
                float t = timeElapsed / _giveAwayDuration;
                product.rotation = Quaternion.Lerp(startRot, target.rotation, t);

                timeElapsed += Time.deltaTime;
                yield return null;
            }
            
            product.rotation = target.rotation;
        }
    }
}