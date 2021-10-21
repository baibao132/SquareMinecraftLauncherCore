using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SquareMinecraftLauncher.Core.OAuth;
using SquareMinecraftLauncher.Minecraft.MCServerPing;
using SquareMinecraftLauncher.Core.Curseforge;
using SquareMinecraftLauncher.Minecraft;
using SquareMinecraftLauncher.SquareMinecraftLauncher.Minecraft;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1Async()
        {
            ////之后的获取MinecraftToken方法
            //MicrosoftLogin microsoftLogin = new MicrosoftLogin();
            //Xbox XboxLogin = new Xbox();
            //var token = microsoftLogin.GetToken(microsoftLogin.Login(false));
            //string refresh_token = token.refresh_token;
            //string Minecraft_Token = new MinecraftLogin().GetToken(XboxLogin.XSTSLogin(XboxLogin.GetToken(token.access_token)));
            ////以上是第一次登录，下面是之后登录
            //Minecraft_Token = new MinecraftLogin().GetToken(XboxLogin.XSTSLogin(XboxLogin.GetToken(microsoftLogin.RefreshingTokens(refresh_token))));
            //Console.WriteLine(Minecraft_Token);
            //MinecraftServer.server("mssj.starmc.cn", 54520);

            CurseForgeInterface CurseForge = new ModCurseForge();
            CurseForge curse = new CurseForge();
            var obj = await curse.Getcategory();
            var item = await CurseForge.popular();
            foreach(var i in item)
            {
                Console.WriteLine(i.name + "   " + i.summary);
            }
            CurseForge.download(item[0].gameVersionLatestFiles[0]);
        }
    }
}
