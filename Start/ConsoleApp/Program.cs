using ConsoleApp.Classes;
using ConsoleApp.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp
{
    internal class Program
    {
        private static IProgramMgr? _programMgr;

        private static void Main(string[] args)
        {
            #region Initialization
            // Configuration: Managed by Host.CreateDefaultBuilder
            var host = CreateHostBuilder(args).Build();
            #endregion

            _programMgr = host.Services.GetRequiredService<IProgramMgr>();
            _programMgr.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
        // Ref: https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.host.createdefaultbuilder?view=dotnet-plat-ext-7.0
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services
                    .AddSingleton<IProgramMgr, ProgramMgr>()
                    .AddSingleton<IConsoleMgr, ConsoleMgr>();
            });
    }
}