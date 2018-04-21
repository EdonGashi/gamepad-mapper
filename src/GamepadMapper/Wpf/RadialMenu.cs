using System.Windows;
using System.Windows.Controls;
using GamepadMapper.Configuration;
using GamepadMapper.Menus;

namespace GamepadMapper.Wpf
{
    public class RadialMenu : Control
    {
        public static readonly DependencyProperty ArcWidthProperty =
            DependencyProperty.RegisterAttached("ArcWidth",
                typeof(double),
                typeof(RadialMenu),
                new PropertyMetadata(0d));

        public static double GetArcWidth(DependencyObject d)
        {
            return (double)d.GetValue(ArcWidthProperty);
        }

        public static void SetArcWidth(DependencyObject d, double value)
        {
            d.SetValue(ArcWidthProperty, value);
        }

        static RadialMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadialMenu), new FrameworkPropertyMetadata(typeof(RadialMenu)));
        }

        public static readonly DependencyProperty IsPointerVisibleProperty =
            DependencyProperty.Register("IsPointerVisible",
                typeof(bool),
                typeof(RadialMenu),
                new PropertyMetadata(false));

        public static readonly DependencyProperty PointerWidthProperty =
            DependencyProperty.Register("PointerWidth",
                typeof(double),
                typeof(RadialMenu),
                new PropertyMetadata(0d));

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen",
                typeof(bool),
                typeof(RadialMenu),
                new PropertyMetadata(false));

        public static readonly DependencyProperty PointerAngleProperty =
            DependencyProperty.Register("PointerAngle",
                typeof(double),
                typeof(RadialMenu),
                new PropertyMetadata(0d));

        public static readonly DependencyProperty HelpScreenProperty =
            DependencyProperty.Register("HelpScreen",
                typeof(HelpConfiguration),
                typeof(RadialMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty CurrentItemProperty =
            DependencyProperty.Register("CurrentItem",
                typeof(PageItem),
                typeof(RadialMenu),
                new PropertyMetadata(null));

        public PageItem CurrentItem
        {
            get { return (PageItem)GetValue(CurrentItemProperty); }
            set { SetValue(CurrentItemProperty, value); }
        }

        public HelpConfiguration HelpScreen
        {
            get { return (HelpConfiguration)GetValue(HelpScreenProperty); }
            set { SetValue(HelpScreenProperty, value); }
        }

        public bool IsPointerVisible
        {
            get => (bool)GetValue(IsPointerVisibleProperty);
            set => SetValue(IsPointerVisibleProperty, value);
        }

        public double PointerAngle
        {
            get => (double)GetValue(PointerAngleProperty);
            set => SetValue(PointerAngleProperty, value);
        }

        public double PointerWidth
        {
            get => (double)GetValue(PointerWidthProperty);
            set => SetValue(PointerWidthProperty, value);
        }

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }
    }
}
