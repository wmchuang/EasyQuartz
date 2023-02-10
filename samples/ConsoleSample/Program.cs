using System;
using EasyQuartz;
using EasyQuartzStore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********** Begin **********");

            new HostBuilder()
                .ConfigureServices(x =>
                    {
                        x.AddEasyQuartz(e =>
                        {
                            e.UseMySql(m => { m.ConnectionString = "server=XXXX;user=root;database='XXXX';port=3306;password=XXXXX;SslMode=None"; });
                            e.UseDashboard();
                        });
                        
                        // x.AddEasyQuartz();
                        x.AddLogging(l =>
                            l.AddConsole()
                        );
                        
                    }
                )
                .Build()
                .Run();

            Console.ReadKey();
        }
    }
}