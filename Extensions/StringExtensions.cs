using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace App.Extensions
{
    public static class StringExtensions
    {
        static public bool IsEmpty(this string input) => string.IsNullOrEmpty(input) || input.ToString().All(c => c == ' ');

        static public void Add(this StringBuilder stringBuilder, string input) =>
            stringBuilder.Append(
                input.ToString() != "Spacebar"
                ? input.ToString()
                : " ");


        static public void PopCharacter(this StringBuilder stringBuilder)
        {
            if (stringBuilder.Length != 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                Console.Write(' ');
                Console.CursorLeft--;
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            }
        }
    }
}
