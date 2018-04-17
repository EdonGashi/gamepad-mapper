using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GamepadMapper.Configuration;
using GamepadMapper.Input;
using GamepadMapper.Menu;

namespace GamepadMapper
{
    public class RootViewModel : INotifyPropertyChanged
    {
        public MenuController MenuController { get; } = new MenuController();

        public void Initialize(Window window)
        {
            var source = new CancellationTokenSource();
            Task.Run(() => Run(window, source.Token), source.Token);
        }

        private async Task Run(Window window, CancellationToken token)
        {
            var app = new InputManager();
            await app.Run(new DeadzoneConfiguration(), Utils.GetDefaultProfile(MenuController), 150d);
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
