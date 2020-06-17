using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

using static App.Extensions.WordDictionaryExtensions;

namespace App.Data
{
    public class MongoDBWordsRepository : IWordsRepositoryAsync
    {
        IMongoDatabase _db;

        IMongoCollection<Word> _collection;

        public MongoDBWordsRepository()
        {
            _db = new MongoClient().GetDatabase("words_db");
            _collection = _db.GetCollection<Word>("words_collection");
        }

        public async Task<IEnumerable<Word>> GetAllAsync()
        {
            var result = new List<Word>();

            foreach (var doc in await (await _collection.FindAsync(new BsonDocument())).ToListAsync())
                result.Add(doc);

            return result;
        }

        public async Task UpdateOrCreateManyAsync(IEnumerable<Word> words)
        {
            var toadd = new List<Word>();
            var src = await GetAllAsync();
            var srcValues = src.Select(w => w.Value).ToList();
            
            foreach (var word in words)
                if (!srcValues.Contains(word.Value))
                    toadd.Add(word);

            await CreateManyAsync(toadd);            

            foreach (var word in words)
                await _collection.ReplaceOneAsync(
                    item => item.Value == word.Value,
                    word);
        }

        public async Task CreateManyAsync(IEnumerable<Word> words) =>
            await _collection.InsertManyAsync(words);

        public async Task DeleteAllAsync() =>
            await _collection.DeleteManyAsync(new BsonDocument());

        // non-async

        public IEnumerable<Word> GetAll() => _collection.Find<Word>(new BsonDocument()).ToList();

        public void UpdateOrCreateMany(IEnumerable<Word> words)
        {
            var toadd = new List<Word>();
            var src = GetAll();
            var srcValues = src.Select(w => w.Value).ToList();

            foreach (var word in words)
                if (!srcValues.Contains(word.Value))
                    toadd.Add(word);

            CreateMany(toadd);

            foreach (var word in words)
                _collection.ReplaceOne(
                    item => item.Value == word.Value,
                    word);
        }

        public void CreateMany(IEnumerable<Word> words) => _collection.InsertMany(words);

        public void DeleteAll() => _collection.DeleteMany(new BsonDocument());
    }
}
