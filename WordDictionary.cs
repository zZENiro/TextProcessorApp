using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using App.Data;

using static App.Extensions.WordDictionaryExtensions;

namespace App
{
    public class WordDictionary
    {
        public List<Word> Words { get; set; }

        public WordDictionary(IWordsRepository repository) =>
            Words = (repository as IWordsRepositoryAsync).GetAllAsync().Result.ToList();

        public async Task InsertAsync(IEnumerable<string> input)
        {
            var sortedInput = SortToInsert(input);
            var freqWordsDict = GetFrequencyDictionary(sortedInput);

            foreach (var weightedWord in freqWordsDict)
                Words.Add(new Word(weightedWord.Value, weightedWord.Key));
        }

        public async Task UpdateOrInsertAsync(IEnumerable<string> input)
        {
            var sortedInput = SortToUpdate(input);
            var freqWordsDict = GetFrequencyDictionary(sortedInput);

            foreach (var weightedWord in freqWordsDict)
            {
                var word = Words.Where(w => w.Value == weightedWord.Value).FirstOrDefault();

                if (!Words.Contains(word))
                {
                    var newWord = new Word(weightedWord.Value, weightedWord.Key);
                    Words.Add(newWord);
                }
                else
                    Words.Where(exsitingWord => exsitingWord.Id == word.Id)
                         .First().Frequency += GetFrequency(word.Value, sortedInput);
            }
        }

        // returns 5 closest words to INPUT by desc frequency
        public async Task<List<Word>> GetContinuesAsync(string input)
        {
            var result = (from word in Words
                       orderby word.Frequency descending
                       where word.Value.StartsWith(input)
                       select word).Take(5).ToList();

            for (int i = 0; i < result.Count; ++i)
                for (int j = 0; j < result.Count; j++)
                    if (result[j].Frequency == result[i].Frequency)
                        if (string.CompareOrdinal(result[j].Value, result[i].Value) > 0)
                        {
                            var cur_i = result[i];
                            result[i] = result[j];
                            result[j] = cur_i;
                        }

            return result;
        }

        private List<string> SortToInsert(IEnumerable<string> input)
        {
            var sortedOutput = new List<string>();
            foreach (var word in input)
                if (CanInsert(word, input))
                    sortedOutput.Add(word);

            return sortedOutput;
        }

        private List<string> SortToUpdate(IEnumerable<string> input)
        {
            var sortedOutput = new List<string>();
            foreach (var word in input)
                if (CanUpdate(word))
                    sortedOutput.Add(word);

            return sortedOutput;
        }
    }
}
