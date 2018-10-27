﻿using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;

namespace SpecFlowLSP
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            /*while (!System.Diagnostics.Debugger.IsAttached)
            {
                await Task.Delay(100);
            }*/
            
            
            var server = new LanguageServer(
                Console.OpenStandardInput(),
                Console.OpenStandardOutput(),
                new LoggerFactory());
            var manager = new GherkinManager();

            server.OnInitialize(request =>
            {
                manager.HandleStartup(new Uri(HttpUtility.UrlDecode(request.RootUri.OriginalString)).LocalPath);
                return Task.CompletedTask;
            });

            server.AddHandler(new TextDocumentHandler(server, manager));
            await server.Initialize();
            await server.WaitForExit;
        }
    }
}