﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WindowsInput.Native;
using GamepadMapper.Input;

namespace GamepadMapper.Configuration.Parsing
{
    public static class Parser
    {
        public static RootConfiguration Parse(string path)
        {
            using (var logger = new FileLogger())
            {
                try
                {
                    return Parse(new TokenStream(new LineReader(path), new FileReader(), logger), logger);
                }
                catch (Exception ex)
                {
                    logger.WriteLine($"Error while reading configuration file '{path}'.");
                    logger.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        public static RootConfiguration Parse(ITokenStream stream, ILogger logger)
        {
            var config = new RootConfiguration();
            while (true)
            {
                var tokens = stream.PeekNonEmpty();
                if (tokens == null)
                {
                    break;
                }

                var token0 = tokens[0].Value.ToLower();
                var token1 = tokens.Length > 1 ? tokens[1].Value : null;
                var token2 = tokens.Length > 2 ? tokens[2].Value : null;
                switch (token0)
                {
                    case "bind":
                        var binding = ParseCommandBinding(stream, logger);
                        if (binding != null)
                        {
                            config.Bindings.Add(binding);
                        }

                        stream.Move();
                        break;
                    case "profile":
                        var profile = ParseProfile(stream, logger);
                        if (profile != null)
                        {
                            config.Profiles.Add(profile);
                        }

                        break;
                    case "menu":
                        var menu = ParseMenuConfiguration(stream, logger);
                        if (menu != null)
                        {
                            config.Menus.Add(menu);
                        }

                        break;
                    default:
                        if (tokens.Length < 3 || token1 != "=")
                        {
                            logger?.WriteLine($"Error: Malformed line at {stream.Path}:{stream.Line}.");
                            break;
                        }

                        if (token0 == "fps")
                        {
                            if (double.TryParse(token2, out var fps) && fps >= 1d && fps <= 300d)
                            {
                                config.Fps = fps;
                            }
                            else
                            {
                                logger?.WriteLine(
                                    $"Warning: Invalid FPS value '{token2}' at '{token0}' at {stream.Path}:{stream.Line}.");
                            }
                        }
                        else if (config.Descriptors.TryGetValue(token0, out var descriptor))
                        {
                            if (!descriptor.isValidValue(token2))
                            {
                                logger?.WriteLine(
                                    $"Warning: Value '{token2}' is invalid or out of range for key '{token0}' at {stream.Path}:{stream.Line}. Default value is assigned.");
                            }

                            descriptor.SetValue(token2);
                        }
                        else
                        {
                            logger?.WriteLine($"Error: Unknown key '{token0}' at {stream.Path}:{stream.Line}.");
                        }

                        stream.Move();
                        break;
                }
            }

            config.InitializeProfiles();
            return config;
        }

        public static MenuConfiguration ParseMenuConfiguration(ITokenStream stream, ILogger logger)
        {
            if (!stream.Starts("menu", logger, out var name))
            {
                return null;
            }

            HelpConfiguration help = null;
            var bindings = new List<CommandBinding>();
            var pages = new List<PageConfiguration>();
            while (true)
            {
                if (stream.Ends("menu", logger, out var row, out var token0))
                {
                    break;
                }

                switch (token0)
                {
                    case "bind":
                        var binding = ParseCommandBinding(stream, logger);
                        if (binding != null)
                        {
                            bindings.Add(binding);
                        }

                        stream.Move();
                        continue;
                    case "help":
                        var h = ParseHelp(stream, logger);
                        if (h != null)
                        {
                            help = h;
                        }

                        continue;
                    case "page":
                        var page = ParsePage(stream, logger);
                        if (page != null)
                        {
                            pages.Add(page);
                        }

                        continue;
                    default:
                        logger?.UnexpectedLine(stream);
                        continue;
                }
            }

            return name != null ? new MenuConfiguration(name, help, bindings, pages) : null;
        }

        public static PageConfiguration ParsePage(ITokenStream stream, ILogger logger)
        {
            if (!stream.Starts("page"))
            {
                return null;
            }

            HelpConfiguration help = null;
            var items = new List<MenuItemConfiguration>();
            while (true)
            {
                if (stream.Ends("page", logger, out var row, out var token0))
                {
                    break;
                }

                switch (token0)
                {
                    case "help":
                        var h = ParseHelp(stream, logger);
                        if (h != null)
                        {
                            help = h;
                        }

                        continue;
                    case "item":
                        var item = ParseMenuItem(stream, logger);
                        if (item != null)
                        {
                            items.Add(item);
                        }

                        continue;
                    default:
                        logger?.UnexpectedLine(stream);
                        continue;
                }
            }

            return new PageConfiguration(help, items);
        }

        public static MenuItemConfiguration ParseMenuItem(ITokenStream stream, ILogger logger)
        {
            if (!stream.Starts("item"))
            {
                return null;
            }

            HelpConfiguration help = null;
            var bindings = new List<CommandBinding>();
            string name = null;
            string text = null;
            List<Token> icon = null;
            while (true)
            {
                if (stream.Ends("item", logger, out var row, out var token0))
                {
                    break;
                }

                if (token0 == "help")
                {
                    var h = ParseHelp(stream, logger);
                    if (h != null)
                    {
                        help = h;
                    }

                    continue;
                }

                if (token0 == "bind")
                {
                    var binding = ParseCommandBinding(stream, logger);
                    if (binding != null)
                    {
                        bindings.Add(binding);
                    }

                    stream.Move();
                    continue;
                }

                if (row.Length < 3 || row[1].Value != "=")
                {
                    logger?.WriteLine($"Error: Malformed assignment at {stream.Path}:{stream.Line}.");
                    stream.Move();
                    continue;
                }

                var rest = string.Join(" ", row.Skip(2));
                switch (token0)
                {
                    case "text":
                        text = rest;
                        break;
                    case "icon":
                        icon = row.Skip(2).ToList();
                        break;
                    case "name":
                        name = rest;
                        break;
                    default:
                        logger?.UnexpectedLine(stream);
                        break;
                }

                stream.Move();
            }

            return new MenuItemConfiguration(name, icon, text, bindings, help);
        }

        public static HelpConfiguration ParseHelp(ITokenStream stream, ILogger logger)
        {
            if (!stream.Starts("help"))
            {
                return null;
            }

            string text = null;
            var inputHelpTexts = new List<InputHelpText>();
            while (true)
            {
                if (stream.Ends("help", logger, out var row, out var token0))
                {
                    break;
                }

                if (row.Length < 3 || row[1].Value != "=")
                {
                    logger?.WriteLine($"Error: Malformed assignment at {stream.Path}:{stream.Line}.");
                    stream.Move();
                    continue;
                }

                var rest = string.Join(" ", row.Skip(2));
                if (token0 == "text")
                {
                    text = rest;
                }
                else if (Enum.TryParse(token0, true, out HelpKey helpKey))
                {
                    inputHelpTexts.Add(new InputHelpText(helpKey, rest));
                }
                else
                {
                    logger?.WriteLine($"Warning: Invalid symbol '{token0}' at {stream.Path}:{stream.Line}.");
                }

                stream.Move();
            }

            return new HelpConfiguration(text, inputHelpTexts);
        }

        public static ProfileConfiguration ParseProfile(ITokenStream stream, ILogger logger)
        {
            if (!stream.Starts("profile", logger, out var name))
            {
                return null;
            }

            var bindings = new List<InputBinding>();
            while (true)
            {
                if (stream.Ends("profile", logger, out var row, out var token0))
                {
                    break;
                }

                if (row.Length < 4 || row[1].Value != "=")
                {
                    logger?.WriteLine($"Error: Malformed key binding at {stream.Path}:{stream.Line}.");
                    stream.Move();
                    continue;
                }

                if (token0 == "ls" || token0 == "rs")
                {
                    if (!string.Equals(row[2].Value, "map"))
                    {
                        logger?.WriteLine($"Error: Unknown analog mapping at {stream.Path}:{stream.Line}.");
                        stream.Move();
                        continue;
                    }

                    var stick = token0 == "ls" ? ThumbStick.Left : ThumbStick.Right;
                    switch (row[3].Value.ToLower())
                    {
                        case "mouse":
                            bindings.Add(new MouseMapping(stick));
                            break;
                        case "scroll":
                            bindings.Add(new ScrollMapping(stick));
                            break;
                        case "radial_menu":
                            bindings.Add(new RadialMenuMapping(stick));
                            break;
                        default:
                            logger?.WriteLine($"Error: Unknown analog mapping at {stream.Path}:{stream.Line}.");
                            break;
                    }

                    stream.Move();
                    continue;
                }

                if (!Enum.TryParse(token0, true, out InputKey inputKey))
                {
                    logger?.WriteLine($"Error: Unknown key '{token0}' at {stream.Path}:{stream.Line}.");
                }

                int end;
                switch (row[2].Value.ToLower())
                {
                    case "map":
                        if (string.Equals(row[3].Value, "mod", StringComparison.OrdinalIgnoreCase))
                        {
                            bindings.Add(new ModMapping(inputKey));
                            break;
                        }

                        var keys = ParseKeyCodes(row, 3, out end);
                        if (keys == null)
                        {
                            logger?.WriteLine($"Error: Invalid key code(s) at {stream.Path}:{stream.Line}.");
                        }
                        else
                        {
                            if (end < row.Length)
                            {
                                logger?.WriteLine($"Warning: Ignored unknown symbols at {stream.Path}:{stream.Line}.");
                            }

                            bindings.Add(new KeyMapping(inputKey, keys));
                        }

                        break;
                    case "press":
                        var action = ParseAction(row, 3, out end);
                        if (action == null)
                        {
                            logger?.WriteLine($"Error: Invalid action at {stream.Path}:{stream.Line}.");
                            break;
                        }

                        if (end < row.Length)
                        {
                            var nextToken = row[end];
                            if (string.Equals(nextToken.Value, "repeat", StringComparison.OrdinalIgnoreCase))
                            {
                                bindings.Add(new PressBinding(inputKey, action, true));
                            }
                            else if (string.Equals(nextToken.Value, "hold", StringComparison.OrdinalIgnoreCase))
                            {
                                end++;
                                if (end >= row.Length)
                                {
                                    logger?.WriteLine($"Error: Expected action after 'hold' at {stream.Path}:{stream.Line}.");
                                    break;
                                }

                                var holdAction = ParseAction(row, end, out end);
                                if (holdAction == null)
                                {
                                    logger?.WriteLine($"Error: Invalid action at {stream.Path}:{stream.Line}.");
                                    break;
                                }

                                if (end < row.Length)
                                {
                                    logger?.WriteLine($"Warning: Ignored unknown symbols at {stream.Path}:{stream.Line}.");
                                }

                                bindings.Add(new PressHoldBinding(inputKey, action, holdAction));
                            }
                            else
                            {
                                logger?.WriteLine($"Error: Invalid symbol '{nextToken}' at {stream.Path}:{stream.Line}.");
                            }
                        }
                        else
                        {
                            bindings.Add(new PressBinding(inputKey, action, false));
                        }

                        break;
                    default:
                        logger?.WriteLine($"Error: Unknown symbol '{row[2].Value}' at {stream.Path}:{stream.Line}.");
                        break;
                }

                stream.Move();
            }

            if (name == null)
            {
                return null;
            }

            return new ProfileConfiguration(name, bindings);
        }

        public static List<VirtualKeyCode> ParseKeyCodes(Token[] tokens, int start, out int end)
        {
            Token PeekToken(int offset = 0)
            {
                var index = start + offset;
                return index < tokens.Length ? tokens[index] : null;
            }

            void NextToken(int num = 1)
            {
                start = Math.Min(start + num, tokens.Length);
            }

            var token0 = PeekToken();
            if (token0 == null)
            {
                end = start;
                return null;
            }

            var keys = new List<VirtualKeyCode>();
            if (Enum.TryParse(token0.Value, true, out VirtualKeyCode keyCode))
            {
                keys.Add(keyCode);
                NextToken();
            }
            else
            {
                end = start;
                return null;
            }

            token0 = PeekToken();
            while (token0 != null && token0.Value == "+")
            {
                NextToken();
                token0 = PeekToken();
                if (token0 != null && Enum.TryParse(token0.Value, true, out keyCode))
                {
                    keys.Add(keyCode);
                }
                else
                {
                    end = start;
                    return null;
                }

                NextToken();
                token0 = PeekToken();
            }

            end = start + 1;
            return keys;
        }

        public static ActionDescriptor ParseAction(Token[] tokens, int start, out int end)
        {
            Token PeekToken(int offset = 0)
            {
                var index = start + offset;
                return index < tokens.Length ? tokens[index] : null;
            }

            void NextToken(int num = 1)
            {
                start = Math.Min(start + num, tokens.Length);
            }

            var actions = new List<ActionDescriptor>();

            parseStart:

            var token0 = PeekToken();
            if (token0 == null)
            {
                goto parseEnd;
            }

            var token1 = PeekToken(1);
            if (string.Equals(token0.Value, "nothing", StringComparison.OrdinalIgnoreCase))
            {
                NextToken();
            }
            else if (token1 != null && token1.Value == "(")
            {
                var call = ParseCall(tokens, start, out var callEnd);
                if (call == null)
                {
                    end = start;
                    return null;
                }

                actions.Add(call);
                start = callEnd;
            }
            else if (Enum.TryParse(token0.Value, true, out VirtualKeyCode keyCode))
            {
                var keys = new List<VirtualKeyCode> { keyCode };
                while (token1 != null && token1.Value == "+")
                {
                    NextToken(2);
                    token0 = PeekToken();
                    token1 = PeekToken(1);
                    if (token0 != null && Enum.TryParse(token0.Value, true, out keyCode))
                    {
                        keys.Add(keyCode);
                    }
                    else
                    {
                        end = start;
                        return null;
                    }
                }

                actions.Add(new KeyAction(keys));
            }

            token0 = PeekToken();
            if (token0 != null && token0.Value == ";")
            {
                NextToken();
                goto parseStart;
            }

            parseEnd:

            end = start + 1;
            switch (actions.Count)
            {
                case 0:
                    return new NoOpAction();
                case 1:
                    return actions[0];
                default:
                    return new Macro(actions);
            }
        }

        private static CommandBinding ParseCommandBinding(ITokenStream stream, ILogger logger)
        {
            var tokens = stream.Peek();
            if (tokens == null)
            {
                return null;
            }

            if (tokens.Length > 3 && tokens[2].Value == "=")
            {
                var command = tokens[1].Value;
                var action = ParseAction(tokens, 3, out var end);
                if (action != null)
                {
                    if (end != tokens.Length)
                    {
                        logger?.WriteLine(
                            $"Warning: Ignored unknown symbols at {stream.Path}:{stream.Line}.");
                    }

                    return new CommandBinding(command, action);
                }

                action = new NoOpAction();
                logger?.WriteLine($"Error: Could not parse action at {stream.Path}:{stream.Line}.");

                return new CommandBinding(command, action);
            }

            logger?.WriteLine($"Error: Malformed binding at {stream.Path}:{stream.Line}.");
            return null;
        }

        private static ActionDescriptor ParseCall(Token[] tokens, int start, out int end)
        {
            Token PeekToken(int offset = 0)
            {
                var index = start + offset;
                return index < tokens.Length ? tokens[index] : null;
            }

            void NextToken(int num = 1)
            {
                start = Math.Min(start + num, tokens.Length);
            }

            var token0 = PeekToken();
            var token1 = PeekToken(1);
            if (token1 == null || token1.Value != "(")
            {
                end = start;
                return null;
            }

            var call = token0.Value.ToLower();
            NextToken(2);
            var args = new List<string>();
            while (true)
            {
                end = start;
                var token = PeekToken();
                if (token == null)
                {
                    return null;
                }

                if (!token.IsString && token.Value == ")")
                {
                    break;
                }

                var isComma = !token.IsString && token.Value == ",";
                if (args.Count > 0)
                {
                    if (!isComma)
                    {
                        return null;
                    }

                    NextToken();
                    token = PeekToken();
                    if (token == null || !token.IsString && token.Value == ",")
                    {
                        end = start;
                        return null;
                    }

                    args.Add(token.Value);
                }
                else
                {
                    if (isComma)
                    {
                        return null;
                    }

                    args.Add(token.Value);
                }

                NextToken();
            }

            end = start;
            switch (call)
            {
                case "command":
                    return args.Count < 1 ? null : new CommandAction(args[0]);
                case "show":
                    return args.Count < 1 ? null : new ShowMenuAction(args[0]);
                case "run":
                    return args.Count < 1 ? null : new RunProgramAction(args[0], args.Count > 1 ? args[1] : null);
                case "flashcfg":
                    return args.Count < 1 ? null : new FlashConfigurationAction(args[0]);
                case "flashmsg":
                    if (args.Count < 1)
                    {
                        return null;
                    }

                    var title = args[0];
                    var text = args.Count > 1 ? args[1] : null;
                    var modifier = args.Count > 2 ? args[2] : null;
                    return new FlashMessageAction(title, text, modifier);
                case "increment":
                    return args.Count < 1 ? null : new IncrementConfigurationAction(args[0]);
                case "decrement":
                    return args.Count < 1 ? null : new DecrementConfigurationAction(args[0]);
                case "toggle":
                    return args.Count < 1 ? null : new ToggleConfigurationAction(args[0]);
                case "reset":
                    return args.Count < 1 ? null : new ResetConfigurationAction(args[0]);
                case "set":
                    return args.Count < 2 ? null : new SetConfigurationAction(args[0], args[1]);
                case "sendchar":
                    return args.Count >= 1 && args[0].Length == 1 ? new SendCharacterAction(args[0][0]) : null;
                case "sendstr":
                    return args.Count >= 1 ? new SendStringAction(args[0]) : null;
                default:
                    return null;
            }
        }
    }
}
