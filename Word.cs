using MongoDB.Bson.Serialization.Attributes;
using System;

namespace App
{
    public class Word
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Value { get; set; }

        public int Frequency { get; set; }

        public Word()
        { }

        public Word(string value, int freq) => (Value, Frequency) = (value, freq);
    }
}