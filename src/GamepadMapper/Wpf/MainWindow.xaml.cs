using System.Windows;
using GamepadMapper.Menus;

namespace GamepadMapper.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IMenuController menuController)
        {
            DataContext = menuController;
            InitializeComponent();
        }
    }
}
