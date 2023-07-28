using Gameplay.Player.Lumberjack;

namespace Infrastructure
{
    public interface ITicker
    {
        void AddTickable(ITickable tickable);
        void AddLateTickable(ILateTickable playerLumberjack);
    }
}