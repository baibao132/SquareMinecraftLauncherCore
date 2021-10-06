using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SquareMinecraftLauncher.Core.OAuth
{
    public class MicrosoftLogin
    {
        Download web = new Download();
        /// <summary>
        /// 取微软Token
        /// </summary>
        /// <param name="code">通过microsoftLogin.Login获取</param>
        /// <returns></returns>
        public Token GetToken(string code)
        {
            string json = web.Post("https://login.live.com/oauth20_token.srf", "client_id=00000000402b5328&code= " + code + " &grant_type=authorization_code&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf", "application/x-www-form-urlencoded");
            var jsonConvert = JsonConvert.DeserializeObject<MicrosoftToken.Root>(json);
            Token token = new Token();
            token.access_token = jsonConvert.access_token;
            token.refresh_token = jsonConvert.refresh_token;
            return token;
        }
        /// <summary>
        /// 重新获取access_token
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public string RefreshingTokens(string refresh_token)
        {
            string json = web.Post("https://login.live.com/oauth20_token.srf", "client_id=00000000402b5328&refresh_token=" + refresh_token + "&grant_type=refresh_token&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf", "application/x-www-form-urlencoded");
            var jsonConvert = JsonConvert.DeserializeObject<MicrosoftToken.Root>(json);
            return jsonConvert.access_token;
        }
        /// <summary>
        /// 微软登录
        /// </summary>
        /// <returns></returns>
        public string Login()
        {
            Thread thd = new Thread(new ThreadStart(MicrosoftLoginFroms.start.Main));
            thd.SetApartmentState(ApartmentState.STA);
            thd.IsBackground = true;
            thd.Start();
            while(MicrosoftLoginFroms.Form1.url == "")
            {
                Thread.Sleep(5000);
            }
            return Regex.Split(MicrosoftLoginFroms.Form1.url, "code=")[1].Split('&')[0];
        }
    }
}
