﻿using Newtonsoft.Json;
using System;

namespace SquareMinecraftLauncher.Core.OAuth
{
    public class Xbox
    {
        Download web = new Download();
        /// <summary>
        /// 取Xbox登录Token
        /// </summary>
        /// <param name="code">通过microsoftLogin.GetToken函数获取</param>
        /// <returns></returns>
        public string GetToken(string code)
        {
            string json = web.Post("https://user.auth.xboxlive.com/user/authenticate", "{ \"Properties\": { \"AuthMethod\": \"RPS\", \"SiteName\": \"user.auth.xboxlive.com\", \"RpsTicket\": \"" + code + "\" }, \"RelyingParty\": \"http://auth.xboxlive.com\", \"TokenType\": \"JWT\" }");
            var jsonConvert = JsonConvert.DeserializeObject<XboxToken.Root>(json);
            return jsonConvert.Token;
        }
        /// <summary>
        /// Xbox取XSTS
        /// </summary>
        /// <param name="code">通过XboxLogin.GetToken函数获取</param>
        /// <returns></returns>
        public string[] XSTSLogin(string code)
        {
            string json = web.Post("https://xsts.auth.xboxlive.com/xsts/authorize", "{ \"Properties\": { \"SandboxId\": \"RETAIL\", \"UserTokens\": [ \"" + code + "\" ] }, \"RelyingParty\": \"rp://api.minecraftservices.com/\", \"TokenType\": \"JWT\" }");
            var jsonConvert = JsonConvert.DeserializeObject<XboxToken.Root>(json);
            string[] r = null;
            try
            {
                r = new string[] { jsonConvert.Token, jsonConvert.DisplayClaims.xui[0].uhs };
            }
            catch (Exception ex)
            {
                var jsonConvert1 = JsonConvert.DeserializeObject<XboxErr.Root>(json);
                throw new SquareMinecraftLauncherException(jsonConvert1.XErr.ToString());
            }
            return r;
        }
    }
}
