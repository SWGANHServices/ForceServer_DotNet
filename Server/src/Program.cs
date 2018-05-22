using Microsoft.Extensions.DependencyInjection;
using Server;
using Server.src.Contracts;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure;
using SwgAnh.Docker.Infrastructure.Factories;
using SwgAnh.Docker.Infrastructure.Logger;
using SwgAnh.Docker.Infrastructure.LoginServer;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgAnhServer;
using SwgAnh.Docker.src.Contracts;
using SwgAnh.Docker.src.Infrastructure.LoginServer;

namespace SwgAnh.Docker
{
    static class Program
    {
        /// <summary>
        /// Setartup for the server
        /// </summary>
        /// <param name="args">Not in use</param>
        private static void Main(string[] args)
        {
            var serviceCollection = ConfigureDependencyInjection();
            var swgServer = serviceCollection.GetService<ISwgServer>();
            swgServer.Run();
        }


        private static ServiceProvider ConfigureDependencyInjection()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<ILoginServer, LoginServerClient>()
                .AddTransient<ISwgServer, SwgServer>()
                .AddSingleton<ILogger, ConsoleLogger>()
                .AddTransient<ISessionRecivedHandler, SessionRecivedHandler>()
                .AddSingleton<IUdpClient, UdpClient>()
                .AddTransient<ISystemMessage, SystemMessage>()
                .AddTransient<ISoeActionFactory, SoeActionFactory>()
                .AddTransient<IChlDataRecived, ChlDataRecived>()
                .BuildServiceProvider();
            return serviceProvider;
        }
    }
}