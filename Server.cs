using App.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App
{
    class HttpServer
    {
        WordDictionary _wordDictionary;

        public static HttpListener listener;
        public static int pageViews = 0;
        public static int requestCount = 0;

        public HttpServer(string url, IWordsRepository repository)
        {
            _wordDictionary = new WordDictionary(repository);

            listener = new HttpListener();
            listener.Prefixes.Add(url);
            Console.WriteLine($"Listening for connections on {url}");
        }

        public async Task StartHandleIncomingConnectionsAsync()
        {
            listener.Start();

            bool runServer = true;

            while (runServer)
            {
                HttpListenerContext ctx = await listener.GetContextAsync();

                await Task.Factory.StartNew(async () =>
                {
                    HttpListenerRequest req = ctx.Request;
                    HttpListenerResponse resp = ctx.Response;

                    if (req.HttpMethod == "GET" && req.Headers["queryword"] != null)
                    {
                        string query = GetQueryWord(req.Headers["queryword"]);
                        byte[] data = GetResponse(await _wordDictionary.GetContinuesAsync(query));

                        resp.ContentType = "text/html";
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = data.LongLength;

                        await resp.OutputStream.WriteAsync(data, 0, data.Length);
                        resp.Close();
                    }
                });
            }

        }

        private byte[] GetResponse(List<Word> words)
        {
            var resp = new StringBuilder();
            foreach (var word in words)
                resp.Append($"- {word.Value}\n");
            return Encoding.UTF8.GetBytes(resp.ToString());
        }

        private string GetQueryWord(string word)
        {
            var req = word.Split(' ').ToList<string>();
            List<byte> queryWord = new List<byte>();

            foreach(var b in req)
                queryWord.Add(byte.Parse(b));

            return Encoding.UTF8.GetString(queryWord.ToArray());
        }
    }
}