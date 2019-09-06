using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AI
{
    internal class Baidu
    {
        // 设置APPID/AK/SK
        string API_KEY = "xGeRsx3XiiOn7ZasdxlYLPOd";
        string SECRET_KEY = "pEprnMZHRf3tq8P4sFGUkDbeSWDRQrCr";
        // 合成
        static bool a = false;
        public void Tts()
        {
            try
            {
                if (a != false)
                {
                    return;
                }
                SikaDeerLauncher.Download web = new SikaDeerLauncher.Download();
                var html = web.getHtml("https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id=" + API_KEY + "&client_secret=" + SECRET_KEY);
                Console.WriteLine("https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id=" + API_KEY + "&client_secret=" + SECRET_KEY);
                if (html != null)
                {
                    var jo = JsonConvert.DeserializeObject<SikaDeerLauncher.BaiduAPIJson.TokenJson.Root>(html);
                    SikaDeerLauncher.Core.SikaDeerLauncherCore SLC = new SikaDeerLauncher.Core.SikaDeerLauncherCore();
                    var html1 = web.getHtml("http://tsn.baidu.com/text2audio?tex=%e5%b0%8f%e9%b9%bf%e5%90%af%e5%8a%a8%e5%99%a8&lan=zh&cuid="+SLC.token()+"&ctp=1&aue=3&tok=" + jo.access_token + "&per=111");
                    Console.WriteLine("http://tsn.baidu.com/text2audio?tex=%e5%b0%8f%e9%b9%bf%e5%90%af%e5%8a%a8%e5%99%a8&lan=zh&cuid=" + SLC.token() + "&ctp=1&aue=3&tok=" + jo.access_token + "&per=111");
                    var html2 = web.getHtml("https://blog.csdn.net/BaiBao132/article/details/97305270");
                    if (html1 != null && html2 != null)
                    {
                        a = true;
                        Console.WriteLine("调用完成");
                    }
                }
            }
            catch (Exception ex)
            {
                a = false;
            }
        }
    }
}