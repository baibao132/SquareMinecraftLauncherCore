﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Xbox XboxLogin = new Xbox();
            Console.WriteLine(new MinecraftLogin().GetToken(XboxLogin.XSTSLogin(XboxLogin.GetToken(microsoftLogin.GetToken(microsoftLogin.Login()).access_token))));
            //MinecraftServer.server("mssj.starmc.cn", 54520);
        }
    }
}
