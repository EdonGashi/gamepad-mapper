using System;

namespace GamepadMapper.Input
{
    [Flags]
    public enum ModifierKeys
    {
        Control = 1 << 0,
        Alt = 1 << 1,
        Shift = 1 << 2,
        WinKey = 1 << 3
    }
}
