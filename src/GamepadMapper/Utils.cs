using System.Collections.Generic;
using WindowsInput.Native;
using GamepadMapper.Input;

namespace GamepadMapper
{
    internal class Utils
    {
        public static List<VirtualKeyCode> ModifiersToKeys(ModifierKeys keys)
        {
            var list = new List<VirtualKeyCode>(3);
            if (keys.HasFlag(ModifierKeys.Control))
            {
                list.Add(VirtualKeyCode.CONTROL);
            }

            if (keys.HasFlag(ModifierKeys.Alt))
            {
                list.Add(VirtualKeyCode.MENU);
            }

            if (keys.HasFlag(ModifierKeys.Shift))
            {
                list.Add(VirtualKeyCode.SHIFT);
            }

            if (keys.HasFlag(ModifierKeys.WinKey))
            {
                list.Add(VirtualKeyCode.LWIN);
            }

            return list;
        }
    }
}
