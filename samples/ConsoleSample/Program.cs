using System;
using EasyQuartz;
using Microsoft.Extensions.Hosting;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********** 开始运行 **********");

            new HostBuilder()
              .ConfigureServices(x => x.AddEasyQuartzService())
              .Build()
              .Run();

            Console.ReadKey();
        }
    }
}
