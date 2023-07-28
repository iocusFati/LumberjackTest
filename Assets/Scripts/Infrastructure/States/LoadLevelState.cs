using Infrastructure.Factories.UIFactoryFolder;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string PlayerSpawnTag = "PlayerSpawn";
        
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IPlayerFactory _playerFactory;
        private readonly IUIFactory _uiFactory;
        private readonly SceneLoader _sceneLoader;

        private Vector3 _initialPoint;

        public LoadLevelState(IGameStateMachine gameStateMachine,
            IPlayerFactory playerFactory,
            IUIFactory uiFactory, 
            SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _playerFactory = playerFactory;
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
        }
        public void Enter(string sceneName)
        {
            if (sceneName != SceneManager.GetActiveScene().name)
            {
                _sceneLoader.Load(sceneName, OnLoaded);
            }
            else
            {
                Reload();
            }
        }

        public void Exit()
        {
            
        }

        private void OnLoaded()
        {
            _initialPoint = GameObject.FindWithTag(PlayerSpawnTag).transform.position;
            _playerFactory.CreatePlayer(_initialPoint);
            _uiFactory.CreateHUD();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void Reload()
        {
        }
    }
}