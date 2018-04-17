using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.IconPacks;
using Microsoft.Expression.Shapes;

namespace GamepadMapper
{
    /// <summary>
    /// Interaction logic for RadialMenu.xaml
    /// </summary>
    public partial class RadialMenu : UserControl
    {
        public RadialMenu()
        {
            InitializeComponent();
            var str = "abcdefghijklmnopqrstuvwxyz";
            str = "Abcdef";
            var icons = new PackIconMaterialKind[]
            {
                PackIconMaterialKind.Settings,
                PackIconMaterialKind.VolumeMute,
                PackIconMaterialKind.VolumeHigh,
                PackIconMaterialKind.VolumeLow,
                PackIconMaterialKind.Keyboard,
                PackIconMaterialKind.Star
            };
            var rand = new Random();
            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                var angle = i * 360d / str.Length;
                var rad = angle * Math.PI / 180d;
                //var block = new TextBlock
                //{
                //    Text = c.ToString(),
                //    FontSize=25d,
                //    TextAlignment = TextAlignment.Center
                //};
                var block = new PackIconMaterial
                {
                    Kind = icons[i],
                    Width = 36d,
                    Height = 36d,
                    Foreground = new SolidColorBrush(Color.FromRgb(64, 64, 64))
                };

                var centerX = 150d;
                var centerY = 150d;
                var dx = 120d * Math.Cos(rad);
                var dy = 120d * Math.Sin(rad);
                Canvas.SetLeft(block, centerX + dx - 18d);
                Canvas.SetTop(block, centerY + dy - 18);
                Canvas.Children.Add(block);
            }
        }

        public bool IsPointerVisible
        {
            get => (bool)GetValue(IsPointerVisibleProperty);
            set => SetValue(IsPointerVisibleProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsPointerVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPointerVisibleProperty =
            DependencyProperty.Register("IsPointerVisible", typeof(bool), typeof(RadialMenu), new PropertyMetadata(false, IsPointerVisibleChanged));

        private static void IsPointerVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = (RadialMenu)d;
            var newValue = (bool)e.NewValue;
            if (!newValue)
            {
                var animation = new DoubleAnimation(0d, new Duration(TimeSpan.FromMilliseconds(200d)))
                {
                    EasingFunction = new QuarticEase
                    {
                        EasingMode = EasingMode.EaseOut
                    }
                };

                menu.BeginAnimation(PointerWidthProperty, animation);
            }
            else
            {
                var animation = new DoubleAnimation(360d / 6d, new Duration(TimeSpan.FromMilliseconds(200d)))
                {
                    EasingFunction = new QuarticEase
                    {
                        EasingMode = EasingMode.EaseOut
                    }
                };

                menu.BeginAnimation(PointerWidthProperty, animation);
            }
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

        // Using a DependencyProperty as the backing store for PointerWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointerWidthProperty =
            DependencyProperty.Register("PointerWidth", typeof(double), typeof(RadialMenu), new PropertyMetadata(0d));

        // Using a DependencyProperty as the backing store for PointerAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointerAngleProperty =
            DependencyProperty.Register("PointerAngle", typeof(double), typeof(RadialMenu), new PropertyMetadata(0d, AngleChanged));

        private static void AngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = (RadialMenu)d;
            //var oldValue = (double)e.OldValue;

            //menu.Pointer.StartAngle = newValue - 20d;
            //menu.Pointer.EndAngle = newValue + 20d;
            //if (Math.Abs(newValue - oldValue) > 60d)
            //{
            //    var animation = new DoubleAnimation(newValue - 20d, new Duration(TimeSpan.FromMilliseconds(100d)));
            //    var animation2 = new DoubleAnimation(newValue + 20d, new Duration(TimeSpan.FromMilliseconds(100d)));
            //    menu.Pointer.BeginAnimation(Arc.StartAngleProperty, animation);
            //    menu.Pointer.BeginAnimation(Arc.EndAngleProperty, animation2);
            //}
            //else
            ////{
            //menu.Pointer.StartAngle = newValue - 20d;
            //menu.Pointer.EndAngle = newValue + 20d;
            //}
            //new DoubleAnimationUsingKeyFrames()
            //{

            //}
        }

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(RadialMenu), new FrameworkPropertyMetadata(false, IsOpenChanged));

        private static void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = (RadialMenu)d;
            var oldValue = (bool)e.OldValue;
            var newValue = (bool)e.NewValue;
            if (oldValue != newValue)
            {
                menu.Pointer.Visibility = newValue ? Visibility.Visible : Visibility.Hidden;
                ((Storyboard)menu.FindResource(newValue ? "Show" : "Hide")).Begin();
            }
        }
    }
}
