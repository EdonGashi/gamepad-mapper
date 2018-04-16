using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;
using WindowsInput.Native;
using XInputDotNetPure;

namespace GamepadMapper
{
    public class RootViewModel : INotifyPropertyChanged
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

        public void Initialize(Window window)
        {
            var source = new CancellationTokenSource();
            Task.Run(() => Run(window, source.Token), source.Token);
        }

        private double Distance(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        private Task Run(Window window, CancellationToken token)
        {
            var inputSimulator = new InputSimulator();
            var lastState = GamePad.GetState(PlayerIndex.One);
            var forceY = 0f;
            var queue = new Queue<AngleData>();
            while (true)
            {
                var state = GamePad.GetState(PlayerIndex.One);
                var left = state.ThumbSticks.Left;
                var right = state.ThumbSticks.Right;
                //inputSimulator.Mouse.MoveMouseBy((int)(left.X * 10f), (int)(left.Y * -10f));
                //inputSimulator.Mouse.HorizontalScroll((int)(Math.Round(right.X * 2f)));
                //forceY -= right.Y;
                //if (Math.Abs(forceY) >= 1f)
                //{
                //    forceY = Math.Sign(forceY) * (Math.Abs(forceY) - 1f);
                //    inputSimulator.Mouse.VerticalScroll(Math.Sign(forceY));
                //}

                IsMenuOpen = state.Buttons.LeftShoulder == ButtonState.Pressed;
                if (IsMenuOpen)
                {
                    IsPointerVisible = Distance(left.X, left.Y) > 0.3d;
                    if (IsPointerVisible)
                    {
                        var angle0 = Math.Atan2(left.X, left.Y);
                        queue.Enqueue(new AngleData(angle0));
                        var avgCos = 0d;
                        var avgSin = 0d;
                        foreach (var item in queue)
                        {
                            avgCos += item.Cos;
                            avgSin += item.Sin;
                        }

                        avgCos /= queue.Count;
                        avgSin /= queue.Count;
                        var angle = Math.Atan2(avgSin, avgCos) * 180d / Math.PI;
                        //if (Math.Abs(angle0 - angle) > 120d)
                        //{
                        //    angle = angle0;
                        //}

                        MenuAngle = angle;
                        if (queue.Count >= 30)
                        {
                            queue.Dequeue();
                        }
                    }
                    else
                    {
                        queue.Clear();
                    }
                    //inputSimulator.Mouse.HorizontalScroll((int)(Math.Round(right.X * 2f)));
                }
                else
                {
                    inputSimulator.Mouse.MoveMouseBy((int)(left.X * 10f), (int)(left.Y * -10f));
                }

                if (state.Buttons.A != lastState.Buttons.A)
                {
                    if (state.Buttons.A == ButtonState.Pressed)
                    {
                        //inputSimulator.Mouse.VerticalScroll()
                        //inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VOLUME_UP);
                        //window.Dispatcher.Invoke(() =>
                        //{
                        //    if (!window.IsVisible)
                        //    {
                        //        window.Activate();
                        //    }
                        //    //var stawte = window.Topmost;
                        //    //window.Topmost = false;
                        //    //return window.Topmost = true;
                        //});
                        //inputSimulator.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.MENU, VirtualKeyCode.CONTROL }, VirtualKeyCode.TAB);
                        //inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.VK_A, VirtualKeyCode.VK_B);
                        //inputSimulator.Keyboard.TextEntry('a');
                        //inputSimulator.Mouse.LeftButtonDown();
                    }
                    else
                    {
                        //inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VOLUME_DOWN);
                        //inputSimulator.Mouse.LeftButtonUp();
                    }
                }
                Thread.Sleep(10);
                lastState = state;
            }
        }

        private struct AngleData
        {
            public AngleData(double angle)
            {
                Angle = angle;
                Sin = Math.Sin(angle);
                Cos = Math.Cos(angle);
            }

            public double Angle { get; }

            public double Sin { get; }

            public double Cos { get; }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
