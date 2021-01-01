using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;
using AngleSharp;
using AngleSharp.Common;
using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LazyCrawl
{
    class Program
    {

        public static async Task Main(string[] args)
        {
            /**** This is a trace message  ***/
            /*
            // Write a trace message to all configured trace listeners
            Trace.WriteLine(DateTime.Now.ToString() + " - Start of Main");

            // Define a trace listener to direct trace output from this method
            // to the console
            ConsoleTraceListener consoleTracer;

            if ((args.Length > 0) && (args[0].ToString().ToLower().Equals("/stderr")))
            // Initialize the console trace listener to write
            // trace output to the standard error stream.
            {
                consoleTracer = new ConsoleTraceListener(true);
            }
            else
            {
                // Initialize the console trace listener to write
                // trace output to the standard output stream.
                consoleTracer = new ConsoleTraceListener();
            }
            // Set the name of the trace listener, which helps identify this
            // particular instance within the trace listener collection.
            consoleTracer.Name = "mainConsoleTracer";

            */


            /******* The original code starts from here *****/
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();




            Log.Logger.Information("Crawler starting up!");

            await DemoSimpleCrawler();
            await DemoSinglePageRequest();

            //consoleTracer.Close();


        }

        private static async Task DemoSimpleCrawler()
        {
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = 100,    //Only crawl 100 pages
                MinCrawlDelayPerDomainMilliSeconds = 3000,   // Wait this many milliseconds between requests
                LoginUser = "ashpat86@gmail.com",       // 
                LoginPassword = "udemyhash#1",
                IsUriRecrawlingEnabled = true,
                UseDefaultCredentials = true

            };

            var crawler = new PoliteWebCrawler(config);

            crawler.PageCrawlCompleted += PageCrawlCompleted;   // Several events available

            var crawlResult = await crawler.CrawlAsync(new Uri("https://www.udemy.com/"));






        }

        private static async Task DemoSinglePageRequest()
        {

            var pageRequester = new PageRequester(new CrawlConfiguration(), new WebContentExtractor());

            var crawledPage = await pageRequester.MakeRequestAsync(new Uri("http://google.com"));
            Log.Logger.Information("{result}", new
            {
                url = Convert.ToString(crawledPage.Uri),
                status = Convert.ToInt32(crawledPage.HttpResponseMessage.StatusCode)
            });





        }

        private static void PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            var httpStatus = e.CrawledPage.HttpResponseMessage.StatusCode;
            var rawPageText = e.CrawledPage.Content.Text;
        }

        private static string LogWrite(string logMessage, StreamWriter w)
        {
            try
            {
                TextWriter writeFile = w;
                writeFile = w;
                writeFile.Write(logMessage);
                writeFile.Flush();
                writeFile.Close();
                writeFile = null;
            }
            catch (IOException e)
            {
                e.ToString();
            }

            return "";
        }

    }
}
