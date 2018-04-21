using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using WindowsInput;
using GamepadMapper.Configuration;
using GamepadMapper.Input;
using GamepadMapper.Menus;
using Ninject;
using Ninject.Extensions.Factory;
using Ninject.Modules;

namespace GamepadMapper.Infrastructure
{
    public class ApplicationModule : NinjectModule
    {
        public ApplicationModule(RootConfiguration rootConfiguration)
        {
            RootConfiguration = rootConfiguration;
        }

        public RootConfiguration RootConfiguration { get; }

        public override void Load()
        {
            var simulator = new InputSimulator();
            Bind<IInputSimulator>().ToConstant(simulator);
            Bind<IKeyboardSimulator>().ToConstant(simulator.Keyboard);
            Bind<IMouseSimulator>().ToConstant(simulator.Mouse);
            Bind<RootConfiguration>().ToConstant(RootConfiguration);
            Bind<IActionFactory>().To<ActionFactory>();
            Bind<IActionFactoryFactory>().ToFactory();
            Bind<IProfileFactory>().To<ProfileFactory>();

            Bind<IGamePadStateReader>().To<GamePadStateReader>().InSingletonScope();
            Bind<ProfileCollection>()
                .ToMethod(ctx => ProfileCollection.FromRootConfiguration(
                    RootConfiguration, ctx.Kernel.Get<IProfileFactory>()))
                .InSingletonScope();
            Bind<ApplicationLoop>().ToSelf().InSingletonScope();
            Bind<IMenuController>()
                .ToMethod(ctx => new MenuController(RootConfiguration, ctx.Kernel.Get<IActionFactoryFactory>()))
                .InSingletonScope();
        }
    }
}
