using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Chatterbox.Plugins
{
    internal class PluginLoader
    {
        public static EventHandler<PluginLoadedEventArgs> PluginLoad; 

        public static readonly List<ChatPlugin> Plugins = new List<ChatPlugin>();

        public static void LoadPlugins()
        {
            if (!Directory.Exists("Plugins"))
                Directory.CreateDirectory("Plugins");

            foreach (var file in Directory.EnumerateFiles("Plugins", "*.dll"))
            {
                try
                {
                    if (!LoadPlugin(file)) continue;
                    if (!Directory.Exists("Plugins/" + Path.GetFileNameWithoutExtension(file)))
                        Directory.CreateDirectory("Plugins/" + Path.GetFileNameWithoutExtension(file));
                }
                catch (Exception e)
                {
                }
                
            }
        }

        public static bool LoadPlugin(string location)
        {
            Assembly lib = Assembly.LoadFile(Path.GetFullPath(location));

            var instance = lib.GetTypes().Where(t => t.BaseType == typeof (ChatPlugin));
            var types = instance as Type[] ?? instance.ToArray();
            if (types.Length <= 0) return false;
            var plugin = Activator.CreateInstance(types[0]) as ChatPlugin;
            if (plugin == null) return false;

            if(PluginLoad != null)
                PluginLoad(null, new PluginLoadedEventArgs(plugin, lib));

            Plugins.Add(plugin);

            return true;
        }

        public class PluginLoadedEventArgs : EventArgs
        {
            public ChatPlugin ChatPlugin { get; set; }
            public Assembly Assembly { get; set; }

            public PluginLoadedEventArgs(ChatPlugin plugin, Assembly ass)
            {
                ChatPlugin = plugin;
                Assembly = ass;
            }
        }
    }
}
