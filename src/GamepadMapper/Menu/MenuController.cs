using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GamepadMapper.Menu
{
    public interface IMenuController
    {
        void Show();

        void Hide();

        void UpdatePointer(bool isVisible, double angle);
    }

    public class MenuController : IMenuController, INotifyPropertyChanged
    {
        private bool isMenuOpen;
        private double menuAngle;
        private bool isPointerVisible;

        public bool IsMenuOpen
        {
            get => isMenuOpen;
            set
            {
                if (value != isMenuOpen)
                {
                    isMenuOpen = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsPointerVisible
        {
            get => isPointerVisible;
            set
            {
                if (value != IsPointerVisible)
                {
                    isPointerVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public double MenuAngle
        {
            get => menuAngle;
            set
            {
                menuAngle = value;
                OnPropertyChanged();
            }
        }

        public void Show()
        {
            IsMenuOpen = true;
        }

        public void Hide()
        {
            IsMenuOpen = false;
        }

        public void UpdatePointer(bool isVisible, double angle)
        {
            if (isMenuOpen)
            {
                IsPointerVisible = isVisible;
                if (isVisible)
                {
                    MenuAngle = angle * 180d / Math.PI + 90d;
                }
            }
            else
            {
                isPointerVisible = isVisible;
                if (isVisible)
                {
                    menuAngle = angle * 180d / Math.PI + 90d;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
