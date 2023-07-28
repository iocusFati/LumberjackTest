using System;
using System.Collections.Generic;
using Infrastructure.Factories.UIFactoryFolder;
using Infrastructure.Services;
using Infrastructure.Services.Input;

namespace Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitState> _states;
        private IExitState _currentState;
        
        public GameStateMachine(
            SceneLoader sceneLoader, 
            AllServices services,
            ITicker ticker,
            ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IExitState>
            {
                [typeof(BootstrapState)] = new BootstrapState(
                    this, sceneLoader, services, coroutineRunner, ticker),
                [typeof(LoadLevelState)] = new LoadLevelState(
                    this, services.Single<IPlayerFactory>(),
                    services.Single<IUIFactory>(), sceneLoader),
                [typeof(GameLoopState)] = new GameLoopState(),
                [typeof(GameLostState)] = new GameLostState(
                    this, services.Single<IInputService>(), coroutineRunner)
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitState
        {
            _currentState?.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitState => 
            _states[typeof(TState)] as TState;
    }
}