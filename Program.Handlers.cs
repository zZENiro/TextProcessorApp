using App.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using static App.InputValidator;
using static App.Extensions.IOExtensions;

namespace App
{   
    static partial class Program
    {
        public static string GetConnectionString() => configuration["ConnString"];
        
        static async Task<string> ChooseCommandAsync(string command)
        {
            switch (command)
            {
                case "exit":
                    return "exit";
                case "createdict":
                    {
                        NextLine();
                        await wordDictionary.InsertAsync(
                            await GetWordsFromFileAsync(InputResourceFile(Console.ReadLine())));

                        await repository.CreateManyAsync(wordDictionary.Words);

                        break;
                    }
                case "updatedict":
                    {
                        NextLine();
                        await wordDictionary.UpdateOrInsertAsync(
                            await GetWordsFromFileAsync(InputResourceFile(Console.ReadLine())));

                        await repository.UpdateOrCreateManyAsync(wordDictionary.Words);

                        break;
                    }
                case "cleardict":
                    {
                        NextLine();
                        wordDictionary.Words.Clear();
                        await repository.DeleteAllAsync();

                        break;
                    }
                default:
                    {
                        NextLine();
                        Console.WriteLine($"Вы ввели: {command}");
                        foreach (var word in await wordDictionary.GetContinuesAsync(command))
                            Console.WriteLine($"- {word.Value}");
                        break;
                    }
            }
            return "";
        }

        static string ReadLineUTF()
        {
            var currentWord = new StringBuilder();
            ConsoleKeyInfo currentKey = new ConsoleKeyInfo();

            do
            {
                currentKey = Console.ReadKey();

                if (currentKey.Key.IsEscape())
                    return "exit";
                else if (currentKey.Key.IsBackspace())
                    currentWord.PopCharacter();
                else if (!currentKey.Key.IsEnter())
                    currentWord.Append(currentKey.KeyChar.ToString());

            } while (!currentKey.Key.IsEnter());

            if (!IsValid(currentWord.ToString()))
                return "exit";

            return currentWord.ToString();
        }

        static void OutputErrorInfo()
        {
            Console.WriteLine("Arguments: \n" +
                              "--port <PORT>\n" +
                              "--host <HOST>");
        }

        static void OutputInfo(string host, int port)
        {
            Console.WriteLine("Доступные команды: " +
                "\ncreatedict - создание словаря" +
                "\nupdatedict - обновление словаря" +
                "\ncleardict - очистить словарь" +
                "\nПеред тем как вводить слова, стоит загрузить словарь");

            Console.WriteLine($"Присоединение к http://{host}:{port}/");
        }

        static void NextLine() => Console.WriteLine();
    }
}
