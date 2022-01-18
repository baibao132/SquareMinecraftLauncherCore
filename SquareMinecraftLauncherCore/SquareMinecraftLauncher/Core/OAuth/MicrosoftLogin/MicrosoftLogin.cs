using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

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
            string json = web.Post("https://login.live.com/oauth20_token.srf", "client_id=00000000402b5328&refresh_token=" + refresh_token + "&grant_type=refresh_token&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf&scope=service::user.auth.xboxlive.com::MBI_SSL", "application/x-www-form-urlencoded");
            var jsonConvert = JsonConvert.DeserializeObject<MicrosoftToken.Root>(json);
            return jsonConvert.access_token;
        }
        /// <summary>
        /// 微软登录
        /// </summary>
        /// <returns></returns>
        public async Task<string> Login(bool AutoLogin)
        {
            await Task.Run(() =>
            {
                if (AutoLogin) MicrosoftLoginFroms.Form1.url1 = "https://login.live.com/oauth20_authorize.srf?client_id=00000000402b5328&response_type=code&scope=service%3A%3Auser.auth.xboxlive.com%3A%3AMBI_SSL&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf";
                else MicrosoftLoginFroms.Form1.url1 = "https://login.live.com/oauth20_authorize.srf?client_id=00000000402b5328&scope=service%3a%3auser.auth.xboxlive.com%3a%3aMBI_SSL&redirect_uri=https%3a%2f%2flogin.live.com%2foauth20_desktop.srf&response_type=code&prompt=login&uaid=057b3be0fc6a4324adfa39149843f54e&msproxy=1&issuer=mso&tenant=consumers&ui_locales=zh-CN#";
                Thread thd = new Thread(new ThreadStart(MicrosoftLoginFroms.start.Main));
                thd.SetApartmentState(ApartmentState.STA);
                thd.IsBackground = true;
                thd.Start();
                while (MicrosoftLoginFroms.Form1.url == "")
                {
                    if (MicrosoftLoginFroms.Form1.close) throw new SquareMinecraftLauncherException("用户取消登录");
                    Thread.Sleep(5000);
                }
            });
            return Regex.Split(MicrosoftLoginFroms.Form1.url, "code=")[1].Split('&')[0];
        }
    }
}
