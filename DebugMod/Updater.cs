using System;
using System.Net;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace DebugMod
{
    internal class Updater
    {
        public static bool AutoupdateEnabled { get; set; } = false;
        public static bool IncludeBetaVersions { get; set; } = true;

        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement)
        {
            return sWhitespace.Replace(input, replacement);
        }

        private static void OpenUrl(string url)
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
                    throw new Exception($"Failed to open {url} in default browser");
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

            bool newUpdate = (currentVer != nextVer); // This should work for now
            
            if (newUpdate)
            {
                INTERNALSETUP.DebugLog(new string[] { $"New update of Debug Mod is available\nYou are currently running: {currentVer}\nNew version: {nextVer}\nDownload at https://github.com/haawwkeye/DebugMod/releases" }, "", "Updater", ConsoleColor.Cyan);
            }
            
            return newUpdate; 
        }

        internal static void DownloadDebugMod()
        {
            //TODO: Add a function to auto update the mod via Download
            bool downloadFailed = false;

            if (downloadFailed)
            {
                OpenUrl("https://github.com/haawwkeye/DebugMod/releases"); // Download failed so open the website to the releases page
            }
        }

        internal static string GetVersion()
        {
            WebClient client = new WebClient();

            // I have no idea what this does but no more errors! Thanks random people on the internet who asked the same question as me
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

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
    }
}
