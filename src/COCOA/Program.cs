using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace COCOA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WikiSearch search = new WikiSearch("Artificial");
            //WebScraper scraper = new WebScraper("tollefj");
            //scraper.getNextLecture();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            host.Run();

        }
    }
}
