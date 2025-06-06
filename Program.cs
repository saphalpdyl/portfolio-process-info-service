﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace _02_PingProcessInformationToWebsiteService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if ( Environment.UserInteractive )
            {
                BaseService service = new BaseService();
                service.TestForDevelopment(args);

            } else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new BaseService()
                };
                ServiceBase.Run(ServicesToRun);

            }
        }
    }
}
