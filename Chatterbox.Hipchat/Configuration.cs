using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Chatterbox.Hipchat
{
    public class Configuration
    {
        public string Email { get; set; }

        public void Save()
        {
            if (!File.Exists("Plugins/Chatterbox.Hipchat/config.json"))
            {
                File.CreateText("Plugins/Chatterbox.Hipchat/config.json").Close();
            }

            File.WriteAllText("Plugins/Chatterbox.Hipchat/config.json", JsonConvert.SerializeObject(this));
        }

        public static Configuration Load()
        {
            if (!File.Exists("Plugins/Chatterbox.Hipchat/config.json"))
            {
                File.CreateText("Plugins/Chatterbox.Hipchat/config.json").Close();
                File.WriteAllText("Plugins/Chatterbox.Hipchat/config.json", JsonConvert.SerializeObject(Default()));
            }

            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("Plugins/Chatterbox.Hipchat/config.json"));
        }


        public static Configuration Default()
        {
            if (!File.Exists("Plugins/Chatterbox.Hipchat/config.json"))
            {
                File.CreateText("Plugins/Chatterbox.Hipchat/config.json").Close();
            }

            return new Configuration
            {
                Email = ""
            };
        }
    }
}
