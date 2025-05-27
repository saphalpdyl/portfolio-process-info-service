using _02_PingProcessInformationToWebsiteService.lib;
using _02_PingProcessInformationToWebsiteService.lib.services;
using _02_PingProcessInformationToWebsiteService.lib.structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Text.Json;
using System.Timers;

namespace _02_PingProcessInformationToWebsiteService
{
    public partial class BaseService : ServiceBase
    {
        Timer Timer = new Timer();

        private const string SERVICE_FOLDER = "saphalpdyl_portfolio";
        private const string DEFAULT_TARGET_URL = "https://saphalpdyl.com";
        private const int DEFAULT_INTERVAL = 1000;

        private Configuration Configuration;
        private ProcessMonitor Monitor;
        private HttpClient HttpClient;
        private WebhookServer server;
        
        public BaseService()
        {
            InitializeComponent();
        }

        public void TestForDevelopment(string[] args)
        {
            OnStart(args);
            Console.ReadLine();
            OnStop();
        }

        private void InitializeServiceFoldersAndLogger()
        {
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string serviceFolderPath = Path.Combine(programFilesPath, SERVICE_FOLDER);

            string configFolderPath = Path.Combine(serviceFolderPath, "config");
            string errorFolderPath = Path.Combine(serviceFolderPath, "errors");

            if (!Directory.Exists(serviceFolderPath))
                Directory.CreateDirectory(serviceFolderPath);

            if (!Directory.Exists(configFolderPath))
                Directory.CreateDirectory(configFolderPath);

            if (!Directory.Exists(errorFolderPath))
                Directory.CreateDirectory(errorFolderPath);

            ErrorLogger.SetupLogger(errorFolderPath);
        }

        private void InitializeConfiguration()
        {
            try
            {
                string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                string serviceFolderPath = Path.Combine(programFilesPath, SERVICE_FOLDER);
                string configFolderPath = Path.Combine(serviceFolderPath, "config");

                string configFilePath = Path.Combine(configFolderPath, "service.config.json");
                if ( !File.Exists(configFilePath) )
                {
                    var defaultConfiguration = new Configuration
                    {
                        ProcessTargets = ProcessTarget.DEFAULT_TARGETS,
                        TargetURL = DEFAULT_TARGET_URL,
                        AuthorizationSecret = "",
                        TickInterval = DEFAULT_INTERVAL,
                    };

                    this.Configuration = defaultConfiguration;

                    string configurationJsonString = JsonSerializer.Serialize(defaultConfiguration, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    });

                    File.WriteAllText(configFilePath, configurationJsonString);
                    Console.WriteLine("Created a new configuration");

                    throw new Exception("[Config.first] Service shutdown because configuration is missing from being created just now.");
                } else
                {
                    string configurationFileValue = File.ReadAllText(configFilePath);
                    this.Configuration = JsonSerializer.Deserialize<Configuration>(configurationFileValue);

                    if (this.Configuration.AuthorizationSecret == "")
                    {
                        throw new Exception("[Config.Missing] Authorization secret is missing");
                    }
                }
            } catch (Exception ex)
            {
                ErrorLogger.GetInstance().LogEvent($"{ex.Message}\n{ex.StackTrace}");
                this.Stop();
            }
        }

        protected override void OnStart(string[] args)
        {
            InitializeServiceFoldersAndLogger();
            InitializeConfiguration();

            Monitor = new ProcessMonitor(this.Configuration.ProcessTargets);

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(this.Configuration.TargetURL),
            };
            server = new WebhookServer(HttpClient, this.Configuration.TargetURL, "/api/refreshProcessInformation");

            Timer.Elapsed += new ElapsedEventHandler(OnTick);
            Timer.Interval += this.Configuration.TickInterval;
            Timer.Enabled = true;
        }

        private void OnTick(object source, ElapsedEventArgs e)
        {
            try
            {
                // Handle the process information getting logic
                var processInformation = Monitor.GetProcessInformation();
                _ = server.SendProcessInformationToServer(processInformation, this.Configuration.AuthorizationSecret);
            }
            catch
            {
                // ignored
            }
        }

        protected override void OnStop()
        {
            
        }
    }
}
