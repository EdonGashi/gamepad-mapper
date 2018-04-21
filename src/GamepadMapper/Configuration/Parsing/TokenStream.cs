using System;
using System.Collections.Generic;
using System.Linq;

namespace GamepadMapper.Configuration.Parsing
{
    public class Token
    {
        public Token(string value)
            : this(value, false)
        {
        }

        public Token(string value, bool isString)
        {
            Value = value;
            IsString = isString;
        }

        public string Value { get; }

        public bool IsString { get; }

        public string AsToken => IsString ? "\"" + Value.Replace("\"", "\"\"") + "\"" : Value;

        public override string ToString() => Value;
    }

    public class TokenStream : ITokenStream
    {
        public static List<Token> Tokenize(string line)
        {
            var tokens = new List<Token>();
            var token = "";
            var inString = false;
            void Terminate()
            {
                if (token.Length > 0)
                {
                    tokens.Add(new Token(token));
                    token = "";
                }
            }

            void TerminateWith(char c)
            {
                Terminate();
                tokens.Add(new Token(c.ToString()));
            }

            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (inString)
                {
                    if (c == '"')
                    {
                        var nextC = i < line.Length - 1 ? line[i + 1] : ' ';
                        if (nextC == '"')
                        {
                            i++;
                            token += '"';
                        }
                        else
                        {
                            tokens.Add(new Token(token, true));
                            token = "";
                            inString = false;
                        }
                    }
                    else
                    {
                        token += c;
                    }

                    continue;
                }

                switch (c)
                {
                    case '#':
                        Terminate();
                        return tokens;
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        Terminate();
                        break;
                    case '"':
                        Terminate();
                        inString = true;
                        break;
                    case '(':
                    case ')':
                    case '+':
                    case ',':
                    case ';':
                    case '=':
                        TerminateWith(c);
                        break;
                    default:
                        token += c;
                        break;
                }
            }

            Terminate();
            return tokens;
        }

        private readonly IFileReader fileReader;
        private readonly ILogger logger;
        private readonly Dictionary<string, Token[]> definitionsMap;
        private readonly Stack<ILineReader> stack;
        private Token[] nextLine;

        public TokenStream(ILineReader initialFile, IFileReader fileReader, ILogger logger)
        {
            this.fileReader = fileReader;
            this.logger = logger;
            definitionsMap = new Dictionary<string, Token[]>(StringComparer.OrdinalIgnoreCase);
            stack = new Stack<ILineReader>();
            stack.Push(initialFile);
            ReadLine();
        }

        public Token[] Peek() => nextLine;

        public void Move()
        {
            if (stack.Count > 0)
            {
                stack.Peek().Move();
                ReadLine();
            }
        }

        public string Path => stack.Count > 0 ? stack.Peek().Path : "<eof>";

        public int Line => stack.Count > 0 ? stack.Peek().Line : -1;

        private void ReadLine()
        {
            start:
            if (stack.Count == 0)
            {
                nextLine = null;
                return;
            }

            var reader = stack.Peek();
            var line = reader.Peek();
            if (line == null)
            {
                stack.Pop();
                goto start;
            }

            var tokens = Tokenize(line);
            if (tokens.Count == 0)
            {
                reader.Move();
                goto start;
            }

            var replacements = new List<Token[]>();
            foreach (var token in tokens)
            {
                if (!token.IsString && token.Value.StartsWith("$"))
                {
                    if (definitionsMap.TryGetValue(token.Value.Substring(1), out var newTokens))
                    {
                        replacements.Add(newTokens);
                        continue;
                    }

                    logger?.WriteLine($"Warning: undefined token '{token}' at {reader.Path}:{reader.Line}.");
                }

                replacements.Add(new [] { token });
            }

            var finalTokens = replacements.SelectMany(t => t).ToArray();
            if (finalTokens.Length == 0)
            {
                reader.Move();
                goto start;
            }

            var token1 = finalTokens[0].Value.ToLower();
            switch (token1)
            {
                case "define":
                    if (finalTokens.Length >= 4 && finalTokens[2].Value == "=")
                    {
                        var definedTokens = finalTokens.Skip(3).ToArray();
                        definitionsMap[finalTokens[1].Value] = definedTokens;
                    }
                    else
                    {
                        logger?.WriteLine($"Error: Malformed 'define' at {reader.Path}:{reader.Line}.");
                    }

                    reader.Move();
                    goto start;
                case "include":
                    if (finalTokens.Length > 1)
                    {
                        var path = finalTokens[1];
                        if (fileReader != null)
                        {
                            try
                            {
                                var newReader = fileReader.ReadFile(path.Value);
                                if (newReader == null)
                                {
                                    throw new Exception();
                                }

                                stack.Push(newReader);
                            }
                            catch
                            {
                                logger?.WriteLine($"Error: Could not open file '{path}' at {reader.Path}:{reader.Line}.");
                            }
                        }
                        else
                        {
                            logger?.WriteLine($"Warning: Skipped include '{path}' at {reader.Path}:{reader.Line}.");
                        }
                    }
                    else
                    {
                        logger?.WriteLine($"Error: Malformed 'include' at {reader.Path}:{reader.Line}.");
                    }

                    reader.Move();
                    goto start;
                default:
                    nextLine = finalTokens;
                    break;
            }
        }
    }
}