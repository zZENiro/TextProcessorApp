using System;
using System.Text;
using System.Linq;
using App.Extensions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;
using System.Net.Sockets;
using System.Net.Http;
using System.Net;
using App.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration.Xml;

namespace App
{
    public static partial class Program
    {
        static IWordsRepositoryAsync repository;
        static WordDictionary wordDictionary;
        static StringBuilder line = new StringBuilder();
        static IConfiguration configuration;

        static string HOST;
        static int PORT;
        static string url;

        static async Task Main(string[] args)
        {
            configuration =
                new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddCommandLine(args)
                .Build();

            repository = new MSSQLDBWordsRepository(configuration);
            wordDictionary = new WordDictionary(repository);

            HOST = configuration["host"];
            PORT = int.Parse(configuration["port"]);
            url = $"http://{HOST}:{PORT}/";

            OutputInfo(HOST, PORT);

            HttpServer server = new HttpServer(url, repository);
            server.StartHandleIncomingConnectionsAsync();

            while (true)
            {
                line.Append(ReadLineUTF());

                if (await ChooseCommandAsync(line.ToString()) == "exit")
                    break;

                line.Clear();
            }
        }
    }
}
