// <copyright file="ConsoleHelper.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Provides useful console utilities.
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Displays a menu of options to pick from. The selected action is executed.
        /// </summary>
        /// <typeparam name="TResult">The result type of the actions.</typeparam>
        /// <param name="title">The menu title.</param>
        /// <param name="noOptionsText">The text to display in case no options were presented.</param>
        /// <param name="canCancel">Whether to check for Escape presses or not.</param>
        /// <param name="options">A list of options to display to the user alongside their actions.</param>
        /// <returns>The result of the action selected or null if Escape was pressed.</returns>
        public static TResult Menu<TResult>(string title, string noOptionsText, bool canCancel, params (string, Func<TResult>)[] options)
        {
            if (options.Length <= 0)
            {
                ConsoleHelper.Warn(noOptionsText);
                Console.ReadKey();
                return default;
            }

            const int startX = 0;
            const int startY = 2;
            const int optionsPerLine = 1;
            const int spacingPerLine = 14;

            int currentSelection = 0;
            ConsoleKey key;
            Console.CursorVisible = false;

            do
            {
                Console.Clear();
                ConsoleHelper.MenuTitle(title);

                for (int i = 0; i < options.Length; i++)
                {
                    Console.SetCursorPosition(startX + ((i % optionsPerLine) * spacingPerLine), startY + (i / optionsPerLine));

                    if (i == currentSelection)
                    {
                        ConsoleHelper.SelectedOption(options[i].Item1, false);
                    }
                    else
                    {
                        Console.Write(options[i].Item1);
                    }

                    Console.ResetColor();
                }

                Console.WriteLine();
                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        {
                            if (currentSelection % optionsPerLine > 0)
                            {
                                currentSelection--;
                            }

                            break;
                        }

                    case ConsoleKey.RightArrow:
                        {
                            if (currentSelection % optionsPerLine < optionsPerLine - 1)
                            {
                                currentSelection++;
                            }

                            break;
                        }

                    case ConsoleKey.UpArrow:
                        {
                            if (currentSelection >= optionsPerLine)
                            {
                                currentSelection -= optionsPerLine;
                            }

                            break;
                        }

                    case ConsoleKey.DownArrow:
                        {
                            if (currentSelection + optionsPerLine < options.Length)
                            {
                                currentSelection += optionsPerLine;
                            }

                            break;
                        }

                    case ConsoleKey.Escape:
                        {
                            if (canCancel)
                            {
                                return default;
                            }

                            break;
                        }
                }
            }
            while (key != ConsoleKey.Enter);

            Console.CursorVisible = true;
            Console.Clear();

            TResult actionResult = options[currentSelection].Item2.Invoke();

            if (actionResult == default)
            {
                return ConsoleHelper.Menu(title, noOptionsText, canCancel, options);
            }

            return actionResult;
        }

        /// <summary>
        /// Reads a string from the user.
        /// </summary>
        /// <param name="message">A prompt message.</param>
        /// <returns>The string.</returns>
        public static string ReadNonEmptyString(string message)
        {
            while (true)
            {
                ConsoleHelper.Prompt($"{message}:", false);
                var line = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(line))
                {
                    return line;
                }
            }
        }

        /// <summary>
        /// Reads a number from the user.
        /// </summary>
        /// <param name="message">A prompt message.</param>
        /// <returns>The number.</returns>
        public static int ReadNumber(string message)
        {
            while (true)
            {
                ConsoleHelper.Prompt($"{message}:", false);
                string line = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(line))
                {
                    try
                    {
                        return int.Parse(line, CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        ConsoleHelper.Error("Invalid number");
                    }
                }
            }
        }

        /// <summary>
        /// Displays an error message.
        /// </summary>
        /// <param name="message">A message.</param>
        /// <param name="newLine">Whether to add a new line after the message or not.</param>
        public static void Error(string message, bool newLine = true)
        {
            ConsoleHelper.Display(message, ConsoleColor.Red, newLine);
        }

        /// <summary>
        /// Displays a warning message.
        /// </summary>
        /// <param name="message">A message.</param>
        /// <param name="newLine">Whether to add a new line after the message or not.</param>
        public static void Warn(string message, bool newLine = true)
        {
            ConsoleHelper.Display(message, ConsoleColor.Yellow, newLine);
        }

        /// <summary>
        /// Displays an prompt message.
        /// </summary>
        /// <param name="message">A message.</param>
        /// <param name="newLine">Whether to add a new line after the message or not.</param>
        public static void Prompt(string message, bool newLine = true)
        {
            ConsoleHelper.Display(message, ConsoleColor.Gray, newLine);
        }

        /// <summary>
        /// Displays a selected option.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="newLine">Whether to add a new line after the option or not.</param>
        public static void SelectedOption(string option, bool newLine = true)
        {
            ConsoleHelper.Display(option, ConsoleColor.Green, newLine);
        }

        /// <summary>
        /// Displays a menu title.
        /// </summary>
        /// <param name="title">The menu title.</param>
        /// <param name="newLine">Whether to add a new line after the title or not.</param>
        public static void MenuTitle(string title, bool newLine = true)
        {
            ConsoleHelper.Display(title, ConsoleColor.Cyan, newLine);
        }

        /// <summary>
        /// Displays a message on the console.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="color">The color to use.</param>
        /// <param name="newLine">Whether to add a new line after the message or not.</param>
        public static void Display(string message, ConsoleColor color, bool newLine = true)
        {
            Console.ForegroundColor = color;
            if (newLine)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }

            Console.ResetColor();
        }
    }
}
