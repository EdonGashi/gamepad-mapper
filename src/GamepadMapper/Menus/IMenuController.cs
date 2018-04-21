﻿using GamepadMapper.Configuration;
using GamepadMapper.Input;

namespace GamepadMapper.Menus
{
    public interface IMenuController
    {
        Menu CurrentMenu { get; }

        MenuPage CurrentPage { get; }

        PageItem CurrentItem { get; }

        bool ItemFocused { get; }

        double PointerAngle { get; }

        double PointerWidth { get; }

        bool IsPointerVisible { get; }

        bool IsOpen { get; }

        HelpConfiguration HelpScreen { get; }

        void Show(string menu);

        void Back();

        void NextPage();

        void PreviousPage();

        void NextItem();

        void PreviousItem();

        void Exit();

        void FlashConfiguration(IConfigDescriptor descriptor);

        void FlashMessage(string message, string title, string modifier);

        void UpdatePointer(bool isVisible, double angle);

        void Update(FrameDetails frame);

        void Dispatch(string command);
    }
}