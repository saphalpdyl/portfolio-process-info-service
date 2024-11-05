using System.Text.Json.Serialization;

namespace _02_PingProcessInformationToWebsiteService.lib
{
    public struct Configuration
    {
        [JsonPropertyName("target_url")]
        public string TargetURL { get; set; }

        [JsonPropertyName("authorization_secret")]
        public string AuthorizationSecret {  get; set; }

        [JsonPropertyName("targets")]
        public ProcessTarget[] ProcessTargets { get; set; }
    }; 
}