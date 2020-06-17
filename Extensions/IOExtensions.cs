using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace App.Extensions
{
    public static class IOExtensions
    {
        static public string InputResourceFile(string path)
        {
            if (Regex.IsMatch(path, @"[A-z]:\/([A-z0-9-_+]+\/)*([A-z0-9]+\.(txt))$") && File.Exists(path))
                return path;
            else
            {
                Console.Write("Неправильный формат абсолютного пути или такого файла не существует" +
                    "\nПопробуйте ввести путь ещё раз: ");
                return InputResourceFile(Console.ReadLine());
            }
        }

        static public async Task<List<string>> GetWordsFromFileAsync(string resourceFile)
        {
            var result = new List<string>();
            var input = new StringBuilder();
            input.Append(File.ReadAllText(resourceFile));
            //using (FileStream fstream = new FileStream(resourceFile, FileMode.Open, FileAccess.Read))
            //using (StreamReader reader = new StreamReader(fstream))
            //    while (!reader.EndOfStream)
            //        input.Append(await reader.Read());

            Regex anyWord = new Regex(@"([^\s]+)");
            foreach (var word in anyWord.Matches(input.ToString()))
                result.Add((word as Match).Value);

            return result;
        }
    }
}
