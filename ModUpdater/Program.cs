using System;
using System.IO;

namespace ModUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: (PluginName) (ModDllPath)\nExample: ModUpdater.exe TestMod C:/temp/"); // Example stuff
                Console.Read();
                return;
            }

            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(i.ToString() + " " + args[i]); // debug info
            }

            string current = Directory.GetCurrentDirectory(); // Since the exe should be in the hacknet Directory when running
            string pluginsFolder = current + "/BepInEx/plugins"; // The path to the plugins folder

            string pluginName = args[0]; // Used to get the plugin we want to update
            string newModPath = args[1]; // Used to find the downloaded dll file so we can replace the plugin

            string modDll = pluginsFolder + "/" + args[0]; // Get the current plugin path

            // This is legit only for errors
            bool doesHacknetExist = File.Exists(current + "/Hacknet.exe"); // Does Hacknet exist
            bool doesPluginFolderExist = Directory.Exists(pluginsFolder); // Does the plugin folder exist
            bool doesNewModPathExist = Directory.Exists(newModPath); // Does the new mod path exist

            bool doesNewModDllExist = File.Exists(newModPath + $"/{pluginName}.dll"); // Does the new mod work
            bool doesModDllExist = File.Exists(modDll); // This shouldn't matter since we have the new dll file anyways

            if (doesHacknetExist && doesPluginFolderExist && doesNewModDllExist) // Make sure everything is here before starting
            {
                //TODO: Replace the current dll with the new dll given
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;

                if (!doesHacknetExist)
                    Console.WriteLine($"[ERROR] Hacknet.exe not found in {current}");

                if (!doesPluginFolderExist)
                    Console.WriteLine($"[ERROR] BepInEx/plugins folder not found");

                if (!doesNewModDllExist && doesNewModPathExist)
                    Console.WriteLine($"[ERROR] {newModPath}/{pluginName}.dll not found");
                else
                    Console.WriteLine("[ERROR] " + newModPath + " is not a vaild path");

                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.Read();
        }
    }
}
