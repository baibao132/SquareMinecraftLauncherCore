using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SquareMinecraftLauncher.Core.OAuth;
using SquareMinecraftLauncher.Minecraft.MCServerPing;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            MicrosoftLogin microsoftLogin = new MicrosoftLogin();
            MinecraftLogin minecraftLogin = new MinecraftLogin();
            Xbox XboxLogin = new Xbox();
            Console.WriteLine(minecraftLogin.GetMincraftuuid(minecraftLogin.GetToken(XboxLogin.XSTSLogin(XboxLogin.GetToken(microsoftLogin.GetToken(microsoftLogin.Login(true)).access_token)))).uuid);

            //MinecraftServer.server("mssj.starmc.cn", 54520);
        }
    }
}
