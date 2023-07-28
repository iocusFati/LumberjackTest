using Infrastructure.AssetProviderService;
using Infrastructure.Factories.UIFactoryFolder;
using Infrastructure.Services;
using Infrastructure.Services.Input;
using Infrastructure.Services.StaticDataService;
using UnityEngine;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string InitialSceneName = "Initial";
        private const string MainSceneName = "Game";
        
        private readonly IGameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ITicker _ticker;

        public BootstrapState(
            IGameStateMachine gameStateMachine,
            SceneLoader sceneLoader,
            AllServices services,
            ICoroutineRunner coroutineRunner,
            ITicker ticker)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _coroutineRunner = coroutineRunner;
            _ticker = ticker;

            RegisterServices(services);
        }

        public void Enter()
        {
            _sceneLoader.Load(InitialSceneName, OnLoaded);
        }

        public void Exit()
        {
            
        }

        private void OnLoaded()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(MainSceneName);
        }

        private void RegisterServices(AllServices services)
        {
            var staticData = RegisterStaticDataService(services);
            var assets = services.RegisterService<IAssets>(
                new AssetProvider());
            var inputService = services.RegisterService<IInputService>(
                InputService());

            var uiFactory = services.RegisterService<IUIFactory>(
                new UIFactory(assets));
            var playerFactory = services.RegisterService<IPlayerFactory>(
                new PlayerFactory(assets, inputService, staticData.PlayerData, _ticker, uiFactory));
        }

        private IStaticDataService RegisterStaticDataService(AllServices services)
        {
            var staticDataService = new StaticDataService();
            staticDataService.Initialize();
            
            return services.RegisterService<IStaticDataService>(staticDataService);
        }
        
        private IInputService InputService() =>
            Application.isEditor
                ? new StandaloneInput()
                : new MobileInput();
    }
}