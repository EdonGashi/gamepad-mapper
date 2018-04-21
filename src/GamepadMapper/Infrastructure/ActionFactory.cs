using System.Diagnostics;
using System.Linq;
using WindowsInput;
using GamepadMapper.Actuators;
using GamepadMapper.Configuration;
using GamepadMapper.Menus;

namespace GamepadMapper.Infrastructure
{
    public interface IActionFactoryFactory
    {
        IActionFactory Create(IMenuController menuController);
    }

    public interface IActionFactory
    {
        IAction Create(ActionDescriptor action);
    }

    public class ActionFactory : IActionFactory
    {
        public ActionFactory(RootConfiguration config, IKeyboardSimulator keyboard, IMouseSimulator mouse, IMenuController menuController)
        {
            Config = config;
            Keyboard = keyboard;
            Mouse = mouse;
            MenuController = menuController;
        }

        public RootConfiguration Config { get; }

        public IKeyboardSimulator Keyboard { get; }

        public IMouseSimulator Mouse { get; }

        public IMenuController MenuController { get; }

        public IAction Create(ActionDescriptor action)
        {
            switch (action)
            {
                case CommandAction command:
                    return new DelegateAction(() => MenuController.Dispatch(command.Command));
                case IncrementConfigurationAction increment:
                    return new DelegateAction(() =>
                    {
                        if (Config.Descriptors.TryGetValue(increment.Key, out var descriptor))
                        {
                            descriptor.Decrement();
                        }
                    });
                case DecrementConfigurationAction decrement:
                    return new DelegateAction(() =>
                    {
                        if (Config.Descriptors.TryGetValue(decrement.Key, out var descriptor))
                        {
                            descriptor.Decrement();
                        }
                    });
                case KeyAction keyAction:
                    return new KeyPressActuator(Keyboard, Mouse, keyAction.Keys);
                case Macro macro:
                    var actions = macro.Actions.Select(Create).ToList();
                    return new DelegateAction(() =>
                    {
                        foreach (var a in actions)
                        {
                            a.Execute();
                        }
                    });
                case NoOpAction _:
                    return new DelegateAction(() => { });
                case ResetConfigurationAction reset:
                    return new DelegateAction(() =>
                    {
                        if (Config.Descriptors.TryGetValue(reset.Key, out var descriptor))
                        {
                            descriptor.Reset();
                        }
                    });
                case RunProgramAction runProgramAction:
                    return new DelegateAction(() =>
                    {
                        try
                        {
                            Process.Start(runProgramAction.Path, runProgramAction.Arguments);
                        }
                        catch
                        {
                            // ignored
                        }
                    });
                case SetConfigurationAction set:
                    return new DelegateAction(() =>
                    {
                        if (Config.Descriptors.TryGetValue(set.Key, out var descriptor))
                        {
                            descriptor.SetValue(set.Value);
                        }
                    });
                case ToggleConfigurationAction toggle:
                    return new DelegateAction(() =>
                    {
                        if (Config.Descriptors.TryGetValue(toggle.Key, out var descriptor))
                        {
                            descriptor.Toggle();
                        }
                    });
                case FlashConfigurationAction flashConfiguration:
                    return new DelegateAction(() =>
                    {
                        if (Config.Descriptors.TryGetValue(flashConfiguration.Key, out var descriptor))
                        {
                            MenuController.FlashConfiguration(descriptor);
                        }
                    });
                case FlashMessageAction flashMessage:
                    return new DelegateAction(() =>
                    {
                        MenuController.FlashMessage(flashMessage.Title, flashMessage.Message, flashMessage.Modifier);
                    });
                case ShowMenuAction showMenu:
                    return new DelegateAction(() =>
                    {
                        MenuController.Show(showMenu.Menu);
                    });
                default:
                    return new DelegateAction(() => { });
            }
        }
    }
}
