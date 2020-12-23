using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SquareMinecraftLauncher.Core.OAuth;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
                OAuth microsoftLogin = new MicrosoftLogin();
            Console.WriteLine( microsoftLogin.GetToken(microsoftLogin.Login()));
        }
    }
}
