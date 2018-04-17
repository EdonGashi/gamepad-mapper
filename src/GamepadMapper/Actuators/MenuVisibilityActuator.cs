using GamepadMapper.Menu;

namespace GamepadMapper.Actuators
{
    public class MenuVisibilityActuator : IMapping
    {
        public MenuVisibilityActuator(IMenuController menuController)
        {
            MenuController = menuController;
        }

        public IMenuController MenuController { get; }

        public void Activate()
        {
            MenuController.Show();
        }

        public void Deactivate()
        {
            MenuController.Hide();
        }
    }
}
