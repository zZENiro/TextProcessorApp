using System;
using System.Collections.Generic;
using System.Text;
using App.Extensions;

namespace App
{
    public static class InputValidator
    {
        static public bool IsValid(string input) => !StringExtensions.IsEmpty(input)    && 
                                                    input.Length >= 1                   && 
                                                    input.Length <= 15;
    }
}
