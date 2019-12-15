﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Net;
namespace FlashPayWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any,Setting.Ins.Port);
                })
                .Build();
    }
}
