using System.Windows;
using System.Windows.Controls;
using GamepadMapper.Configuration;

namespace GamepadMapper.Wpf
{
    public class HelpKeyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate A { get; set; }
        public DataTemplate B { get; set; }
        public DataTemplate X { get; set; }
        public DataTemplate Y { get; set; }
        public DataTemplate DPad { get; set; }
        public DataTemplate DPadHorizontal { get; set; }
        public DataTemplate DPadVertical { get; set; }
        public DataTemplate DPadLeft { get; set; }
        public DataTemplate DPadUp { get; set; }
        public DataTemplate DPadRight { get; set; }
        public DataTemplate DPadDown { get; set; }
        public DataTemplate LB { get; set; }
        public DataTemplate RB { get; set; }
        public DataTemplate LT { get; set; }
        public DataTemplate RT { get; set; }
        public DataTemplate Back { get; set; }
        public DataTemplate Start { get; set; }
        public DataTemplate LSB { get; set; }
        public DataTemplate RSB { get; set; }
        public DataTemplate LS { get; set; }
        public DataTemplate RS { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is HelpKey key)
            {
                if (key >= HelpKey.ModA)
                {
                    key -= HelpKey.ModA;
                }

                switch (key)
                {
                    case HelpKey.LS:
                        return LS;
                    case HelpKey.RS:
                        return RS;
                    case HelpKey.A:
                        return A;
                    case HelpKey.B:
                        return B;
                    case HelpKey.X:
                        return X;
                    case HelpKey.Y:
                        return Y;
                    case HelpKey.DPad:
                        return DPad;
                    case HelpKey.DPadHorizontal:
                        return DPadHorizontal;
                    case HelpKey.DPadVertical:
                        return DPadVertical;
                    case HelpKey.DPadLeft:
                        return DPadLeft;
                    case HelpKey.DPadUp:
                        return DPadUp;
                    case HelpKey.DPadRight:
                        return DPadRight;
                    case HelpKey.DPadDown:
                        return DPadDown;
                    case HelpKey.LB:
                        return LB;
                    case HelpKey.RB:
                        return RB;
                    case HelpKey.LT:
                        return LT;
                    case HelpKey.RT:
                        return RT;
                    case HelpKey.Back:
                        return Back;
                    case HelpKey.Start:
                        return Start;
                    case HelpKey.LSB:
                        return LSB;
                    case HelpKey.RSB:
                        return RSB;
                }
            }


            return base.SelectTemplate(item, container);
        }
    }
}
