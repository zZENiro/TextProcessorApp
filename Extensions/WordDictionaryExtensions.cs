using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Extensions
{
    public static class WordDictionaryExtensions
    {
        public static int GetFrequency(string word, IEnumerable<string> input) =>
            input.Count(wordInCollection => wordInCollection == word);

        public static List<KeyValuePair<int, string>> GetFrequencyDictionary(IEnumerable<string> input)
        {
            var result = new List<KeyValuePair<int, string>>();

            foreach (var word in input)
            {
                var pair = new KeyValuePair<int, string>(GetFrequency(word, input), word);
                if (!result.Contains(pair))
                    result.Add(pair);
            }

            return result;
        }

        // Метод для отбора слов по условиям 
        // 3 <= WordLenght <= 15; 
        // WordFrequency >= 3
        public static bool CanInsert(string word, IEnumerable<string>  fromInput) =>
            word.Length >= 3    && 
            word.Length <= 15   && 
            fromInput.Count(relatedWord => relatedWord == word) >= 3;

        public static bool CanUpdate(string word) =>
            word.Length >= 3 &&
            word.Length <= 15;
    }
}
