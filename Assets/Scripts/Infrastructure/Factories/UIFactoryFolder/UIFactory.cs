using System;
using Infrastructure.AssetProviderService;
using UI.HUDFolder;
using UnityEngine;

namespace Infrastructure.Factories.UIFactoryFolder
{
    public class UIFactory : IUIFactory
    {
        private const int _hudRootOrder = 0;

        private readonly IAssets _assetProvider;

        private Transform _uiRoot;

        public event Action<HUD> OnHUDCreated;

        public UIFactory(IAssets assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public HUD CreateHUD()
        {
            _uiRoot = CreateUIRoot(_hudRootOrder);
            
            var hudDisplay = _assetProvider.Instantiate<HUD>(AssetPaths.HUD, _uiRoot);
            OnHUDCreated.Invoke(hudDisplay);
            
            return hudDisplay;
        }
        
        private Transform CreateUIRoot(int order)
        {
            var canvas = _assetProvider.Instantiate<Canvas>(AssetPaths.UIRoot);
            // canvas.sortingOrder = order;

            return canvas.transform;
        }
    }
}