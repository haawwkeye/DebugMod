using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;

namespace DebugMod
{
    internal class Updater
    {
        public static readonly string path = Directory.GetCurrentDirectory();
        public static bool AutoupdateEnabled { get; set; } = true;
        public static bool IncludeBetaVersions { get; set; } = true;

        private  static WebClient wc = null; // getWC() stuff

        private static readonly Regex sWhitespace = new Regex(@"\s+");
        private static readonly string releasePage = "https://github.com/haawwkeye/DebugMod/releases";
        private static readonly string ModUpdaterDownloadLink = "https://github.com/haawwkeye/DebugMod/releases/download/ModUpdater-Latest/ModUpdater.zip";

        private static void killHacknet() // This is used to kill the Hacknet exe env
        {
            Process[] processes = Process.GetProcessesByName("Hacknet");

            foreach (Process process in processes)
            {
                try
                {
                    if (processes.Length == 1)
                    {
                        process.Kill(); // We need to kill hacknet to start update
                        process.Dispose();
                    }
                    else
                    {
                        INTERNALSETUP.DebugLog(new string[] { $"unable to find correct hacknet.exe {processes.Length} found\nUsing backup" }, "", "Updater", ConsoleColor.Yellow);
                        Environment.Exit(0);
                        break;
                    }
                }
                catch
                {
                    continue;
                }
            }
        }

        private static void Write(string action, string text, ConsoleColor actColor1 = ConsoleColor.Blue, ConsoleColor actColor2 = ConsoleColor.Blue, ConsoleColor txtColor = ConsoleColor.White, string start = "\n")
        {
            Console.Write(start);
            Console.ForegroundColor = actColor2;
            Console.Write("[");
            Console.ForegroundColor = actColor1;
            Console.Write(action);
            Console.ForegroundColor = actColor2;
            Console.Write("] ");
            Console.ForegroundColor = txtColor;
            Console.Write(text);

            Console.ForegroundColor = ConsoleColor.White; // set back to white
        }

        private static string getModUpdaterDir(string path, string name)
        {
            string full = (path + name);

            if (Directory.Exists(full))
            {
                Directory.Delete(full, true);
            }

            Directory.CreateDirectory(full);

            return full;
        }

        private static void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
        {
            Console.Write("\n");
            Write("+", "downloaded, extracting\n");

            string file = $"{DebugMod.ModName}ModUpdater.zip";
            string fullpath = getModUpdaterDir(path, $"{DebugMod.ModName}ModUpdater");

            ZipFile.ExtractToDirectory(file, fullpath);

            Write("-", "Deleted zip file");

            File.Delete($"{path}/{file}");

            Write("+", "Updater installed, launching");

            Process.Start($"{fullpath}/ModUpdater.exe {DebugMod.ModName} {path}/BepInEx/plugins"); // args for the exe

            if (e.Error != null)
            {
                Console.WriteLine(e.Error.ToString());
            }

            killHacknet();
        }

        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Write("\rDownloading {0} % complete...", e.ProgressPercentage);
        }

        private static void DownloadFileInBackground(Uri address, string v)
        {
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);

            wc.DownloadFileAsync(address, v + ".zip");
        }

        public static string ReplaceWhitespace(string input, string replacement)
        {
            return sWhitespace.Replace(input, replacement);
        }

        private static void OpenUrl(string url) // Lets make this private so we dont have to worry about something else calling this
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    INTERNALSETUP.DebugLog(new string[] { $"Failed to open {url} in default browser" }, "", "Updater", ConsoleColor.Red);
                }
            }
        }

        internal static string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        internal static string[] PraseVersion(string version)
        {
            return version.Split("."[1]); ;
        }

        internal static bool CheckVersions(string currentVer, string nextVer)
        {
            INTERNALSETUP.DebugLog(new string[] { "Checking for update" }, "", "Updater", ConsoleColor.Cyan);

            bool newUpdate = false;//(currentVer != nextVer); // This should work for now
            
            if (newUpdate)
            {
                INTERNALSETUP.DebugLog(new string[] { $"New update of Debug Mod is available\nYou are currently running: {currentVer}\nNew version: {nextVer}\nDownload at {releasePage}" }, "", "Updater", ConsoleColor.Cyan);
                
                if (AutoupdateEnabled)
                {
                    DownloadDebugMod();
                }
            }
            
            return newUpdate; 
        }

        private static void DownloadDebugMod() // dont need anything but check versions to call this
        {
            INTERNALSETUP.DebugLog(new string[] { "Attempting autoupdate" }, "", "Updater", ConsoleColor.Yellow);

            //TODO: Add a function to auto update the mod via Download
            bool downloadFailed = false;

            if (downloadFailed)
            {
                INTERNALSETUP.DebugLog(new string[] { $"autoupdate failed\nOpening browser to {releasePage}" }, "", "Updater", ConsoleColor.Red);
                OpenUrl(releasePage); // Download failed so open the website to the releases page
            }
            else
            {
                DownloadFileInBackground(new Uri(ModUpdaterDownloadLink), DebugMod.ModName + "ModUpdater"); // Let this download!
            }
        }

        internal static string GetVersion()
        {
            WebClient client = getWC();

            if (IncludeBetaVersions)
            {
                //Normal Release + dev builds
                return ReplaceWhitespace(client.DownloadString("https://raw.githubusercontent.com/haawwkeye/DebugMod/master/VersionFileBeta.txt"), "");
            }
            else
            {
                //Normal Release
                return ReplaceWhitespace(client.DownloadString("https://raw.githubusercontent.com/haawwkeye/DebugMod/master/VersionFile.txt"), "");
            }
        }

        private static WebClient getWC()
        {
            if (wc == null)
            {
                wc = new WebClient();
                // I have no idea what this does but no more errors! Thanks random people on the internet who asked the same question as me
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12
                       | SecurityProtocolType.Ssl3;
            }

            return wc;
        }
    }
}
