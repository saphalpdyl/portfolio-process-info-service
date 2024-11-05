using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _02_PingProcessInformationToWebsiteService.lib
{

    public class ProcessTarget
    {
        public static readonly ProcessTarget[] DEFAULT_TARGETS = {
            new ProcessTarget { ApplicationName = "Android Studio", ProcessName = "studio64" },
            new ProcessTarget { ApplicationName = "Visual Studio Code", ProcessName = "Code" },
            new ProcessTarget { ApplicationName = "Visual Studio IDE", ProcessName = "devenv" },
            new ProcessTarget { ApplicationName = "Postman", ProcessName = "Postman" },
            new ProcessTarget { ApplicationName = "Obsidian", ProcessName = "obsidian" },
            new ProcessTarget { ApplicationName = "Chrome", ProcessName = "chrome" }
        };

        [JsonPropertyName("app_name")]
        public string ApplicationName {  get; set; }

        [JsonPropertyName("process_name")]
        public string ProcessName {  get; set; }
    }
}
