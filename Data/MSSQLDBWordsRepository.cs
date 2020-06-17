using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace App.Data
{
    class MSSQLDBWordsRepository : IWordsRepositoryAsync
    {
        WordsContext _ctx;

        public MSSQLDBWordsRepository(IConfiguration configuration) =>
            _ctx = new WordsContext(configuration);

        public void CreateMany(IEnumerable<Word> words)
        {
            _ctx.Words.AddRange(words);
            _ctx.SaveChanges();
        }

        public async Task CreateManyAsync(IEnumerable<Word> words)
        {
            await _ctx.Words.AddRangeAsync(words);
            await _ctx.SaveChangesAsync();
        }

        public void DeleteAll()
        {
            _ctx.Words.FromSqlRaw<Word>("TRUNCATE TABLE wordsList_tbl");
            _ctx.SaveChanges();
        }

        public async Task DeleteAllAsync()
        {
            await Task.Factory.StartNew(() => { _ctx.Words.FromSqlRaw<Word>("TRUNCATE TABLE wordsList_tbl"); });
            await _ctx.SaveChangesAsync();
        }

        public IEnumerable<Word> GetAll() => _ctx.Words.ToList<Word>();

        public async Task<IEnumerable<Word>> GetAllAsync() => await _ctx.Words.ToListAsync<Word>();

        public void UpdateOrCreateMany(IEnumerable<Word> words)
        {
            var toAdd = new List<Word>();
            var src = GetAll();
            var srcValues = src.Select(w => w.Value).ToList();

            foreach (var word in words)
                if (!srcValues.Contains(word.Value))
                    toAdd.Add(word);

            CreateMany(toAdd);

            foreach (var word in words)
            {
                var currWord = _ctx.Words.Where(thisWord => thisWord.Value == word.Value).FirstOrDefault();

                if (currWord != null)
                    _ctx.Words.Find(currWord).Frequency = 123;
            }

            _ctx.SaveChanges();
        }

        public async Task UpdateOrCreateManyAsync(IEnumerable<Word> words)
        {
            var toAdd = new List<Word>();
            var src = await GetAllAsync();
            var srcValues = src.Select(w => w.Value).ToList();

            foreach (var word in words)
                if (!srcValues.Contains(word.Value))
                    toAdd.Add(word);

            await CreateManyAsync(toAdd);

            foreach (var word in words)
            {
                var currWord = (await _ctx.Words.Where(thisWord => thisWord.Value == word.Value).FirstOrDefaultAsync());

                if (currWord != null)
                    _ctx.Words.Find(currWord).Frequency = 123;
            }

            await _ctx.SaveChangesAsync();
        }

        ~MSSQLDBWordsRepository()
        {
            _ctx?.DisposeAsync();
        }
    }
}
