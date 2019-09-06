using SikaDeerLauncher.MinecraftServer.Json;
using System;
using System.Collections.Generic;

namespace MinecraftServer.Server.Forge
{
    /// <summary>
    /// Contains information about a modded server install.
    /// </summary>
    public class ForgeInfo
    {
        /// <summary>
        /// Represents an individual forge mod.
        /// </summary>
        public class ForgeMod
        {
            public ForgeMod(String ModID, String Version)
            {
                this.ModID = ModID;
                this.Version = Version;
            }

            public readonly String ModID;
            public readonly String Version;

            public override string ToString()
            {
                return ModID + " [" + Version + ']';
            }
        }

        public List<ForgeMod> Mods;


        internal ForgeInfo(jsonForge.ModListItem[] mod)
        {
            // Example ModInfo (with spacing):

            // "modinfo": {
            //     "type": "FML",
            //     "modList": [{
            //         "modid": "mcp",
            //         "version": "9.05"
            //     }, {
            //         "modid": "FML",
            //         "version": "8.0.99.99"
            //     }, {
            //         "modid": "Forge",
            //         "version": "11.14.3.1512"
            //     }, {
            //         "modid": "rpcraft",
            //         "version": "Beta 1.3 - 1.8.0"
            //     }]
            // }

            this.Mods = new List<ForgeMod>();
            foreach (var mod1 in mod)
            {
                String modid = mod1.modid;
                String version = mod1.version;

                this.Mods.Add(new ForgeMod(modid, version));
            }
        }
    }
}
