﻿using System;
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
        public static void RegisterCommand(string commandName, Action<OS, string[]> handler, bool addAutocomplete = true, bool caseSensitive = false, bool retry = false)
        {
            if (retry)
                DebugMod.TotalCommands -= 1; // set back by one since we are retrying

            try
            {
                DebugMod.TotalCommands += 1;
                Command.CommandManager.RegisterCommand(commandName, handler, addAutocomplete, caseSensitive);
                DebugMod.CommandsLoaded += 1;
            }
            catch (Exception e)
            {
                // we dont want it to error if its already been registered
                Exception test1 = new ArgumentException($"Command {commandName} has already been registered!", nameof(commandName));

                // we also dont want it to error if its not implemented since this might just be addAutocomplete
                Exception test2 = new NotImplementedException();

                if (e.Message != test1.Message && e.Message != test2.Message)
                {
                    throw e;
                }
                else
                {
                    if (e.Message == test2.Message && !retry) // if it still errors on retry something else probably broke and we should send back a warn
                    {
                        RegisterCommand(commandName, handler, false, false, true); // Hopefully to fix
                    }
                    else if (e.Message == test1.Message && retry)
                    {
                        DebugMod.CommandsLoaded += 1;
                        return;
                    }
                    else
                    {
                        DebugLog(new string[] { e.Message, e.StackTrace }, "\n", "Warn", ConsoleColor.DarkYellow);
                    }
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
                //TODO: Rewrite DebugDaemon
                Pathfinder.Daemon.DaemonManager.RegisterDaemon<DebugDaemon>();
                INTERNALSETUP.RegisterCommand("loadDebugMenu", Commands.LoadDebugMenu, false); // Works
                INTERNALSETUP.RegisterCommand("openAllPorts", Commands.OpenAllPorts, true); // Works
                INTERNALSETUP.RegisterCommand("bypassProxy", Commands.BypassProxy, true); // Works
                INTERNALSETUP.RegisterCommand("solveFirewall", Commands.SolveFirewall, true); // Works
                INTERNALSETUP.RegisterCommand("getAdmin", Commands.GetAdmin, true); // Works
                INTERNALSETUP.RegisterCommand("loseAdmin", Commands.LoseAdmin, true); // Works

                if (DebugEnabled)
                {
                    INTERNALSETUP.RegisterCommand("startDeathSeq", Commands.DeathSeq, true); // Works
                    INTERNALSETUP.RegisterCommand("cancelDeathSeq", Commands.CancelDeathSeq, true); // Works
                    INTERNALSETUP.RegisterCommand("setHomeNodeServer", Commands.SetHomeNodeServer, true); // Works
                    INTERNALSETUP.RegisterCommand("setHomeAssetServer", Commands.SetHomeAssetServer, true); // Works
                    INTERNALSETUP.RegisterCommand("debug", Commands.Debug, true); // Works   
                    INTERNALSETUP.RegisterCommand("revealAll", Commands.RevealAll, true); // Works
                    INTERNALSETUP.RegisterCommand("addIRCMessage", Commands.AddIRCMessage, true); // Works
                    INTERNALSETUP.RegisterCommand("strikerAttack", Commands.StrikerAttack, true); // Works
                    INTERNALSETUP.RegisterCommand("themeAttack", Commands.ThemeAttack, true); // Works
                    INTERNALSETUP.RegisterCommand("callThePoliceSoTheyCanTraceYou", Commands.CallThePoliceSoTheyCanTraceYou, true); // Works
                    INTERNALSETUP.RegisterCommand("reportYourselfToFBI", Commands.ReportYourselfToFBI, true); // Works
                    INTERNALSETUP.RegisterCommand("traceYourselfIn", Commands.TraceYourselfIn, true); // Works
                    INTERNALSETUP.RegisterCommand("warningFlash", Commands.WarningFlash, true); // Works
                    INTERNALSETUP.RegisterCommand("stopTrace", Commands.StopTrace, true); // Works
                    INTERNALSETUP.RegisterCommand("hideDisplay", Commands.HideDisplay, true); // Works
                    INTERNALSETUP.RegisterCommand("hideNetMap", Commands.HideNetMap, true); // Works
                    INTERNALSETUP.RegisterCommand("hideTerminal", Commands.HideTerminal, true); // Works
                    INTERNALSETUP.RegisterCommand("hideRAM", Commands.HideRAM, true); // Works
                    INTERNALSETUP.RegisterCommand("showDisplay", Commands.ShowDisplay, true); // Works
                    INTERNALSETUP.RegisterCommand("showNetMap", Commands.ShowNetMap, true); // Works
                    INTERNALSETUP.RegisterCommand("showTerminal", Commands.ShowTerminal, true); // Unknown
                    INTERNALSETUP.RegisterCommand("showRAM", Commands.ShowRAM, true); // Works
                    INTERNALSETUP.RegisterCommand("getUniversalAdmin", Commands.GetUniversalAdmin, true); // Works
                    INTERNALSETUP.RegisterCommand("changeUserDetails", Commands.ChangeUserDetails, true); // Partial
                    //INTERNALSETUP.RegisterCommand("executeHack", Commands.ExecuteHack, true);
                    INTERNALSETUP.RegisterCommand("generateExampleAcademicRecord", Commands.GenerateExampleAcadmicRecord, true); // Works
                    INTERNALSETUP.RegisterCommand("generateExampleMedicalRecord", Commands.GenerateExampleMedicalRecord, true); // Fixed
                    INTERNALSETUP.RegisterCommand("changeMusic", Commands.ChangeMusic, true); // Fixed
                    INTERNALSETUP.RegisterCommand("crashComputer", Commands.CrashComputer, true); // Works
                    INTERNALSETUP.RegisterCommand("addProxy", Commands.AddProxy, true); // Works
                    INTERNALSETUP.RegisterCommand("addFirewall", Commands.AddFirewall, true); // Works
                    INTERNALSETUP.RegisterCommand("addUser", Commands.AddUser, true); // Works
                    INTERNALSETUP.RegisterCommand("openPort", Commands.OpenPort, true);
                    INTERNALSETUP.RegisterCommand("closeAllPorts", Commands.CloseAllPorts, true); // Works
                    INTERNALSETUP.RegisterCommand("closePort", Commands.ClosePort, true); // Fixed
                    INTERNALSETUP.RegisterCommand("removeProxy", Commands.RemoveProxy, true); // Works
                    INTERNALSETUP.RegisterCommand("playSFX", Commands.PlaySFX, true); // Works
                    INTERNALSETUP.RegisterCommand("deleteWhitelistDLL", Commands.DeleteWhitelistDLL, true); // Works
                    INTERNALSETUP.RegisterCommand("addComputer", Commands.AddComputer, true); // Works
                    INTERNALSETUP.RegisterCommand("getMoreRAM", Commands.GetMoreRAM, true); // Works
                    INTERNALSETUP.RegisterCommand("setFaction", Commands.SetFaction, true); // Works
                    INTERNALSETUP.RegisterCommand("tracedBehind250Proxies", Commands.TracedBehind250Proxies, true); // Works
                    INTERNALSETUP.RegisterCommand("oxygencraftStorageFacilityCache", Commands.OxygencraftStorageFaciltyCache, true); // Don't tell anyone about this command, keep it a secret: Note: Bugged
                    INTERNALSETUP.RegisterCommand("disableEmailIcon", Commands.DisableEmailIcon, true); // Works
                    INTERNALSETUP.RegisterCommand("enableEmailIcon", Commands.EnableEmailIcon, true); // Works
                    INTERNALSETUP.RegisterCommand("nodeRestore", Commands.NodeRestore, true); // Unknown
                    INTERNALSETUP.RegisterCommand("addWhiteCircle", Commands.AddRestoreCircle, true); // Works
                    INTERNALSETUP.RegisterCommand("whitelistBypass", Commands.WhitelistBypass, true); // Works
                    INTERNALSETUP.RegisterCommand("setTheme", Commands.SetTheme, true);  // Works
                    INTERNALSETUP.RegisterCommand("setCustomTheme", Commands.SetCustomTheme, true); // Works
                    INTERNALSETUP.RegisterCommand("linkComputer", Commands.LinkComputer, true); // Works
                    INTERNALSETUP.RegisterCommand("unlinkComputer", Commands.UnlinkComputer, true); // Works
                    INTERNALSETUP.RegisterCommand("loseAllNodes", Commands.LoseAllNodes, true); // Works
                    INTERNALSETUP.RegisterCommand("loseNode", Commands.LoseNode, true); // Works
                    INTERNALSETUP.RegisterCommand("revealNode", Commands.RevealNode, true); // Works
                    INTERNALSETUP.RegisterCommand("removeComputer", Commands.RemoveComputer, true); // Works
                    INTERNALSETUP.RegisterCommand("resetIP", Commands.ResetIP, true); // Works
                    INTERNALSETUP.RegisterCommand("resetPlayerCompIP", Commands.ResetPlayerCompIP, true); // Works
                    INTERNALSETUP.RegisterCommand("setIP", Commands.SetIP, true); // Works
                    INTERNALSETUP.RegisterCommand("showFlags", Commands.ShowFlags, true); // Works
                    INTERNALSETUP.RegisterCommand("addFlag", Commands.AddFlag, true); // Works
                    INTERNALSETUP.RegisterCommand("removeFlag", Commands.RemoveFlag, true); // Works
                    INTERNALSETUP.RegisterCommand("authenticateToIRC", Commands.AuthenticateToIRC, true); // Works
                    INTERNALSETUP.RegisterCommand("addAgentToIRC", Commands.AddAgentToIRC, true); // Works
                    INTERNALSETUP.RegisterCommand("setCompPorts", Commands.SetCompPorts, true); // Works
                    //INTERNALSETUP.RegisterCommand("removePortFromComp", Commands.RemovePortFromComp,  true); Replaced with setCompPorts
                    INTERNALSETUP.RegisterCommand("addSongChangerDaemon", Commands.AddSongChangerDaemon, true); // Works
                    INTERNALSETUP.RegisterCommand("addRicerConnectDaemon", Commands.AddRicerConnectDaemon, true); // Works
                    INTERNALSETUP.RegisterCommand("addDLCCreditsDaemon", Commands.AddDLCCreditsDaemon, true); // Works
                    //INTERNALSETUP.RegisterCommand("addIRCDaemon", Commands.AddIRCDaemon,  true);
                    INTERNALSETUP.RegisterCommand("addISPDaemon", Commands.AddISPDaemon, true); // Works
                    INTERNALSETUP.RegisterCommand("quit", Commands.Quit, true); // Works
                    INTERNALSETUP.RegisterCommand("deleteLogs", Commands.DeleteLogs, true); // Works
                    INTERNALSETUP.RegisterCommand("forkbombProof", Commands.ForkbombProof, true); // Works

                    INTERNALSETUP.RegisterCommand("changeCompIcon", Commands.ChangeCompIcon, true);
                    INTERNALSETUP.RegisterCommand("removeSongChangerDaemon", Commands.RemoveSongChangerDaemon, true);
                    INTERNALSETUP.RegisterCommand("removeRicerConnectDaemon", Commands.RemoveRicerConnectDaemon, true);
                    INTERNALSETUP.RegisterCommand("removeDLCCreditsDaemon", Commands.RemoveDLCCreditsDaemon, true);
                    //INTERNALSETUP.RegisterCommand("removeIRCDaemon", Commands.RemoveIRCDaemon,  true);
                    INTERNALSETUP.RegisterCommand("removeISPDaemon", Commands.RemoveISPDaemon, true);
                    INTERNALSETUP.RegisterCommand("forkbombVirus", Commands.ForkbombVirus, true);
                    INTERNALSETUP.RegisterCommand("installInviolabilty", Commands.InstallInviolabilty, true);
                    INTERNALSETUP.RegisterCommand("removeAllDaemons", Commands.RemoveAllDaemons, true);
                    INTERNALSETUP.RegisterCommand("showIPNamesAndID", Commands.ShowIPNamesAndID, true);
                    INTERNALSETUP.RegisterCommand("changeAdmin", Commands.ChangeAdmin, true);
                    INTERNALSETUP.RegisterCommand("viewAdmin", Commands.ViewAdmin, true);
                    INTERNALSETUP.RegisterCommand("tellPeopleYouAreGonnaHackThemOnline", Commands.TellPeopleYouAreGonnaHackThemOnline, true);
                    INTERNALSETUP.RegisterCommand("myFatherIsCCC", Commands.MyFatherIsCCC, true);
                    INTERNALSETUP.RegisterCommand("cantTouchThis", Commands.CantTouchThis, true);
                    INTERNALSETUP.RegisterCommand("replayPlaneMission", Commands.ReplayPlaneMission, true);
                    INTERNALSETUP.RegisterCommand("replayPlaneMissionSecondary", Commands.ReplayPlaneMissionSecondary, true);
                    INTERNALSETUP.RegisterCommand("viewFaction", Commands.ViewFaction, true);
                    INTERNALSETUP.RegisterCommand("viewPlayerVal", Commands.ViewPlayerVal, true);
                    INTERNALSETUP.RegisterCommand("kaguyaTrialEffect", Commands.KaguyaTrialEffect, true);
                    INTERNALSETUP.RegisterCommand("kaguyaTrialEffect2", Commands.KaguyaTrialEffect2, true);
                    INTERNALSETUP.RegisterCommand("kaguyaTrialEffect3", Commands.KaguyaTrialEffect3, true);
                    INTERNALSETUP.RegisterCommand("summonDebugModDaemonComp", Commands.SummonDebugModDaemonComp, true);
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
