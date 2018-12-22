﻿using Microsoft.Extensions.DependencyInjection;
using SwgAnh.Docker.Contracts;
using SwgAnh.Docker.Infrastructure.Factories;
using SwgAnh.Docker.Infrastructure.Logger;
using SwgAnh.Docker.Infrastructure.LoginServer;
using SwgAnh.Docker.Infrastructure.Packets;
using SwgAnh.Docker.Infrastructure.SwgAnhServer;

namespace SwgAnh.Docker
{
    internal static class Program
    {
        /// <summary>
        ///     Setartup for the server
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
                .AddTransient<ISessionReceivedHandler, SessionReceivedHandler>()
                .AddSingleton<IUdpClient, UdpClient>()
                .AddTransient<ISystemMessage, SystemMessage>()
                .AddTransient<ISoeActionFactory, SoeActionFactory>()
                .AddTransient<IChlDataRecived, ChlDataReceived>()
                .AddTransient<INetStatusRequestRecived, NetStatusRequestReceived>()
                .BuildServiceProvider();
            return serviceProvider;
        }
    }
}