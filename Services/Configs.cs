using ChatZorro.Entities;
using System.Text.Json;

namespace ChatZorro.Services
{
    public class Configs
    {
        Configuration config = null;

        protected void Load()
        {
            config = new Configuration();
            string path = System.IO.Directory.GetCurrentDirectory() + "\\Data";
            string fileName = System.IO.Path.Combine(path, "Config.ini");
            string json = System.IO.File.ReadAllText(fileName);
            var configData = JsonSerializer.Deserialize<Configuration>(json);
            this.config.UserCounter = configData.UserCounter;
            this.config.ChatCounter = configData.ChatCounter;
            this.config.MessageCounter = configData.MessageCounter;
            this.config.SqlConnectionString = configData.SqlConnectionString;
        }

        protected void Save()
        {
            string json = JsonSerializer.Serialize(config);
            string path = System.IO.Directory.GetCurrentDirectory() + "\\Data";
            string fileName = System.IO.Path.Combine(path, $"Config.ini");
            System.IO.File.WriteAllText(fileName, json);
        }

        internal int NewUserCode()
        {
            this.config.UserCounter++;
            Save();
            return this.config.UserCounter;
        }

        internal int NewChatCode()
        {
            this.config.ChatCounter++;
            Save();
            return this.config.ChatCounter;
        }

        internal int NewMessageCode()
        {
            this.config.MessageCounter++;
            Save();
            return this.config.MessageCounter;
        }

        internal string GetConnectionString()
        {
            return this.config.SqlConnectionString;
        }
    }
}
