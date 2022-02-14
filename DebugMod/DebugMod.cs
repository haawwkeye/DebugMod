using System;
using Command = Pathfinder.Command;
using Hacknet;
using Pathfinder.Event;
using System.Net;
using BepInEx;
using BepInEx.Hacknet;

namespace DebugMod
{
    internal class INTERNALSETUP
    {
        public static void DebugLog(string[] args, string join = " ", string type = "Message", ConsoleColor color = ConsoleColor.White)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("");
                return;
            }
            else
            {
                ConsoleColor consoleColor = Console.ForegroundColor;

                Console.ForegroundColor = color;
                Console.WriteLine($"[{type}] " + string.Join(join, args));
                Console.ForegroundColor = consoleColor;
            }
        }

        // addAutocomplete is false until fixed
        public static void RegisterCommand(string commandName, Action<OS, string[]> handler, bool addAutocomplete = true, bool caseSensitive = false)
        {
            if (addAutocomplete == true) // since addAutocomplete doesnt work rn
                addAutocomplete = false;

            try
            {
                DebugMod.TotalCommands += 1;
                Command.CommandManager.RegisterCommand(commandName, handler, addAutocomplete, caseSensitive);
                DebugMod.CommandsLoaded += 1;
            }
            catch (Exception e)
            {
                // we dont want it to error if its already been registered
                Exception test = new ArgumentException($"Command {commandName} has already been registered!", nameof(commandName));
                if (e.Message != test.Message)
                {
                    throw e;
                }
                else
                {
                    DebugLog(new string[] { e.Message, e.StackTrace }, "\n", "Warn", ConsoleColor.DarkYellow);
                }
            }
        }
    }

    [BepInPlugin(ModGUID, ModName, ModVer)]
    public class DebugMod : HacknetPlugin
    {
        public const string ModGUID = "com.DebugMod";
        public const string ModName = "DebugMod";
        public const string ModVer = "1.0.0";
        private bool alreadyLoaded = false;
        private bool shouldLoad = true;
        public static int CommandsLoaded { get; internal set; } = 0;
        public static int TotalCommands { get; internal set; } = 0;

        public static string version = ModVer;
        public static string newVersion = GetVersion();
        public string GetIdentifier()
        {
            return "Debug Mod";
        }

        private static string GetVersion()
        {
            //WebClient client = new WebClient();
            //return client.DownloadString("https://raw.githubusercontent.com/oxygencraft/DebugMod/master/VersionFile.txt");
            return version;
        }

        public string Identifier
        {
            get
            {
                return GetIdentifier();
            }
        }

        public override bool Load()
        {
            if (alreadyLoaded)
            {
                return false;
            }

            alreadyLoaded = true;

            Console.WriteLine("Loading Debug Mod");

            // Load all content
            LoadContent();

            INTERNALSETUP.DebugLog(new string[] { $"{CommandsLoaded}/{TotalCommands} commands loaded" }, "\n", "Warn", ConsoleColor.DarkYellow);

            // Patch Assembly
            HarmonyInstance.PatchAll(typeof(DebugMod).Assembly);

            return shouldLoad;
        }

        private void LoadContent()
        {
            try
            {
                bool DebugEnabled = true;
                INTERNALSETUP.RegisterCommand("openAllPorts", Commands.OpenAllPorts, true); // Works
                INTERNALSETUP.RegisterCommand("bypassProxy", Commands.BypassProxy, true); // Works
                INTERNALSETUP.RegisterCommand("solveFirewall", Commands.SolveFirewall, true); // Works
                INTERNALSETUP.RegisterCommand("getAdmin", Commands.GetAdmin, true); // Works
                INTERNALSETUP.RegisterCommand("loseAdmin", Commands.LoseAdmin, true); // Works


                if (DebugEnabled)
                {
                    INTERNALSETUP.RegisterCommand("startDeathSeq", Commands.DeathSeq, false); // Works
                    INTERNALSETUP.RegisterCommand("cancelDeathSeq", Commands.CancelDeathSeq, false); // Works
                    INTERNALSETUP.RegisterCommand("setHomeNodeServer", Commands.SetHomeNodeServer, false); // Works
                    INTERNALSETUP.RegisterCommand("setHomeAssetServer", Commands.SetHomeAssetServer, false); // Works
                    INTERNALSETUP.RegisterCommand("debug", Commands.Debug, false); // Works   
                    INTERNALSETUP.RegisterCommand("revealAll", Commands.RevealAll, false); // Works
                    INTERNALSETUP.RegisterCommand("addIRCMessage", Commands.AddIRCMessage, false); // Works
                    INTERNALSETUP.RegisterCommand("strikerAttack", Commands.StrikerAttack, false); // Works
                    INTERNALSETUP.RegisterCommand("themeAttack", Commands.ThemeAttack, false); // Works
                    INTERNALSETUP.RegisterCommand("callThePoliceSoTheyCanTraceYou", Commands.CallThePoliceSoTheyCanTraceYou, false); // Works
                    INTERNALSETUP.RegisterCommand("reportYourselfToFBI", Commands.ReportYourselfToFBI, false); // Works
                    INTERNALSETUP.RegisterCommand("traceYourselfIn", Commands.TraceYourselfIn, false); // Works
                    INTERNALSETUP.RegisterCommand("warningFlash", Commands.WarningFlash, false); // Works
                    INTERNALSETUP.RegisterCommand("stopTrace", Commands.StopTrace, false); // Works
                    INTERNALSETUP.RegisterCommand("hideDisplay", Commands.HideDisplay, false); // Works
                    INTERNALSETUP.RegisterCommand("hideNetMap", Commands.HideNetMap, false); // Works
                    INTERNALSETUP.RegisterCommand("hideTerminal", Commands.HideTerminal, false); // Works
                    INTERNALSETUP.RegisterCommand("hideRAM", Commands.HideRAM, false); // Works
                    INTERNALSETUP.RegisterCommand("showDisplay", Commands.ShowDisplay, false); // Works
                    INTERNALSETUP.RegisterCommand("showNetMap", Commands.ShowNetMap, false); // Works
                    INTERNALSETUP.RegisterCommand("showTerminal", Commands.ShowTerminal, false); // Unknown
                    INTERNALSETUP.RegisterCommand("showRAM", Commands.ShowRAM, false); // Works
                    INTERNALSETUP.RegisterCommand("getUniversalAdmin", Commands.GetUniversalAdmin, false); // Works
                    INTERNALSETUP.RegisterCommand("changeUserDetails", Commands.ChangeUserDetails, false); // Partial
                    //INTERNALSETUP.RegisterCommand("executeHack", Commands.ExecuteHack, false);
                    INTERNALSETUP.RegisterCommand("generateExampleAcademicRecord", Commands.GenerateExampleAcadmicRecord, false); // Works
                    INTERNALSETUP.RegisterCommand("generateExampleMedicalRecord", Commands.GenerateExampleMedicalRecord, false); // Fixed
                    INTERNALSETUP.RegisterCommand("changeMusic", Commands.ChangeMusic, false); // Fixed
                    INTERNALSETUP.RegisterCommand("crashComputer", Commands.CrashComputer, false); // Works
                    INTERNALSETUP.RegisterCommand("addProxy", Commands.AddProxy, false); // Works
                    INTERNALSETUP.RegisterCommand("addFirewall", Commands.AddFirewall, false); // Works
                    INTERNALSETUP.RegisterCommand("addUser", Commands.AddUser, false); // Works
                    INTERNALSETUP.RegisterCommand("openPort", Commands.OpenPort, false);
                    INTERNALSETUP.RegisterCommand("closeAllPorts", Commands.CloseAllPorts, false); // Works
                    INTERNALSETUP.RegisterCommand("closePort", Commands.ClosePort, false); // Fixed
                    INTERNALSETUP.RegisterCommand("removeProxy", Commands.RemoveProxy, false); // Works
                    INTERNALSETUP.RegisterCommand("playSFX", Commands.PlaySFX, false); // Works
                    INTERNALSETUP.RegisterCommand("deleteWhitelistDLL", Commands.DeleteWhitelistDLL, false); // Works
                    INTERNALSETUP.RegisterCommand("addComputer", Commands.AddComputer, false); // Works
                    INTERNALSETUP.RegisterCommand("getMoreRAM", Commands.GetMoreRAM, false); // Works
                    INTERNALSETUP.RegisterCommand("setFaction", Commands.SetFaction, false); // Works
                    INTERNALSETUP.RegisterCommand("tracedBehind250Proxies", Commands.TracedBehind250Proxies, false); // Works
                    INTERNALSETUP.RegisterCommand("oxygencraftStorageFacilityCache", Commands.OxygencraftStorageFaciltyCache, false); // Don't tell anyone about this command, keep it a secret: Note: Bugged
                    INTERNALSETUP.RegisterCommand("disableEmailIcon", Commands.DisableEmailIcon, false); // Works
                    INTERNALSETUP.RegisterCommand("enableEmailIcon", Commands.EnableEmailIcon, false); // Works
                    INTERNALSETUP.RegisterCommand("nodeRestore", Commands.NodeRestore, false); // Unknown
                    INTERNALSETUP.RegisterCommand("addWhiteCircle", Commands.AddRestoreCircle, false); // Works
                    INTERNALSETUP.RegisterCommand("whitelistBypass", Commands.WhitelistBypass, false); // Works
                    INTERNALSETUP.RegisterCommand("setTheme", Commands.SetTheme, false);  // Works
                    INTERNALSETUP.RegisterCommand("setCustomTheme", Commands.SetCustomTheme, false); // Works
                    INTERNALSETUP.RegisterCommand("linkComputer", Commands.LinkComputer, false); // Works
                    INTERNALSETUP.RegisterCommand("unlinkComputer", Commands.UnlinkComputer, false); // Works
                    INTERNALSETUP.RegisterCommand("loseAllNodes", Commands.LoseAllNodes, false); // Works
                    INTERNALSETUP.RegisterCommand("loseNode", Commands.LoseNode, false); // Works
                    INTERNALSETUP.RegisterCommand("revealNode", Commands.RevealNode, false); // Works
                    INTERNALSETUP.RegisterCommand("removeComputer", Commands.RemoveComputer, false); // Works
                    INTERNALSETUP.RegisterCommand("resetIP", Commands.ResetIP, false); // Works
                    INTERNALSETUP.RegisterCommand("resetPlayerCompIP", Commands.ResetPlayerCompIP, false); // Works
                    INTERNALSETUP.RegisterCommand("setIP", Commands.SetIP, false); // Works
                    INTERNALSETUP.RegisterCommand("showFlags", Commands.ShowFlags, false); // Works
                    INTERNALSETUP.RegisterCommand("addFlag", Commands.AddFlag, false); // Works
                    INTERNALSETUP.RegisterCommand("removeFlag", Commands.RemoveFlag, false); // Works
                    INTERNALSETUP.RegisterCommand("authenticateToIRC", Commands.AuthenticateToIRC, false); // Works
                    INTERNALSETUP.RegisterCommand("addAgentToIRC", Commands.AddAgentToIRC, false); // Works
                    INTERNALSETUP.RegisterCommand("setCompPorts", Commands.SetCompPorts, false); // Works
                    //INTERNALSETUP.RegisterCommand("removePortFromComp", Commands.RemovePortFromComp,  false); Replaced with setCompPorts
                    INTERNALSETUP.RegisterCommand("addSongChangerDaemon", Commands.AddSongChangerDaemon, false); // Works
                    INTERNALSETUP.RegisterCommand("addRicerConnectDaemon", Commands.AddRicerConnectDaemon, false); // Works
                    INTERNALSETUP.RegisterCommand("addDLCCreditsDaemon", Commands.AddDLCCreditsDaemon, false); // Works
                    //INTERNALSETUP.RegisterCommand("addIRCDaemon", Commands.AddIRCDaemon,  false);
                    INTERNALSETUP.RegisterCommand("addISPDaemon", Commands.AddISPDaemon, false); // Works
                    INTERNALSETUP.RegisterCommand("quit", Commands.Quit, false); // Works
                    INTERNALSETUP.RegisterCommand("deleteLogs", Commands.DeleteLogs, false); // Works
                    INTERNALSETUP.RegisterCommand("forkbombProof", Commands.ForkbombProof, false); // Works

                    INTERNALSETUP.RegisterCommand("changeCompIcon", Commands.ChangeCompIcon, false);
                    INTERNALSETUP.RegisterCommand("removeSongChangerDaemon", Commands.RemoveSongChangerDaemon, false);
                    INTERNALSETUP.RegisterCommand("removeRicerConnectDaemon", Commands.RemoveRicerConnectDaemon, false);
                    INTERNALSETUP.RegisterCommand("removeDLCCreditsDaemon", Commands.RemoveDLCCreditsDaemon, false);
                    //INTERNALSETUP.RegisterCommand("removeIRCDaemon", Commands.RemoveIRCDaemon,  false);
                    INTERNALSETUP.RegisterCommand("removeISPDaemon", Commands.RemoveISPDaemon, false);
                    INTERNALSETUP.RegisterCommand("forkbombVirus", Commands.ForkbombVirus, false);
                    INTERNALSETUP.RegisterCommand("installInviolabilty", Commands.InstallInviolabilty, false);
                    INTERNALSETUP.RegisterCommand("removeAllDaemons", Commands.RemoveAllDaemons, false);
                    INTERNALSETUP.RegisterCommand("showIPNamesAndID", Commands.ShowIPNamesAndID, false);
                    INTERNALSETUP.RegisterCommand("changeAdmin", Commands.ChangeAdmin, false);
                    INTERNALSETUP.RegisterCommand("viewAdmin", Commands.ViewAdmin, false);
                    INTERNALSETUP.RegisterCommand("tellPeopleYouAreGonnaHackThemOnline", Commands.TellPeopleYouAreGonnaHackThemOnline, false);
                    INTERNALSETUP.RegisterCommand("myFatherIsCCC", Commands.MyFatherIsCCC, false);
                    INTERNALSETUP.RegisterCommand("cantTouchThis", Commands.CantTouchThis, false);
                    INTERNALSETUP.RegisterCommand("replayPlaneMission", Commands.ReplayPlaneMission, false);
                    INTERNALSETUP.RegisterCommand("replayPlaneMissionSecondary", Commands.ReplayPlaneMissionSecondary, false);
                    INTERNALSETUP.RegisterCommand("viewFaction", Commands.ViewFaction, false);
                    INTERNALSETUP.RegisterCommand("viewPlayerVal", Commands.ViewPlayerVal, false);
                    INTERNALSETUP.RegisterCommand("kaguyaTrialEffect", Commands.KaguyaTrialEffect, false);
                    INTERNALSETUP.RegisterCommand("kaguyaTrialEffect2", Commands.KaguyaTrialEffect2, false);
                    INTERNALSETUP.RegisterCommand("kaguyaTrialEffect3", Commands.KaguyaTrialEffect3, false);
                    INTERNALSETUP.RegisterCommand("summonDebugModDaemonComp", Commands.SummonDebugModDaemonComp, false);
                    //TODO: Rewrite DebugDaemon
                    //Pathfinder.Daemon.IInterface daemon = new DebugDaemon();
                    //Pathfinder.Daemon.Handler.RegisterDaemon("DebugModDaemon", daemon);
                    /*if (version != newVersion)
                    {
                        EventManager.RegisterListener<OSLoadSaveFileEvent>(NewUpdateAlert);
                    } */
                }
            }
            catch (Exception e)
            {
                if (shouldLoad)
                    shouldLoad = false;

                INTERNALSETUP.DebugLog(new string[] { "Failed to load Debug Mod", e.Message, e.StackTrace }, "\n", "Error", ConsoleColor.Red);
            }
        }

        //TODO: Remake NewUpdateAlert
        /*
        public void NewUpdateAlert(OSLoadSaveFileEvent obj)
        {
            OS os = obj.OS;
            double time = 6;
            Action action = (Action)(() =>
            {
                os.write("New update of Debug Mod is available");
                os.write("You are currently running: " + version + "New version: " + newVersion);
            });
            os.delayer.Post(ActionDelayer.Wait(time), action);
            obj.IsCancelled = true;
        }
        */

        public override bool Unload()
        {
            Console.WriteLine("Unloading Debug Mod");
            return true;
        }
    }
}
