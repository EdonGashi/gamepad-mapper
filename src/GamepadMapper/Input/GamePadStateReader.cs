using XInputDotNetPure;

namespace GamepadMapper.Input
{
    public interface IGamePadStateReader
    {
        GamePadState GetState(PlayerIndex playerIndex);
    }

    public class GamePadStateReader : IGamePadStateReader
    {
        public GamePadState GetState(PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex, GamePadDeadZone.None);
        }
    }
}
