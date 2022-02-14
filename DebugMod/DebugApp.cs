using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hacknet;
using Microsoft.Xna.Framework;
using Pathfinder;

namespace DebugMod
{
    class DebugAppExe : Pathfinder.Executable.BaseExecutable
    {
        public DebugAppExe(Rectangle location, OS operatingSystem, string[] args) : base(location, operatingSystem, args)
        {
            needsProxyAccess = false;
            ramCost = 500;
            IdentifierName = "DebugAppExe";
        }

        public override void Update(float t)
        {
            base.Update(t);
        }

        public override void Completed()
        {
            base.Completed();
        }
    }
}
