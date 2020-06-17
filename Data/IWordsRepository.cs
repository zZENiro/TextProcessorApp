using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Data
{
    public interface IWordsRepository
    {
        public IEnumerable<Word> GetAll();

        public void UpdateOrCreateMany(IEnumerable<Word> words);

        public void CreateMany(IEnumerable<Word> words);

        public void DeleteAll();
    }
}
