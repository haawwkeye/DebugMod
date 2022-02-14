using Hacknet;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA = Microsoft.Xna.Framework;
using System.Reflection;
using Microsoft.Xna.Framework;
using Pathfinder;
using Pathfinder.Port;

namespace DebugMod
{
    static class Commands
    {

        public static void OpenAllPorts(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            foreach (var port in computer.ports)
            {
                if (!computer.isPortOpen(port))
                    computer.openPort(port, os.thisComputer.ip);
            }
        }
        public static void CloseAllPorts(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            foreach (var port in computer.ports)
            {
                if (computer.isPortOpen(port))
                    computer.closePort(port, os.thisComputer.ip);
            }
        }
        public static void BypassProxy(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            computer.proxyActive = false;
        }

        public static void SolveFirewall(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            computer.firewall.solved = true;
        }

        public static void GetAdmin(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            computer.adminIP = os.thisComputer.ip;
        }

        public static void DeathSeq(OS os, string[] args)
        {
            os.TraceDangerSequence.BeginTraceDangerSequence();
        }
        public static void CancelDeathSeq(OS os, string[] args)
        {
            os.TraceDangerSequence.CancelTraceDangerSequence();
        }

        public static void SetHomeNodeServer(OS os, string[] args)
        {
            os.homeNodeID = os.connectedComp.idName;
        }
        public static void SetHomeAssetServer(OS os, string[] args)
        {
            os.homeAssetServerID = os.connectedComp.idName;
        }
        public static void Debug(OS os, string[] args)
        {
            int num = PortExploits.services.Count;
            var binFiles = os.thisComputer.files.root.folders[2].files;
            for (int index = 0; index < PortExploits.services.Count && index < num; ++index)
            {
                var file = new FileEntry(PortExploits.crackExeData[PortExploits.portNums[index]], PortExploits.cracks[PortExploits.portNums[index]]);
                bool hasFile = binFiles.Contains(file);

                os.write(hasFile.ToString() + " " + file.name);

                if (hasFile == false)
                {
                    binFiles.Add(file);
                }
                continue;
            }
            /*
            binFiles.Add(new FileEntry(PortExploits.crackExeData[9], PortExploits.cracks[9]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[10], PortExploits.cracks[10]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[11], PortExploits.cracks[11]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[12], PortExploits.cracks[12]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[13], PortExploits.cracks[13]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[14], PortExploits.cracks[14]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[15], PortExploits.cracks[15]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[16], PortExploits.cracks[16]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[17], PortExploits.cracks[17]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[31], PortExploits.cracks[31]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[33], PortExploits.cracks[33]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[34], PortExploits.cracks[34]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[35], PortExploits.cracks[35]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[36], PortExploits.cracks[36]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[37], PortExploits.cracks[37]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[38], PortExploits.cracks[38]));
            binFiles.Add(new FileEntry(PortExploits.crackExeData[39], PortExploits.cracks[39]));
            */
            var pacemaker = new FileEntry(PortExploits.DangerousPacemakerFirmware, "KBT_TestFirmware.dll");
            var hasPacemaker = binFiles.Contains(pacemaker);

            os.write(hasPacemaker.ToString() + " " + pacemaker.name);

            if (hasPacemaker == false)
            {
                binFiles.Add(pacemaker);
            }
        }
        public static void LoseAdmin(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            computer.adminIP = os.connectedComp.ip;
        }
        public static void RevealAll(OS os, string[] args)
        {
            for (int index = 0; index < os.netMap.nodes.Count; ++index)
                os.netMap.visibleNodes.Add(index);
        }
        public static void AddIRCMessage(OS os, string[] args)
        {
            string computer = args[1];
            string author = args[2];
            string message = args[3];
            if (args.Length < 3)
            {
                os.write("Usage: addIRCMessage (ComputerID) (Author) (Message)");
                return;
            }

            DLCHubServer IRC = Programs.getComputer(os, computer).getDaemon(typeof(DLCHubServer)) as DLCHubServer;
            IRC.IRCSystem.AddLog(author, message, null);
        }
        public static void StrikerAttack(OS os, string[] args)
        {
            HackerScriptExecuter.runScript("DLC/ActionScripts/Hackers/SystemHack.txt", (object)os, (string)null);
        }
        public static void ThemeAttack(OS os, string[] args)
        {
            HackerScriptExecuter.runScript("HackerScripts/ThemeHack.txt", (object)os, (string)null);
        }
        public static void CallThePoliceSoTheyCanTraceYou(OS os, string[] args)
        {
            os.traceTracker.start(100f);
        }
        public static void ReportYourselfToFBI(OS os, string[] args)
        {
            os.traceTracker.start(20f);
        }
        public static void TraceYourselfIn(OS os, string[] args)
        {
            string TraceTimeInput = args[1];
            try
            {
                int IsNumber = Convert.ToInt32(args[1]);
            }
            catch
            {
                os.write("Usage: TraceYourselfIn: (TimeInSeconds)");
                return;
            }
            float.TryParse(TraceTimeInput, out float TraceTime);
            os.traceTracker.start(TraceTime);
        }
        public static void WarningFlash(OS os, string[] args)
        {
            SoundEffect sound = os.content.Load<SoundEffect>("SFX/beep");
            os.warningFlash();
            sound.Play();
        }
        public static void StopTrace(OS os, string[] args)
        {
            os.traceTracker.stop();
        }
        public static void HideDisplay(OS os, string[] args)
        {
            os.display.visible = false;
        }
        public static void HideNetMap(OS os, string[] args)
        {
            os.netMap.visible = false;
        }
        public static void HideTerminal(OS os, string[] args)
        {
            os.terminal.visible = false;
        }
        public static void HideRAM(OS os, string[] args)
        {
            os.ram.visible = false;
        }
        public static void ShowDisplay(OS os, string[] args)
        {
            os.display.visible = true;
        }
        public static void ShowNetMap(OS os, string[] args)
        {
            os.netMap.visible = true;
        }
        public static void ShowTerminal(OS os, string[] args)
        {
            os.terminal.visible = true;
        }
        public static void ShowRAM(OS os, string[] args)
        {
            os.ram.visible = true;
        }
        public static void GetUniversalAdmin(OS os, string[] args)
        {
            List<Computer> computerListExtensionExtensionExtension = os.netMap.nodes;
            List<int> visbleCompsExtensionExtensionExtension = os.netMap.visibleNodes;
            string str = os.thisComputer.ip;
            for (int index = 0; index < computerListExtensionExtensionExtension.Count; ++index)
            {
                computerListExtensionExtensionExtension[index].adminIP = os.thisComputer.ip;
            }
        }
        public static void HackComputer(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            int RAMAvailable = os.ramAvaliable;
            //TODO: Make HackComputer Command
        }
        public static void ChangeUserDetails(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            string oldUser = args[1];
            string newUser = args[2];
            string newPass = args[3];
            if (args.Length < 3)
            {
                os.write("Usage: changeUserDetails (NewPassword)");
            }
            for (int index = 0; index < computer.users.Count; ++index)
            {
                if (computer.users[index].name.ToLower().Equals(oldUser))
                    computer.users[index] = new UserDetail(newUser, newPass, (byte)0);
                UserDetail user = computer.users[index];
                user.known = true;
            }
        }
        public static void GenerateExampleAcadmicRecord(OS os, string[] args)
        {
            Computer computer = os.thisComputer;
            Folder folder = os.thisComputer.files.root.searchForFolder("home");
            string File = "FULL NAME HERE\n\n--------------------\nDEGREE HERE\nUNIVERSITY HERE\nGPA HERE";
            folder.files.Add(new FileEntry(File, "FULL NAME HERE")); 
        }
        public static void GenerateExampleMedicalRecord(OS os, string[] args)
        {
            Computer computer = os.thisComputer;
            Folder folder = os.thisComputer.files.root.searchForFolder("home");
            string File = "FIRST NAME HERE\n--------------------\nLAST NAME HERE\n--------------------\nmale OR female\n--------------------\nDATE OF BIRTH HERE TIME OF BIRTH HERE\n--------------------\nMedical Record\nDate of Birth :: DATE OF BIRTH HERE TIME OF BIRTH HERE\nBlood Type :: BLOOD TYPE HERE\nHeight :: HEIGHT HERE IN CM\n Allergies :: ALLERGIES HERE\nActive Prescriptions :: ACTIVE PRESCRIPTSION HERE\nRecorded Visits :: RECORDED VISTS HERE\nNotes :: NOTES HERE";
            folder.files.Add(new FileEntry(File, "LASTNAMEHERE FIRSTNAMEHERE"));
        }
        public static void ExecuteHack(OS os, string[] args)
        {
            string HackerScript = args[1];
            if (args.Length < 0)
            {
                os.write("Usage: executeHack (HackerScriptLocation)\nHacker script must be in Content/HackerScripts");
                return;
            }
            HackerScriptExecuter.runScript("HackerScripts/" + HackerScript, (string)null);
        }
        public static void DeleteWhitelistDLL(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            List<int> FolderPath = new List<int>();
            FolderPath.Add(5);
            Folder folder = computer.files.root.searchForFolder("Whitelist");
            for (int index = 0; index < folder.files.Count; ++index)
            {
                if (folder.files[index].name.Equals("authenticator.dll"))
                {
                    folder.files.Remove(folder.files[index]);
                    os.execute("connect " + computer.ip);
                }
            }
        }
        public static void ChangeMusic(OS os, string[] args)
        {
            string SongInput = args[1];
            string Song = args[1].Replace("/", "\\");
            if (Song == SongInput)
            {
                Song = args[1].Replace("\\", "\\");
            }
            MusicManager.playSongImmediatley(Song);
        }
        public static void CrashComputer(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            computer.crash(os.thisComputer.ip);
        }
        public static void AddProxy(OS os, string[] args)
        {
            string ProxyTimeInput = args[1];
            float.TryParse(ProxyTimeInput, out float ProxyTime);
            Computer computer = os.connectedComp;
            if (args.Length < 0)
            {
                os.write("Usage: addProxy (Time)");
                return;
            }
            computer.addProxy(ProxyTime);
        }
        public static void AddFirewall(OS os, string[] args)
        {
            string Solution = args[1];
            string LevelInput = args[2];
            string AdditionalTimeInput = args[3];
            int Level = Convert.ToInt32(LevelInput);
            float.TryParse(AdditionalTimeInput, out float AdditionalTime);
            Computer computer = os.connectedComp;
            if (args.Length == 1 && args[1] != null)
            {
                computer.addFirewall(Level);
            }
            else if (args.Length == 2 && args[1] != null && args[2] != null)
            {
                computer.addFirewall(Level, Solution);
            }
            else if (args.Length == 3 && args[1] != null && args[2] != null && args[3] != null)
            {
                computer.addFirewall(Level, Solution, AdditionalTime);
            }
            else
            {
                os.write("Usage: addFirewall (Level) [Solution] [AdditionalTime]");
            }
        }
        public static void AddUser(OS os, string[] args)
        {
            string Username = args[1];
            string Password = args[2];
            string TypeInput = args[3];
            byte Type = Convert.ToByte(TypeInput);
            Computer computer = os.connectedComp;
            if (args.Length < 3)
            {
                os.write("Usage: addUser (Username) (Password) (Type)");
                return;
            }
            computer.addNewUser(os.thisComputer.ip, Username, Password, Type);
        }
        /*public static void OpenPort(OS os, string[] args)
        {
            int port = Convert.ToInt32(args[1]);
            string ip = os.thisComputer.ip;
            Computer computer = os.connectedComp;
            Console.WriteLine(computer.ports);
            if (port == 22)
            {
                computer.openPort(22, ip);
            }
            else if (port == 21)
            {
                computer.openPort(21, ip);
            }
            else if (port == 25)
            {
                computer.openPort(25, ip);
            }
            else if (port == 80)
            {
                computer.openPort(80, ip);
            }
            else if (port == 1433)
            {
                computer.openPort(1433, ip);
            }
            else if (port == 3724)
            {
                computer.openPort(3724, ip);
            }
            else if (port == 104)
            {
                computer.openPort(104, ip);
            }
            else if (port == 3659)
            {
                computer.openPort(3659, ip);
            }
            else if (port == 192)
            {
                computer.openPort(192, ip);
            }
            else if (port == 6881)
            {
                computer.openPort(6881, ip);
            }
            else if (port == 443)
            {
                computer.openPort(443, ip);
            }
            else if (port == 9418)
            {
                computer.openPort(9418, ip);
            }
        }
        public static void ClosePort(OS os, string[] args)
        {
            int port = Convert.ToInt32(args[1]);
            string ip = os.thisComputer.ip;
            Computer computer = os.connectedComp;
            Console.WriteLine(computer.ports);
            if (port == 22)
            {
                computer.closePort(22, ip);
            }
            else if (port == 21)
            {
                computer.closePort(21, ip);
            }
            else if (port == 25)
            {
                computer.closePort(25, ip);
            }
            else if (port == 80)
            {
                computer.closePort(80, ip);
            }
            else if (port == 1433)
            {
                computer.closePort(1433, ip);
            }
            else if (port == 3724)
            {
                computer.closePort(3724, ip);
            }
            else if (port == 104)
            {
                computer.closePort(104, ip);
            }
            else if (port == 3659)
            {
                computer.closePort(3659, ip);
            }
            else if (port == 192)
            {
                computer.closePort(192, ip);
            }
            else if (port == 6881)
            {
                computer.closePort(6881, ip);
            }
            else if (port == 443)
            {
                computer.closePort(443, ip);
            }
            else if (port == 9418)
            {
                computer.closePort(9418, ip);
            }
        } */
        public static void OpenPort(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            if (args.Length < 1)
            {
                os.write("Usage: openPort (PortToopen)");
                return;
            }
            int port = Convert.ToInt32(args[1]);
            string ip = os.thisComputer.ip;
            computer.openPort(port, ip);
        }
        public static void ClosePort(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            if (args.Length < 1)
            {
                os.write("Usage: closePort (PortToClose)\n");
                
            }
            int port = Convert.ToInt32(args[1]);
            string ip = os.thisComputer.ip;
            computer.closePort(port, ip);
        }
        public static void RemoveProxy(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            computer.hasProxy = false;
        }
        public static void RemoveFirewall(OS os, string[] args)
        {
            Computer computer = os.connectedComp;
            computer.firewall = null;
        }
        public static void AddComputer(OS os, string[] args)
        {
            if (args.Length < 5)
            {
                os.write("Usage: addComputer (Name) (IP) (SecurityLevel) (CompType) (ID)");
            }
            try
            {
                int IsNumber = Convert.ToInt32(args[3]);
                byte IsByte = Convert.ToByte(args[4]);
            }
            catch
            {
                os.write("Usage: addComputer (Name) (IP) (SecurityLevel) (CompType) (ID)");
            }
            string Name = args[1];
            string IP = args[2];
            int SecurityLevel = Convert.ToInt32(args[3]);
            byte CompType = Convert.ToByte(args[4]);
            string ID = args[5];
            Computer computer = new Computer(Name, IP, os.netMap.getRandomPosition(), SecurityLevel, CompType, os);
            computer.idName = ID;
            os.netMap.nodes.Add(computer); // If you are adding a new computer, you must add the object to nodes list
        }
        public static void DefineComputer(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            string Action = args[2];
            string ActionArgs = args[3];

            if (Action == "tracetime")
            {
                float.TryParse(ActionArgs, out float TraceTime);
                computer.traceTime = TraceTime;
            }
            else if (Action == "isadminsuper")
            {
                bool success = bool.TryParse(ActionArgs, out bool TrueOrFalse);
                if (!success)
                {
                    TrueOrFalse = computer.admin.IsSuper;
                }
                computer.admin.IsSuper = TrueOrFalse;
            }
        }
        public static void PlaySFX(OS os, string[] args)
        {
            SoundEffect sound = os.content.Load<SoundEffect>(args[1]);
            sound.Play();
        }
        public static void GetMoreRAM(OS os, string[] args)
        {
            os.totalRam = 2048;
        }
        public static void SetFaction(OS os, string[] args)
        {
            string factionInput = args[1];
            if (factionInput == "entropy")
            {
                os.allFactions.setCurrentFaction("entropy", os);
            }
            else if (factionInput == "csec")
            {
                os.allFactions.setCurrentFaction("hub", os);
            }
            else if (factionInput == "bibliotheque")
            {
                os.allFactions.setCurrentFaction("Bibliotheque", os);
            }
            else
            {
                os.write("Usage: setFaction entropy/csec");
            }
        }
        public static void TracedBehind250Proxies(OS os, string[] args)
        {
            os.traceTracker.start(500f);
        }
        public static void OxygencraftStorageFaciltyCache(OS os, string[] args) // Don't tell anyone about this command, keep it a secret
        {
            Computer computer = new Computer("oxygencraft Storage Facility", "4825.18.385.2956", os.netMap.getRandomPosition(), 2000, 2, os);
            computer.idName = "oxyStorageCache";
            computer.adminPass = "edhufguHUFHJGHLRWEHU32867837@!^&*$^&#@^&74";
            computer.admin.IsSuper = true;
            computer.admin.ResetsPassword = true;
            computer.addFirewall(25, "ijijUFERHUHUGR2184327567uGgyregyhwuiEHUT43UHI887328", 15);
            computer.portsNeededForCrack = 13;
            computer.addProxy(3600);
            computer.HasTracker = true;
            computer.traceTime = 45f;
            computer.ports.Add(1433);
            computer.ports.Add(104);
            computer.ports.Add(3724);
            computer.ports.Add(443);
            computer.ports.Add(6881);
            computer.ports.Add(192);
            computer.ports.Add(3659);
            computer.ports.Add(9418);
            for (int index = 0; index < computer.users.Count; ++index)
            {
                if (computer.users[index].name.ToLower().Equals("admin"))
                {
                    UserDetail user = computer.users[index];
                    if (os.username == "oxygencraft" || os.username == "oxygencraft2" || os.username == "oxygencraft3" || os.username == "oxygencraft4")
                    {
                        user.known = true;
                    }
                }
            }
            Folder bin = computer.files.root.searchForFolder("bin");
            bin.files.Add(new FileEntry(PortExploits.crackExeData[25], Utils.GetNonRepeatingFilename("SMTPoverflow", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[22], Utils.GetNonRepeatingFilename("SSHCrack", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[21], Utils.GetNonRepeatingFilename("FTPBounce", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[211], Utils.GetNonRepeatingFilename("FTPSprint", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[80], Utils.GetNonRepeatingFilename("WebServerWorm", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[1433], Utils.GetNonRepeatingFilename("SQL_MemCorrupt", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[104], Utils.GetNonRepeatingFilename("KBT_PortTest", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[3724], Utils.GetNonRepeatingFilename("WoWHack", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[443], Utils.GetNonRepeatingFilename("SSLTrojan", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[6881], Utils.GetNonRepeatingFilename("TorrentStreamInjector", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[192], Utils.GetNonRepeatingFilename("PacificPortcrusher", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[3659], Utils.GetNonRepeatingFilename("confloodEOS", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[9418], Utils.GetNonRepeatingFilename("GitTunnel", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[4], Utils.GetNonRepeatingFilename("SecurityTracer", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[9], Utils.GetNonRepeatingFilename("Decypher", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[10], Utils.GetNonRepeatingFilename("DECHead", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[11], Utils.GetNonRepeatingFilename("Clock", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[12], Utils.GetNonRepeatingFilename("TraceKill", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[13], Utils.GetNonRepeatingFilename("eosDeviceScan", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[14], Utils.GetNonRepeatingFilename("themechanger", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[15], Utils.GetNonRepeatingFilename("hacknet", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[16], Utils.GetNonRepeatingFilename("HexClock", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[17], Utils.GetNonRepeatingFilename("Sequencer", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[31], Utils.GetNonRepeatingFilename("KaguyaTrials", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[32], Utils.GetNonRepeatingFilename("SignalScramble", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[33], Utils.GetNonRepeatingFilename("MemForensics", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[34], Utils.GetNonRepeatingFilename("MemDumpGenerator", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[35], Utils.GetNonRepeatingFilename("NetMapOrganizer", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[36], Utils.GetNonRepeatingFilename("ComShell", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[37], Utils.GetNonRepeatingFilename("DNotes", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[38], Utils.GetNonRepeatingFilename("ClockV2", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.crackExeData[39], Utils.GetNonRepeatingFilename("TuneSwap", ".exe", bin)));
            bin.files.Add(new FileEntry(PortExploits.DangerousPacemakerFirmware, Utils.GetNonRepeatingFilename("PacemakerDangerous", ".dll", bin)));
            bin.files.Add(new FileEntry(PortExploits.ValidPacemakerFirmware, Utils.GetNonRepeatingFilename("PacemakerWorking", ".dll", bin)));
            bin.files.Add(new FileEntry(PortExploits.ValidAircraftOperatingDLL, Utils.GetNonRepeatingFilename("747FlightSystem", ".dll", bin)));
            os.netMap.nodes.Add(computer);
        }
        public static void DisableEmailIcon(OS os, string[] args)
        {
            os.DisableEmailIcon = true;
        }
        public static void EnableEmailIcon(OS os, string[] args)
        {
            os.DisableEmailIcon = false;
        }
        public static void NodeRestore(OS os, string[] args)
        {
            DLC1SessionUpgrader.ReDsicoverAllVisibleNodesInOSCache(os);
        }
        public static void AddRestoreCircle(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            SFX.addCircle(computer.getScreenSpacePosition(), Utils.AddativeWhite * 0.4f, 70f);
        }
        public static void WhitelistBypass(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            Folder folder = computer.files.root.searchForFolder("Whitelist");
            for (int index = 0; index < folder.files.Count; ++index)
            {
                if (folder.files[index].name.Equals("list.txt") || folder.files[index].name.Equals("source.txt"))
                {
                    folder.files.Remove(folder.files[index]);
                    folder.files.Add(new FileEntry(os.thisComputer.ip, "list.txt"));
                    os.execute("connect " + computer.ip);
                }
            }
        }
        public static void SetTheme(OS os, string[] args)
        {
            OSTheme Theme;
            string ThemeInput = args[1];
            if (ThemeInput == "TerminalOnly")
            {
                Theme = OSTheme.TerminalOnlyBlack;
                ThemeManager.switchTheme(os, Theme);
            }
            else if (ThemeInput == "Blue")
            {
                Theme = OSTheme.HacknetBlue;
                ThemeManager.switchTheme(os, Theme);
            }
            else if (ThemeInput == "Teal")
            {
                Theme = OSTheme.HacknetTeal;
                ThemeManager.switchTheme(os, Theme);
            }
            else if (ThemeInput == "Yellow")
            {
                Theme = OSTheme.HacknetYellow;
                ThemeManager.switchTheme(os, Theme);
            }
            else if (ThemeInput == "Green")
            {
                Theme = OSTheme.HackerGreen;
                ThemeManager.switchTheme(os, Theme);
            }
            else if (ThemeInput == "White")
            {
                Theme = OSTheme.HacknetWhite;
                ThemeManager.switchTheme(os, Theme);
            }
            else if (ThemeInput == "Purple")
            {
                Theme = OSTheme.HacknetPurple;
                ThemeManager.switchTheme(os, Theme);
            }
            else if (ThemeInput == "Mint")
            {
                Theme = OSTheme.HacknetMint;
                ThemeManager.switchTheme(os, Theme);
            }
            else if (ThemeInput == "Colamaeleon")
            {
                Theme = OSTheme.Colamaeleon;
                ThemeManager.switchTheme(os, Theme);
            }
            else if (ThemeInput == "GreenCompact")
            {
                Theme = OSTheme.GreenCompact;
                ThemeManager.switchTheme(os, Theme);
            }
            else if (ThemeInput == "Riptide")
            {
                Theme = OSTheme.Riptide;
                ThemeManager.switchTheme(os, Theme);
            }
            else if (ThemeInput == "Riptide2")
            {
                Theme = OSTheme.Riptide2;
                ThemeManager.switchTheme(os, Theme);
            }
            else
            {
                os.write("Usage: setTheme: (Theme)\nValid Options: TerminalOnly,Blue,Teal,Yellow,Green,White,Purple,Mint,Colamaeleon,GreenCompact,Riptide,Riptide2");
            }
        }
        public static void SetCustomTheme(OS os, string[] args)
        {
            ThemeManager.switchTheme(os, args[1]);
        }
        public static void LinkComputer(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            Computer computer2 = Programs.getComputer(os, args[2]);
            try
            {
                string IsComputer = computer2.adminPass;
            }
            catch
            {
                os.write("Usage: linkComputer: (SourceIP) (RemoteIP)");
            }
            computer.links.Add(os.netMap.nodes.IndexOf(computer2));
        }
        public static void UnlinkComputer(OS os, string[] args)
        {
            Computer computer1 = Programs.getComputer(os, args[1]);
            Computer computer2 = Programs.getComputer(os, args[2]);
            computer1.links.Remove(os.netMap.nodes.IndexOf(computer2));
        }
        public static void LoseAllNodes(OS os, string[] args)
        {
            for (int index = 1; index < os.netMap.nodes.Count; ++index)
            {
                Computer computer = os.netMap.nodes[index];
                XNA.Vector2 Pos = computer.getScreenSpacePosition();
                List<TraceKillExe.PointImpactEffect> ImpactEffects = new List<TraceKillExe.PointImpactEffect>();
                ImpactEffects.Add(new TraceKillExe.PointImpactEffect()
                {
                    location = Pos,
                    scaleModifier = (float)(3.0 + (computer.securityLevel > 2 ? 1.0 : 0.0)),
                    cne = new ConnectedNodeEffect(os, true),
                    timeEnabled = 0.0f,
                    HasHighlightCircle = true
                });
                os.netMap.visibleNodes.Remove(index);
            }
        }
        public static void LoseNode(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            int CompToRemove = os.netMap.nodes.IndexOf(computer);
            XNA.Vector2 Pos = computer.getScreenSpacePosition();
            List<TraceKillExe.PointImpactEffect> ImpactEffects = new List<TraceKillExe.PointImpactEffect>();
            ImpactEffects.Add(new TraceKillExe.PointImpactEffect()
            {
                location = Pos,
                scaleModifier = (float)(3.0 + (computer.securityLevel > 2 ? 1.0 : 0.0)),
                cne = new ConnectedNodeEffect(os, true),
                timeEnabled = 0.0f,
                HasHighlightCircle = true
            });
            os.netMap.visibleNodes.Remove(CompToRemove);
        }
        public static void RevealNode(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            int CompToReveal = os.netMap.nodes.IndexOf(computer);
            os.netMap.visibleNodes.Add(CompToReveal);
        }
        public static void RemoveComputer(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            int CompToRemove = os.netMap.nodes.IndexOf(computer);
            XNA.Vector2 Pos = computer.getScreenSpacePosition();
            List<TraceKillExe.PointImpactEffect> ImpactEffects = new List<TraceKillExe.PointImpactEffect>();
            ImpactEffects.Add(new TraceKillExe.PointImpactEffect()
            {
                location = Pos,
                scaleModifier = (float)(3.0 + (computer.securityLevel > 2 ? 1.0 : 0.0)),
                cne = new ConnectedNodeEffect(os, true),
                timeEnabled = 0.0f,
                HasHighlightCircle = true
            });
            os.netMap.visibleNodes.Remove(CompToRemove);
            os.netMap.nodes.Remove(computer);
        }
        public static void ResetIP(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            computer.ip = NetworkMap.generateRandomIP();
        }
        public static void ResetPlayerCompIP(OS os, string[] args)
        {
            os.thisComputerIPReset();
        }
        public static void SetIP(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            computer.ip = args[2];
        }
        public static void ShowFlags(OS os, string[] args)
        {
            os.write(os.Flags.GetSaveString());
        }
        public static void AddFlag(OS os, string[] args)
        {
            os.Flags.AddFlag(args[1]);
        }
        public static void RemoveFlag(OS os, string[] args)
        {
            os.Flags.RemoveFlag(args[1]);
        }
        public static void AuthenticateToIRC(OS os, string[] args)
        {
            os.Flags.RemoveFlag("DLC_Player_IRC_Authenticated");
        }
        public static void AddAgentToIRC(OS os, string[] args)
        {
            Computer computerobject = Programs.getComputer(os, args[1]);
            if (args.Length < 6)
            {
                os.write("Usage: addAgentToIRC (NameORIDORIP) (AgentName) (AgentPassword) (AgentColourRed) (AgentColourBlue) (AgentColourGreen)");
                return;
            }
            try
            {
                string IsComp = computerobject.adminIP;
                int IsNum = Convert.ToInt32(args[4]);
                int IsNum2 = Convert.ToInt32(args[5]);
                int IsNum3 = Convert.ToInt32(args[6]);
                if (args[2] == null || args[3] == null || args[4] == null || args[5] == null || args[6] == null)
                {
                    int ThrowError = Convert.ToInt32("a");
                }
            }
            catch
            {
                os.write("Usage: addAgentToIRC (NameORIDORIP) (AgentName) (AgentPassword) (AgentColourRed) (AgentColourBlue) (AgentColourGreen)");
            }
            string computer = computerobject.idName;
            DLCHubServer IRC = Programs.getComputer(os, computer).getDaemon(typeof(DLCHubServer)) as DLCHubServer;
            Color colour = new Color(Convert.ToInt32(args[4]), Convert.ToInt32(args[5]), Convert.ToInt32(args[6]));
            IRC.AddAgent(args[2], args[3], colour);
        }
        public static void SetCompPorts(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            ComputerLoader.loadPortsIntoComputer(args[2], computer);
        }
        public static void AddCustomPortToComp(OS os, string[] args)
        {
            //TODO: Make AddCustomPortToComp
        }
        public static void RemoveCustomPortFromComp(OS os, string[] args)
        {
            //TODO: Make RemoveCustomPortFromComp
            /*
            Computer computer = Programs.getComputer(os, args[1]);
            Pathfinder.Game.Computer.Extensions.GetModdedPortList(computer);
            */
        }
        public static void AddSongChangerDaemon(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            SongChangerDaemon daemon = new SongChangerDaemon(computer, os);
            computer.daemons.Add(daemon);
        }
        public static void AddRicerConnectDaemon(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            CustomConnectDisplayDaemon daemon = new CustomConnectDisplayDaemon(computer, os);
            computer.daemons.Add(daemon);
        }
        public static void AddDLCCreditsDaemon(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            DLCCreditsDaemon daemon = new DLCCreditsDaemon(computer, os);
            computer.daemons.Add(daemon);
        }
        public static void AddIRCDaemon(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            IRCDaemon daemon = new IRCDaemon(computer, os, args[2]);
            computer.daemons.Add(daemon);
        }
        public static void AddISPDaemon(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            ISPDaemon daemon = new ISPDaemon(computer, os);
            computer.daemons.Add(daemon);
        }
        public static void Quit(OS os, string[] args)
        {
            Game1.getSingleton().Exit();
        }
        public static void DeleteLogs(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            //Console.WriteLine("Computer object obtained");
            Folder folder = computer.files.root.searchForFolder("log");
            //Console.WriteLine("Folder object obtained");
            folder.files.Clear();
            //Console.WriteLine("Deleted all logs");
        }
        public static void ForkbombProof(OS os, string[] args)
        {
            os.totalRam = 1000000000;
        }
        public static void ChangeCompIcon(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            computer.icon = args[2];
        }
        public static void RemoveSongChangerDaemon(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            Daemon daemon = computer.getDaemon(typeof(SongChangerDaemon));
            computer.daemons.Remove(daemon);
        }
        public static void RemoveRicerConnectDaemon(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            Daemon daemon = computer.getDaemon(typeof(CustomConnectDisplayDaemon));
            computer.daemons.Remove(daemon);
        }
        public static void RemoveDLCCreditsDaemon(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            Daemon daemon = computer.getDaemon(typeof(DLCCreditsDaemon));
            computer.daemons.Remove(daemon);
        }
        public static void RemoveIRCDaemon(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            Daemon daemon = computer.getDaemon(typeof(IRCDaemon));
            computer.daemons.Remove(daemon);
        }
        public static void RemoveISPDaemon(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            Daemon daemon = computer.getDaemon(typeof(ISPDaemon));
            computer.daemons.Remove(daemon);
        }
        public static void ForkbombVirus(OS os, string[] args) // Bugged
        {
            for (int index = 1; index < os.netMap.nodes.Count; ++index)
                os.netMap.nodes[index].crash(os.thisComputer.ip);
        }
        public static void InstallInviolabilty(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            computer.portsNeededForCrack = 9999999;
        }
        public static void RemoveAllDaemons(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            computer.daemons.Clear();
        }
        public static void ShowIPNamesAndID(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            os.write("ID: " + computer.idName);
            os.write("Name: " + computer.name);
            os.write("IP: " + computer.ip);
        }
        public static void SummonDebugModDaemonComp(OS os, string[] args)
        {
            //TODO: Make SummonDebugModDaemonComp
            /*
            Computer computer = new Computer("DebugMod Comp", NetworkMap.generateRandomIP(), os.netMap.getRandomPosition(), 50000, 2, os);
            computer.idName = "debugMod";
            os.netMap.nodes.Add(computer);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Pathfinder.Game.Computer.Extensions.AddModdedDaemon(computer, "DebugModDaemon");
            os.execute("connect " + computer.ip);
            */
        }
        public static void ChangeAdmin(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            if (args[2] == "basic")
            {
                computer.admin = new BasicAdministrator();
            }
            else if (args[2] == "fastbasic")
            {
                computer.admin = new FastBasicAdministrator();
            }
            else if (args[2] == "fastprogress")
            {
                computer.admin = new FastProgressOnlyAdministrator();
            }
            else if (args[2] == "alwaysactive")
            {
                computer.admin = new AlwaysActiveAdmin();
            }
            else if (args[2] == "none")
            {
                computer.admin = null;
            }
            else
            {
                os.write("Usage: changeAdmin (IDORIPORName) (Admin)");
                os.write("Valid options: basic,fastbasic,fastprogress,alwaysactive,none");
            }
        }
        public static void ViewAdmin(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            if (computer.admin == new BasicAdministrator())
            {
                os.write("Basic Admin");
            }
            else if (computer.admin == new FastBasicAdministrator())
            {
                os.write("Fast Basic Admin");
            }
            else if (computer.admin == new FastProgressOnlyAdministrator())
            {
                os.write("Fast Progress Admin");
            }
            else if (computer.admin == new AlwaysActiveAdmin())
            {
                os.write("Always Active Admin");
            }
            else
            {
                os.write("You may of entered the computer incorrectly or there is no admin for the computer.");
                os.write("Usage: viewAdmin (IPORIDORName)");
            }
        }

        public static void ReplayPlaneMission(OS os, string[] args)
        {
            Computer DHS = Programs.getComputer(os, "dhs");
            Computer CrashPlane = Programs.getComputer(os, "dair_crash");
            Computer SecondaryPlane = Programs.getComputer(os, "dair_secondary");
            Folder DHSFolder = DHS.files.root.searchForFolder("HomeBase");
            Folder CrashPlaneFolder = CrashPlane.files.root.searchForFolder("FlightSystems");
            Folder SecondaryPlaneFolder = SecondaryPlane.files.root.searchForFolder("FlightSystems");
            AircraftDaemon CrashPlaneDaemon = ((AircraftDaemon)CrashPlane.getDaemon(typeof(AircraftDaemon)));
            AircraftDaemon SecondaryPlaneDaemon = ((AircraftDaemon)SecondaryPlane.getDaemon(typeof(AircraftDaemon)));
            if (!os.Flags.HasFlag("dlc_complete") && args.Length == 1)
            {
                os.write("You have not completed the dlc yet, if you want spoilers and wish to continue, type this command:");
                os.write("replayPlaneMission y");
                return;
            }
            os.homeAssetServerID = "dhsDrop";
            os.homeNodeID = "dhs";
            os.currentMission = null;
            os.delayer.RunAllDelayedActions();
            os.IsInDLCMode = true;
            os.allFactions.setCurrentFaction("Bibliotheque", os);
            DLCHubServer dlcHubServer = (DLCHubServer)Programs.getComputer(os, "dhs").getDaemon(typeof(DLCHubServer));
            os.currentFaction.playerValue = 1;
            int num = 10;
            for (int index = 0; index < num; ++index)
            {
                MissionFunctions.runCommand(1, "addRankSilent");
                if (index + 1 < num)
                {
                    os.delayer.RunAllDelayedActions();
                    dlcHubServer.DelayedActions.InstantlyResolveAllActions(os);
                    dlcHubServer.ClearAllActiveMissions();
                }
            }
            os.mailicon.isEnabled = false;
            CrashPlane.ip = "209.15.13.134";
            SecondaryPlane.ip = "208.73.211.70";
            if (!CrashPlaneFolder.containsFile("747FlightOps.dll"))
            {
                CrashPlaneFolder.files.Add(new FileEntry(PortExploits.ValidAircraftOperatingDLL, "747FlightOps.dll"));
            }
            if (!SecondaryPlaneFolder.containsFile("747FlightOps.dll"))
            {
                SecondaryPlaneFolder.files.Add(new FileEntry(PortExploits.ValidAircraftOperatingDLL, "747FlightOps.dll"));
            }
            CrashPlaneDaemon.IsInCriticalFirmwareFailure = false;
            CrashPlaneDaemon.CurrentAltitude = 30000;
            SecondaryPlaneDaemon.IsInCriticalFirmwareFailure = false;
            SecondaryPlaneDaemon.CurrentAltitude = 30000;
            DHSFolder.files.Remove(DHSFolder.files[1]);
            DHSFolder.files.Add(new FileEntry(LongVariables.IRCLog(os), "active.log"));
            os.Flags.RemoveFlag("dlc_complete");
            os.Flags.RemoveFlag("dlc_complete_FromUnknown");
            os.Flags.RemoveFlag("dlc_complete_FromCSEC");
            os.Flags.RemoveFlag("dlc_complete_FromEntropy");
            os.Flags.RemoveFlag("DLC_PlaneCrashResponseTriggered");
            os.Flags.RemoveFlag("DLC_DoubleCrashResponseTriggered");
            os.Flags.RemoveFlag("DLC_PlaneSaveResponseTriggered");
            os.Flags.RemoveFlag("DLC_PlaneResult");
            os.Flags.RemoveFlag("AircraftInfoOverlayDeactivated");
            os.Flags.RemoveFlag("AircraftInfoOverlayActivated");
            ComputerLoader.loadMission("Content/DLC/Missions/Airline2/Missions/AirlineMission2_Player.xml");
            for (int index = 0; index < os.netMap.visibleNodes.Count; index++)
            {
                string str = os.PreDLCVisibleNodesCache + (os.PreDLCVisibleNodesCache.Length > 0 ? (object)"," : (object)"") + (object)os.netMap.visibleNodes[index];
                os.PreDLCVisibleNodesCache = str;
            }
            os.execute("loseAllNodes");
            while (!os.Flags.HasFlag("AircraftInfoOverlayDeactivated"))
            {

            }
            DLC1SessionUpgrader.EndDLCSection(os);
        }
        public static void ReplayPlaneMissionSecondary(OS os, string[] args)
        {
            Hacknet.Misc.SessionAccelerator.AccelerateSessionToDLCEND(os);   
        }
        public static void TellPeopleYouAreGonnaHackThemOnline(Hacknet.OS os, string[] args)
        {
            os.traceTracker.start(200f);   
        }
        public static void MyFatherIsCCC(OS os, string[] args)
        {
            os.traceTracker.start(5f);
        }
        public static void CantTouchThis(OS os, string[] args)
        {
            os.traceTracker.start(99999f);
        }
        public static void ViewFaction(OS os, string[] args)
        {
            os.write(os.currentFaction.name);
        }
        public static void ViewPlayerVal(OS os, string[] args)
        {
            os.write(Convert.ToString(os.currentFaction.playerValue));
        }
        public static void KaguyaTrialEffect(OS os, string[] args)
        {
            int y = 0;
            Rectangle location = new Rectangle(os.ram.bounds.X, y, RamModule.MODULE_WIDTH, (int)OS.EXE_MODULE_HEIGHT);
            DLCIntroExe exe = new DLCIntroExe(location, os, null);
            AccessBypass.CallPrivateMethod<DLCIntroExe>(exe, "AddRadialMailLine");
        }
        public static void KaguyaTrialEffect2(OS os, string[] args)
        {
            //float timeInExplosion = 0f;
            int y = 0;
            Rectangle location = new Rectangle(os.ram.bounds.X, y, RamModule.MODULE_WIDTH, (int)OS.EXE_MODULE_HEIGHT);
            DLCIntroExe exe = new DLCIntroExe(location, os, null);
            AccessBypass.CallPrivateMethod<DLCIntroExe>(exe, "UpdateUIFlickerIn");
        }
        public static void KaguyaTrialEffect3(OS os, string[] args)
        {
            int y = 0;
            Rectangle location = new Rectangle(os.ram.bounds.X, y, RamModule.MODULE_WIDTH, (int)OS.EXE_MODULE_HEIGHT);
            DLCIntroExe exe = new DLCIntroExe(location, os, null);
            AccessBypass.CallPrivateMethod<DLCIntroExe>(exe, "CompleteMailPhaseOut");
        }
        public static void AntiTrace(OS os, string[] args)
        {
            while (!os.Flags.HasFlag("Stop_Anti_Trace"))
            {
                os.traceTracker.stop();
            }
            os.Flags.RemoveFlag("Stop_Anti_Trace");
        }
        public static void StopAntiTrace(OS os, string[] args)
        {
            os.Flags.AddFlag("Stop_Anti_Trace");
        }
        public static void RunHackerScriptFunction(OS os, string[] args)
        {
            string sourceComp = args[1];
            string targetComp = args[2];
            string command = args[3];
            string[] functionArgs = new string[] { sourceComp, targetComp, command };
        }
        public static void AircraftNuke(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            //AircraftDaemon computerdaemon = (AircraftDaemon)computer.getDaemon(typeof(AircraftDaemon));
            AircraftDaemon computerdaemon = new AircraftDaemon(computer, os, "OxyNuke", new Vector2(20f, 20f), new Vector2(50f, 50f), 50f);
            computer.daemons.Add(computerdaemon);
            computerdaemon.isListed = false;
            computerdaemon.IsInCriticalFirmwareFailure = true;
            computerdaemon.CallPrivateMethod<AircraftDaemon>("CrashAircraft", null);
            computerdaemon.StartReloadFirmware();
        }
        public static void AddAircraftDaemon(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            float.TryParse(args[3], out float float1);
            float.TryParse(args[4], out float float2);
            float.TryParse(args[5], out float float3);
            float.TryParse(args[6], out float float4);
            float.TryParse(args[7], out float float5);
            Vector2 origin = new Vector2(float1, float2);
            Vector2 dest = new Vector2(float3, float4);
            AircraftDaemon daemon = new AircraftDaemon(computer, os, args[2], origin, dest, float5);
            computer.daemons.Add(daemon);
        }
        public static void SetAircraftAltitude(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            AircraftDaemon aircraft = (AircraftDaemon)computer.getDaemon(typeof(AircraftDaemon));
            aircraft.CurrentAltitude = Convert.ToDouble(args[2]);
        }
        public static void SetAircraftSpeed(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            AircraftDaemon aircraft = (AircraftDaemon)computer.getDaemon(typeof(AircraftDaemon));
            float.TryParse(args[2], out float speed);
            aircraft.SetPrivateField("currentAirspeed", speed);
        }
        public static void SetAircraftRateOfClimb(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            AircraftDaemon aircraft = (AircraftDaemon)computer.getDaemon(typeof(AircraftDaemon));
            float.TryParse(args[2], out float climbrate);
            aircraft.SetPrivateField("rateOfClimb", climbrate);
        }
        public static void SetAircraftFirmwareFailure(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            AircraftDaemon aircraft = (AircraftDaemon)computer.getDaemon(typeof(AircraftDaemon));
            aircraft.IsInCriticalFirmwareFailure = true;
        }
        public static void SetAircraftFirmwareSucessful(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            AircraftDaemon aircraft = (AircraftDaemon)computer.getDaemon(typeof(AircraftDaemon));
            aircraft.IsInCriticalFirmwareFailure = false;
        }
        public static void SetAircraftProgress(OS os, string[] args)
        {
            Computer computer = Programs.getComputer(os, args[1]);
            AircraftDaemon aircraft = (AircraftDaemon)computer.getDaemon(typeof(AircraftDaemon));
            float.TryParse(args[2], out float progress);
            aircraft.SetPrivateField("FlightProgress", progress);
        }
        public static void KaguyaTrialEffect5(OS os, string[] args)
        {
            SFX.addCircle(os.mailicon.pos + new Vector2(20f, 6f), Utils.AddativeRed * 0.8f, 100f);
        }
    }
}
