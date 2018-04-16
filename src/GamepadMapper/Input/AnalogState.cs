using System;

namespace GamepadMapper.Input
{
    public struct AnalogState
    {
        public AnalogState(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }

        public double Y { get; }

        public double Angle => Math.Atan2(Y, X);

        public double Distance => Math.Sqrt(X * X + Y * Y);

        public double DistanceSquared => X * X + Y * Y;
    }
}