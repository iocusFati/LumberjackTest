using Infrastructure.AssetProviderService;
using Infrastructure.Factories.UIFactoryFolder;
using Infrastructure.Services.Input;
using Infrastructure.StaticData.PlayerData;
using UnityEngine;

namespace Infrastructure.States
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IAssets _assets;
        private readonly IInputService _inputService;
        private readonly PlayerStaticData _playerStaticData;
        private readonly ITicker _ticker;
        private readonly IUIFactory _uiFactory;

        public PlayerFactory(IAssets assets,
            IInputService inputService,
            PlayerStaticData playerStaticData,
            ITicker ticker, 
            IUIFactory uiFactory)
        {
            _assets = assets;
            _inputService = inputService;
            _playerStaticData = playerStaticData;
            _ticker = ticker;
            _uiFactory = uiFactory;
        }

        public void CreatePlayer(Vector3 at)
        {
            Player player = _assets.Instantiate<Player>(AssetPaths.Player, at);
            player.Construct(_inputService, _uiFactory, _playerStaticData, _ticker);
        }
    }
}