using Infrastructure.Services.Input;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class GameLostState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IInputService _inputService;
        private readonly ICoroutineRunner _coroutineRunner;
        
        private readonly float _timeBeforeLose;
        private readonly float _cameraRotateDuration;
        
        public GameLostState(
            IGameStateMachine gameStateMachine,
            IInputService inputService,
            ICoroutineRunner coroutineRunner)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _coroutineRunner = coroutineRunner;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
        
        private void RestartGame()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(SceneManager.GetActiveScene().name);
        }
    }
}