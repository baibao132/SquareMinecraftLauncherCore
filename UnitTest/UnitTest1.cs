using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SquareMinecraftLauncher.Core.OAuth;
using SquareMinecraftLauncher.Minecraft.MCServerPing;
using SquareMinecraftLauncher.Core.Curseforge;
using SquareMinecraftLauncher.Minecraft;
using System.Threading.Tasks;
using System.Threading;
using SquareMinecraftLauncher.Core;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1Async()
        {
            ModCurseForge curseforge = new ModCurseForge();
            var a = await curseforge.popular();
            foreach (var i in a) Console.WriteLine(i.summary);
        }
    }
}
