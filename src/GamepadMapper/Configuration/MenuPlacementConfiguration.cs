using System.ComponentModel;
using System.Runtime.CompilerServices;
using GamepadMapper.Annotations;

namespace GamepadMapper.Configuration
{
    public class MenuPlacementConfiguration : INotifyPropertyChanged
    {
        private double scale = 1d;
        private MenuPosition menuPosition = MenuPosition.BottomRight;

        public double Scale
        {
            get => scale;
            set
            {
                if (value.Equals(scale)) return;
                scale = value;
                OnPropertyChanged();
            }
        }

        public MenuPosition MenuPosition
        {
            get => menuPosition;
            set
            {
                if (value == menuPosition) return;
                menuPosition = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum MenuPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }
}
