using System;

namespace GamepadMapper.Actuators
{
    public class DelegateAction : IAction
    {
        public DelegateAction(Action action)
        {
            Action = action;
        }

        public Action Action { get; }

        public void Execute()
        {
            Action.Invoke();
        }
    }
}
