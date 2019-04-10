using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using VideoWriter.Struct;

namespace VideoWriter.Robots
{
    public class StateBot
    {
        public StateBot()
        {

        }

        public async Task Save(Post post)
        {
            string _directoryTempGhost = ConfigurationSettings.AppSettings["DIRECTORY_TEMP"];
            string _nameFile = "Content.json";

            if (!Directory.Exists(_directoryTempGhost))
                Directory.CreateDirectory(_directoryTempGhost);

            if (!File.Exists(_directoryTempGhost + _nameFile))
            {
                FileStream fileCreated = File.Create(_directoryTempGhost + _nameFile);
                fileCreated.Close();
            }

            using (StreamWriter file = File.CreateText(_directoryTempGhost + _nameFile))
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.Serialize(file, post);
            }
        }
        public async Task<Post> Load()
        {
            string _directoryTempGhost = ConfigurationSettings.AppSettings["DIRECTORY_TEMP"];
            string _nameFile = "Content.json";

            if (!Directory.Exists(_directoryTempGhost))
                Directory.CreateDirectory(_directoryTempGhost);

            if (!File.Exists(_directoryTempGhost + _nameFile))
            {
                FileStream fileCreated = File.Create(_directoryTempGhost + _nameFile);
                fileCreated.Close();
            }

            Post post = new Post();

            using (StreamReader r = new StreamReader(_directoryTempGhost + _nameFile))
            {
                string json = r.ReadToEnd();
                post = JsonConvert.DeserializeObject<Post>(json);
            }
            return post == null ? new Post() : post;
        }
    }
}
