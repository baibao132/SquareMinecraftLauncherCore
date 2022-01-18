using Newtonsoft.Json;
using System;

namespace SquareMinecraftLauncher.Core.OAuth
{
    public class MinecraftLogin
    {
        Download web = new Download();
        /// <summary>
        /// 取微软登录Minecraft的access_token
        /// </summary>
        /// <param name="code">通过XboxLogin.XSTSLogin函数获取</param>
        /// <returns></returns>
        public string GetToken(string[] code)
        {
            string json = web.Post("https://api.minecraftservices.com/authentication/login_with_xbox", "{ \"identityToken\": \"XBL3.0 x=" + code[1] + ";" + code[0] + " \" }");
            if (json == "") throw new SquareMinecraftLauncherException("Minecraft登录异常");
            try
            {
                var j = JsonConvert.DeserializeObject<MinecraftLoginResult.Root>(json);
                return j.access_token;
            }
            catch (Exception ex)
            {
                var j = JsonConvert.DeserializeObject<MinecraftLoginErr.Root>(json);
                throw new SquareMinecraftLauncherException(j.error);
            }

        }

        public MinecraftLoginToken GetMincraftuuid(string token)
        {
            string json = web.Get("https://api.minecraftservices.com/minecraft/profile", "Authorization: Bearer " + token);
            if (json == "") throw new SquareMinecraftLauncherException("Minecraft登录异常");
            try
            {
                var j = JsonConvert.DeserializeObject<PossessionGame.Root>(json);
                MinecraftLoginToken minecraftLoginToken = new MinecraftLoginToken();
                minecraftLoginToken.name = j.name;
                minecraftLoginToken.SkinUrl = j.skins[0].url;
                minecraftLoginToken.uuid = j.id;
                return minecraftLoginToken;
            }
            catch (Exception ex)
            {
                throw new SquareMinecraftLauncherException("你未拥有该游戏");
            }
        }


    }
}
