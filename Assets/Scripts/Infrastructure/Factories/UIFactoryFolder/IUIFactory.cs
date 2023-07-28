using System;
using Infrastructure.Services;
using UI.HUDFolder;

namespace Infrastructure.Factories.UIFactoryFolder
{
    public interface IUIFactory : IService
    {
        HUD CreateHUD();
        event Action<HUD> OnHUDCreated;
    }
}