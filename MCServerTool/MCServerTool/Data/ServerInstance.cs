using System;
using System.Text.Json.Serialization;

namespace MCServerTool.Data
{
    public class ServerInstance
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("name")]
        public string Name { get; set; } = "New Server";

        [JsonPropertyName("version")]
        public string Version { get; set; } = "1.20.4";

        [JsonPropertyName("core")]
        public string Core { get; set; } = "Vanilla";

        [JsonPropertyName("executable_name")]
        public string ExecutableName { get; set; } = "server.jar";

        [JsonPropertyName("working_directory")]
        public string WorkingDirectory { get; set; } = "";

        [JsonPropertyName("java_path")]
        public string JavaPath { get; set; } = "java";

        [JsonPropertyName("java_arguments")]
        public string JavaArguments { get; set; } = "-Xms1G -Xmx2G";

        [JsonPropertyName("eula_accepted")]
        public bool EulaAccepted { get; set; } = false;

        [JsonPropertyName("no_gui")]
        public bool NoGui { get; set; } = true;
    }
}
