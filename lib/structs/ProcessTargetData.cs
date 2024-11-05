using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _02_PingProcessInformationToWebsiteService.lib.structs
{
    internal struct ProcessTargetData
    {

        [JsonPropertyName("appName")]
        public string ApplicationName {  get; set; }

        [JsonPropertyName("isRunning")]
        public bool IsRunning {  get; set; }

    }
}
