using _02_PingProcessInformationToWebsiteService.lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace _02_PingProcessInformationToWebsiteService
{
    public partial class BaseService : ServiceBase
    {
        private const string SERVICE_FOLDER = "saphalpdyl_portfolio";
        private const string DEFAULT_TARGET_URL = "https://saphalpdyl.com/";
        private Configuration Configuration;
        
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

        private async void InitializeConfiguration()
        {
            try
            {
                string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string serviceFolderPath = Path.Combine(programFilesPath, SERVICE_FOLDER);
                string configFolderPath = Path.Combine(serviceFolderPath, "config");

                if ( !Directory.Exists(serviceFolderPath) )
                    Directory.CreateDirectory(serviceFolderPath);

                if ( !Directory.Exists(configFolderPath) )
                    Directory.CreateDirectory(configFolderPath);

                string configFilePath = Path.Combine(configFolderPath, "service.config.json");
                if ( !File.Exists(configFilePath) )
                {
                    var defaultConfiguration = new Configuration
                    {
                        ProcessTargets = ProcessTarget.DEFAULT_TARGETS,
                        TargetURL = DEFAULT_TARGET_URL,
                        AuthorizationSecret = "",
                    };

                    this.Configuration = defaultConfiguration;

                    string configurationJsonString = JsonSerializer.Serialize(defaultConfiguration, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    });

                    File.WriteAllText(configFilePath, configurationJsonString);
                    Console.WriteLine("Created a new configuration");
                } else
                {
                    string configurationFileValue = File.ReadAllText(configFilePath);
                    this.Configuration = JsonSerializer.Deserialize<Configuration>(configurationFileValue);
                    Console.WriteLine(configurationFileValue);
                }
            } catch (Exception ex)
            {

            }
        }

        protected override void OnStart(string[] args)
        {
            InitializeConfiguration();
        }

        protected override void OnStop()
        {
        }
    }
}
