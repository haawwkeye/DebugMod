using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathfinder.Daemon;
using Hacknet;
using Hacknet.Gui;
using Pathfinder;
using Hacknet.UIUtils;
using Pathfinder.Util;

namespace DebugMod
{
    //TODO: Rewrite DebugDaemon
    public class DebugDaemon : BaseDaemon
    {
        public DebugDaemon(Computer computer, string serviceName, OS opSystem) : base(computer, serviceName, opSystem) { }
        public override string Identifier => "DebugMod";

        private DebugModState State;
        private float t = 0f;
        private static Color themeColour = new Color(45, 180, 231);

        public override void draw(Rectangle bounds, SpriteBatch sb)
        {
            base.draw(bounds, sb);

            /*
            Console.WriteLine("DRAW DEBUG START");
            Console.WriteLine(State);
            Console.WriteLine(bounds);
            Console.WriteLine(sb);
            Console.WriteLine("DRAW DEBUG END");
            */

            Rectangle bounds1 = new Rectangle(bounds.X + 40, bounds.Y + 40, bounds.Width - 80, bounds.Height - 80);
            Draw(bounds, bounds1, sb);
        }

        public void Draw(Rectangle bounds, Rectangle bounds1, SpriteBatch sb) // Draws stuff on the screen, will need
        {
            t += 1.0f;
            //if (t > 50.1f)
            switch (State)
            {
                case DebugModState.HomePage:
                    DrawHome(bounds1, t, sb);
                    break;
                case DebugModState.DebugPage:
                    DrawDebug(bounds1, t, sb);
                    break;
                case DebugModState.Page1:
                    DrawPage1(bounds1, t, sb);
                    break;
                case DebugModState.Page2:
                    break;
                case DebugModState.Page3:
                    break;
                case DebugModState.Page4:
                    break;
                case DebugModState.Page5:
                    break;
                case DebugModState.Blank:
                    DrawPageRequirements(bounds, sb);
                    break;
                default:
                    State = DebugModState.HomePage;
                    break;
            }
        }

        private void DrawDebug(Rectangle rect, float ticks, SpriteBatch sb)
        {
            Color colour = new Color(45, 180, 231);
            const string doLabel = "doLabel";
            const string doMeasuredSmallLabel = "doMeasuredSmallLabel";
            const string doMeasuredTinyLabel = "doMeasuredTinyLabel";
            const string doSmallLabel = "doSmallLabel";

            TextItem.doLabel(new Vector2(500f, 400f), doLabel, null);
            TextItem.doMeasuredSmallLabel(new Vector2(500f, 500f), doMeasuredSmallLabel, null);
            TextItem.doMeasuredTinyLabel(new Vector2(500f, 600f), doMeasuredTinyLabel, null);
            TextItem.doSmallLabel(new Vector2(500f, 300f), doSmallLabel, null);

            if (Button.doButton(1, 800, 100, 200, 75, "Button", null))
                State = DebugModState.HomePage;

            Button.doButton(2, 685, 843, 25, 25, "<-", null);
            Button.doButton(2, 733, 843, 25, 25, "->", null);
        }

        private void DrawPageRequirements(Rectangle rect, SpriteBatch sb)
        {
            const string title = "Debug Mod";
            const string newVersion = "New version of Debug Mod is available";
            string newVersionLine2 = "You are currently running: " + DebugMod.version + " New version: " + DebugMod.newVersion;

            TextItem.doLabel(new Vector2(280f, 55f), title, null);

            if (DebugMod.newVersion != DebugMod.version)
            {
                TextItem.doSmallLabel(new Vector2(500f, 55f), newVersion, themeColour);
                TextItem.doSmallLabel(new Vector2(500f, 70f), newVersionLine2, themeColour);
            }
        }

        private void DrawHome(Rectangle rect, float ticks, SpriteBatch sb)
        {
            string text = @"Welcome to the Debug Mod Daemon! This mod started off from an image, you can still find it by looking
up All ports open in Hacknet Steam Artwork. Later, I gave the mod early to someone and decided to do
more and more. After a while, I made the mod public. Ok, enough chatter about the history about this
mod, let's explain how to use this daemon.

The buttons with -> and <- are the forwards and backwards buttons. These will cycle from page to
page, there are five pages (unknown yet). In one of these pages, there will be a list of commands
you can use (not all are implemented yet or some aren't possible using this daemon). When you
select a command, you will be prompted with some info or the command will execute. If some info is
optional, you can skip it by typing null.";

            DrawPageRequirements(rect, sb);

            Button.doButton(6, 673, 843, 25, 25, "<-", null);

            if (Button.doButton(49, 720, 843, 25, 25, "->", null))
                State = DebugModState.Page1;

            TextItem.doSmallLabel(new Vector2(300f, 120f), text, null);
        }

        private void DrawPage1(Rectangle rect, float ticks, SpriteBatch sb)
        {
            DrawPageRequirements(rect, sb);
            if (Button.doButton(593, 673, 843, 25, 25, "<-", null))
                State = DebugModState.HomePage;

            Button.doButton(49, 720, 843, 25, 25, "->", null);

            //Button.doButton(13, 500, 200, 100, 40, "Button", null);
            //Button.doButton(13, 650, 200, 150, 20, "Button", null); Selected
            //Button.doButton(13, 750, 400, 150, 60, "Button", null);

            if (Button.doButton(13, 300, 125, 150, 20, "Get Universal Admin", themeColour))
            {
                os.execute("getUniversalAdmin");
            }

            if (Button.doButton(4538, 300, 150, 150, 20, "Reveal All Nodes", themeColour))
            {
                os.execute("revealAll");
            }

            if (Button.doButton(132343, 300, 175, 150, 20, "Get More RAM", themeColour))
            {
                os.execute("getMoreRAM");
            }

            if (Button.doButton(1342321, 300, 200, 150, 20, "Delete Whitelist DLL", themeColour))
            {
                GetInfoTextBox("deleteWhitelistDLL", "IP, ID or Name", rect, sb);
            }

            if (Button.doButton(12312, 300, 225, 150, 20, "Forkbomb Proof", themeColour))
            {
                os.execute("forkbombProof");
            }

            if (Button.doButton(1946325, 300, 250, 150, 20, "Debug", themeColour))
            {
                os.execute("debug");
            }

            if (Button.doButton(937264, 300, 275, 150, 20, "Theme Attack", themeColour))
            {
                os.execute("themeAttack");
            }

            if (Button.doButton(174242, 300, 300, 150, 20, "Striker Attack", themeColour))
            {
                os.execute("strikerAttack");
            }

            if (Button.doButton(1342312, 300, 325, 150, 20, "Warning Flash", themeColour))
            {
                os.execute("warningFlash");
            }
        }

        private void GetInfoTextBox(string command, string textBoxTitle, Rectangle rect, SpriteBatch sb)
        {
            string input = os.terminal.currentLine;

            if (input == null || input == "")
            {
                input = "";
            }

            //string textbox = TextBox.doTerminalTextField(18835235, 250, 250, 35, 30, 2, textBoxTitle, GuiData.smallfont);

            os.execute(command + " " + input);

            //string textbox = TextBox.doTerminalTextField(18835235, 250, 250, 35, 30, 2, textBoxTitle, GuiData.smallfont);
            //throw new NotImplementedException();
            /*
            if (textbox == null)
            {
                os.write("IT DOESN'T WORK :(");
            }
            else
            {
                os.write("IT WORKS IT WORKS: " + textbox);
            }
            */
        }

        public override void loadInit()
        {
            base.registerAsDefaultBootDaemon();
            base.loadInit();
        }

        /*
        public void InitFiles() // IDK what this does, maybe creates the files, in that case no
        {
            //throw new NotImplementedException();
        }

        public void LoadInit() // IDK what this does
        {
            registerAsDefaultBootDaemon();
        }

        public void LoadInstance(Dictionary<string, string> objects) // IDK what this does
        {
            
        }

        public void OnCreate() // May or may not need this
        {

        }

        public void OnNavigatedTo() // Won't need this
        {
            State = DebugModState.HomePage;
        }

        public void OnUserAdded(string name, string pass, byte type) // Won't need this
        {
            
        }
        internal void NewCommand()
        {
            
        }
        */
        private enum DebugModState {
            HomePage,
            Page1,
            Page2,
            Page3,
            Page4,
            Page5,
            DebugPage,
            Blank,
        }
    }
}
