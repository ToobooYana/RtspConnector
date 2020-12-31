﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RtspConnector.Client
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var root = new RtspGlobal();

            configuration.GetSection("RtspGlobal").Bind(root);

            root.Cams.ForEach(c =>
            {
                CopyFromRootIfNotSet(c, root);
            });

            // für jede cam einen Connector
            var connectors = new List<Connector>();

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            


            var tasks = new List<Func<Task>>();
            foreach (var cam in root.Cams)
            {
                var connector = new Connector();

                tasks.Add(() => connector.RunAsync(cam, token));
            }

            await Task.WhenAll(tasks.AsParallel().Select(async task => await task()));

            System.Console.WriteLine("Gestartet.");
            System.Console.WriteLine("Taste drücken zum Beenden");
            System.Console.ReadKey();

            source.Cancel();
        }

        private static void CopyFromRootIfNotSet(RtspConfiguration c, RtspGlobal root)
        {
            if (string.IsNullOrEmpty(c.BaseFolder))
                c.BaseFolder = root.BaseFolder;
            if (string.IsNullOrEmpty(c.User))
                c.User = root.User;
            if (string.IsNullOrEmpty(c.Password))
                c.Password = root.Password;
            if (string.IsNullOrEmpty(c.RtspAddress))
                c.RtspAddress = root.RtspAddress;
            if (string.IsNullOrEmpty(c.RtspPort))
                c.RtspPort = root.RtspPort;

            if (c.DelayAfterRecording == 0)
                c.DelayAfterRecording = root.DelayAfterRecording;
            if (c.Duration == 0)
                c.Duration = root.Duration;
            if (c.RtspIntervall == 0)
                c.RtspIntervall = root.RtspIntervall;
        }
    }
}
