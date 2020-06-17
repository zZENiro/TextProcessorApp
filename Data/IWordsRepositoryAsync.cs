using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Data
{
    public interface IWordsRepositoryAsync : IWordsRepository
    {
        public Task<IEnumerable<Word>> GetAllAsync();

        public Task UpdateOrCreateManyAsync(IEnumerable<Word> words);

        public Task CreateManyAsync(IEnumerable<Word> words);

        public Task DeleteAllAsync();
    }
}
