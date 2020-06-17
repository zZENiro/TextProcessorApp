using System;
using System.Collections.Generic;
using System.Text;

namespace App.Extensions
{
    public static class ConsoleKeyExtesnions
    {
        static public bool IsBackspace(this ConsoleKey key) => key == ConsoleKey.Backspace;

        static public bool IsEnter(this ConsoleKey key) => key == ConsoleKey.Enter;

        static public bool IsEscape(this ConsoleKey key) => key == ConsoleKey.Escape;
    }
}
