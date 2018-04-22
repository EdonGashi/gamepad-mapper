using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GamepadMapper.Configuration;
using GamepadMapper.Infrastructure;
using GamepadMapper.Input;

namespace GamepadMapper.Menus
{
    public class MenuController : IMenuController, INotifyPropertyChanged
    {
        private class MenuState
        {
            public Menu Menu;
            public int Page;
        }

        private readonly Stack<MenuState> menuStack;

        private bool previousIsOpen;
        private double previousPointerAngle;
        private bool previousIsPointerVisible;
        private bool previousItemFocused;
        private Menu previousCurrentMenu;
        private MenuPage previousCurrentPage;
        private PageItem previousCurrentItem;

        private bool isOpen;
        private double pointerAngle;
        private bool isPointerVisible;

        private bool itemFocused;

        private Menu currentMenu;
        private MenuPage currentPage;
        private PageItem currentItem;

        private static readonly PropertyChangedEventArgs IsOpenChanged = new PropertyChangedEventArgs(nameof(IsOpen));
        private static readonly PropertyChangedEventArgs PointerAngleChanged = new PropertyChangedEventArgs(nameof(PointerAngle));
        private static readonly PropertyChangedEventArgs IsPointerVisibleChanged = new PropertyChangedEventArgs(nameof(IsPointerVisible));
        private static readonly PropertyChangedEventArgs ItemFocusedChanged = new PropertyChangedEventArgs(nameof(ItemFocused));
        private static readonly PropertyChangedEventArgs CurrentMenuChanged = new PropertyChangedEventArgs(nameof(CurrentMenu));
        private static readonly PropertyChangedEventArgs CurrentPageChanged = new PropertyChangedEventArgs(nameof(CurrentPage));
        private static readonly PropertyChangedEventArgs CurrentItemChanged = new PropertyChangedEventArgs(nameof(CurrentItem));
        private static readonly PropertyChangedEventArgs PointerWidthChanged = new PropertyChangedEventArgs(nameof(PointerWidth));
        private static readonly PropertyChangedEventArgs HelpScreenChanged = new PropertyChangedEventArgs(nameof(HelpScreen));

        public MenuController(RootConfiguration config, IActionFactoryFactory factory)
        {
            var actionFactory = factory.Create(this);
            MenuCollection = new MenuCollection(config.Menus.ToDictionary(m => m.Name, m => Menu.FromConfig(m, actionFactory)));
            GlobalBindings = CommandBindingCollection.FromCollection(config.Bindings, actionFactory);
            menuStack = new Stack<MenuState>();
        }

        public MenuController(MenuCollection menuCollection, CommandBindingCollection globalBindings)
        {
            MenuCollection = menuCollection;
            GlobalBindings = globalBindings;
            menuStack = new Stack<MenuState>();
        }

        #region Props

        public MenuCollection MenuCollection { get; }

        public CommandBindingCollection GlobalBindings { get; }

        public double PointerAngle => pointerAngle;

        public bool IsPointerVisible => isPointerVisible;

        public bool IsOpen => isOpen;

        public bool ItemFocused => itemFocused;

        public Menu CurrentMenu => currentMenu;

        public MenuPage CurrentPage => currentPage;

        public PageItem CurrentItem => currentItem;

        public double PointerWidth => currentPage != null && currentPage.Items.Count > 1
            ? 360d / currentPage.Items.Count
            : 180d;

        public HelpConfiguration HelpScreen => currentItem?.HelpScreen ?? currentPage?.HelpScreen ?? currentMenu?.HelpScreen;

        #endregion

        public void Show(string menu)
        {
            if (MenuCollection.Menus.TryGetValue(menu, out var menuObject))
            {
                currentMenu = menuObject;
                currentPage = menuObject.Pages[0];
                currentItem = GetCurrentItem();
                itemFocused = false;
                menuStack.Push(new MenuState
                {
                    Menu = menuObject,
                    Page = 0
                });
                isOpen = true;
            }
        }

        public void Back()
        {
            if (menuStack.Count > 1)
            {
                menuStack.Pop();
                var current = menuStack.Peek();
                currentMenu = current.Menu;
                currentPage = current.Menu.Pages[current.Page];
                currentItem = GetCurrentItem();
                itemFocused = false;
                isOpen = menuStack.Count != 0;
            }
            else if (menuStack.Count == 1)
            {
                Exit();
            }
        }

        public void NextPage()
        {
            var menu = currentMenu;
            var page = currentPage;
            if (menu == null || menu.Pages.Count == 1)
            {
                return;
            }

            var index = (page?.Index + 1 ?? 0) % menu.Pages.Count;
            currentPage = menu.Pages[index];
            itemFocused = false;
        }

        public void PreviousPage()
        {
            var menu = currentMenu;
            var page = currentPage;
            if (menu == null || menu.Pages.Count == 1)
            {
                return;
            }

            var index = page?.Index - 1 ?? 0;
            if (index < 0)
            {
                index = menu.Pages.Count - 1;
            }

            currentPage = menu.Pages[index];
            itemFocused = false;
        }

        public void NextItem()
        {
            var page = currentPage;
            var item = currentItem;
            if (page == null)
            {
                return;
            }

            var index = (item?.Index + 1 ?? 0) % page.Items.Count;
            currentItem = page.Items[index];
            pointerAngle = currentItem.Angle;
            isPointerVisible = true;
            itemFocused = true;
        }

        public void PreviousItem()
        {
            var page = currentPage;
            var item = currentItem;
            if (page == null)
            {
                return;
            }

            var index = item?.Index - 1 ?? 0;
            if (index < 0)
            {
                index = page.Items.Count - 1;
            }

            currentItem = page.Items[index];
            pointerAngle = currentItem.Angle;
            isPointerVisible = true;
            itemFocused = true;
        }

        public void Exit()
        {
            menuStack.Clear();
            currentMenu = null;
            currentPage = null;
            currentItem = null;
            itemFocused = false;
            isPointerVisible = false;
            isOpen = false;
        }

        public void FlashConfiguration(IConfigDescriptor descriptor)
        {
        }

        public void FlashMessage(string message, string title, string modifier)
        {
        }

        public void UpdatePointer(bool isVisible, double angle)
        {
            if (itemFocused)
            {
                if (!isVisible)
                {
                    return;
                }

                var nextItem = GetItem(angle);
                if (nextItem == currentItem)
                {
                    return;
                }

                isPointerVisible = true;
                pointerAngle = angle;
                itemFocused = false;
                currentItem = nextItem;
            }
            else
            {
                isPointerVisible = isVisible;
                if (isVisible)
                {
                    pointerAngle = angle;
                }

                currentItem = GetCurrentItem();
            }
        }

        public void Update(FrameDetails frame)
        {
            if (isOpen != previousIsOpen)
            {
                Raise(IsOpenChanged);
                previousIsOpen = isOpen;
            }

            if (isPointerVisible != previousIsPointerVisible)
            {
                Raise(IsPointerVisibleChanged);
                previousIsPointerVisible = isPointerVisible;
            }

            if (itemFocused != previousItemFocused)
            {
                Raise(ItemFocusedChanged);
                previousItemFocused = itemFocused;
            }

            if (Math.Abs(pointerAngle - previousPointerAngle) > 0.1d)
            {
                Raise(PointerAngleChanged);
                previousPointerAngle = pointerAngle;
            }

            var raiseHelp = false;
            if (currentMenu != previousCurrentMenu)
            {
                Raise(CurrentMenuChanged);
                previousCurrentMenu = currentMenu;
                raiseHelp = true;
            }

            if (currentPage != previousCurrentPage)
            {
                Raise(CurrentPageChanged);
                Raise(PointerWidthChanged);
                previousCurrentPage = currentPage;
                raiseHelp = true;
            }

            if (currentItem != previousCurrentItem)
            {
                Raise(CurrentItemChanged);
                previousCurrentItem = currentItem;
                raiseHelp = true;
            }

            if (raiseHelp)
            {
                Raise(HelpScreenChanged);
            }
        }

        public void Dispatch(string command)
        {
            if (command == null)
            {
                return;
            }

            var menu = currentMenu;
            var item = currentItem;

            if (item != null && item.CommandBindings.TryDispatch(command))
            {
                return;
            }

            if (menu != null && menu.CommandBindings.TryDispatch(command))
            {
                return;
            }

            if (GlobalBindings.TryDispatch(command))
            {
                return;
            }

            // Native bindings
            switch (command.ToLower())
            {
                case "menu_back":
                case "native_menu_back":
                    Back();
                    break;
                case "menu_exit":
                case "native_menu_exit":
                    Exit();
                    break;
                case "menu_prev_page":
                case "native_menu_prev_page":
                    PreviousPage();
                    break;
                case "menu_next_page":
                case "native_menu_next_page":
                    NextPage();
                    break;
                case "menu_prev_item":
                case "native_menu_prev_item":
                    PreviousItem();
                    break;
                case "menu_next_item":
                case "native_menu_next_item":
                    NextItem();
                    break;
                case "menu_focus":
                case "native_menu_focus":
                    if (currentItem != null)
                    {
                        pointerAngle = currentItem.Angle;
                        isPointerVisible = true;
                        itemFocused = true;
                    }

                    break;
            }
        }

        private PageItem GetItem(double angle)
        {
            var page = currentPage;
            if (page == null)
            {
                return null;
            }

            if (angle < 0d)
            {
                angle += 360d;
            }

            foreach (var item in page.Items)
            {
                if (item.StartAngle > item.EndAngle)
                {
                    if (angle <= item.EndAngle || angle >= item.StartAngle)
                    {
                        return item;
                    }
                }

                if (angle >= item.StartAngle && angle <= item.EndAngle)
                {
                    return item;
                }
            }

            return null;
        }

        private PageItem GetCurrentItem()
        {
            var hasPointer = isPointerVisible;
            if (!hasPointer)
            {
                return null;
            }

            var page = currentPage;
            if (page == null)
            {
                return null;
            }

            var angle = pointerAngle;
            if (angle < 0d)
            {
                angle += 360d;
            }

            foreach (var item in page.Items)
            {
                if (item.StartAngle > item.EndAngle)
                {
                    if (angle <= item.EndAngle || angle >= item.StartAngle)
                    {
                        return item;
                    }
                }

                if (angle >= item.StartAngle && angle <= item.EndAngle)
                {
                    return item;
                }
            }

            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void Raise(PropertyChangedEventArgs arg)
        {
            PropertyChanged?.Invoke(this, arg);
        }
    }
}
