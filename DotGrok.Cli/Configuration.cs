namespace DotGrok.Cli
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class Configuration
    {
        private static readonly string ConfigFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".grokrc");

        public static ConfigurationData Grok { get; private set; }

        public static void Save()
        {
            var json = JsonConvert.SerializeObject(Grok, Formatting.Indented);
            File.WriteAllText(ConfigFile, json);
        }

        public static void Load()
        {
            if(File.Exists(ConfigFile))
            {
                var json = File.ReadAllText(ConfigFile);
                Grok = JsonConvert.DeserializeObject<ConfigurationData>(json);
            }
            else
            {
                Grok = new ConfigurationData();
            }
        }
    }

    public class ConfigurationData
    {
        public Dictionary<string, GrokPattern> Patterns { get; set; } = new Dictionary<string, GrokPattern>();
    }
}
