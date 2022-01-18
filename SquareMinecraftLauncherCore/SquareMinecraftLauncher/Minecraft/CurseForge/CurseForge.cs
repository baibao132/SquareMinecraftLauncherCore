using mcbbs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SquareMinecraftLauncher.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Minecraft
{
    public class CurseForge
    {
        Download web = new Download();
        /// <summary>
        /// 取CurseForge支持的游戏版本
        /// </summary>
        /// <returns></returns>
        public async Task<string[]> GetVersion()
        {
            string text = null;
            await Task.Factory.StartNew(() =>
            {
                text = web.getHtml("https://addons-ecs.forgesvc.net/api/v2/minecraft/version");
            });
            if (text == null)
            {
                throw new SquareMinecraftLauncherException("请求失败");
            }
            List<string> list = new List<string>();
            foreach (JToken token in (JArray)JsonConvert.DeserializeObject(new mcbbsnews().TakeTheMiddle(text, "\"versions\":", "]}") + "]"))
            {
                list.Add(token["id"].ToString());
            }
            return list.ToArray();
        }
        youdao youdao = new youdao();
        /// <summary>
        /// 取CurseForge类别
        /// </summary>
        /// <returns></returns>
        public async Task<List<category>> Getcategory()
        {
            string json = null;
            await Task.Factory.StartNew(() =>
            {
                json = web.getHtml("https://addons-ecs.forgesvc.net/api/v2/category");
            });
            if (json == null)
            {
                throw new SquareMinecraftLauncherException("请求失败");
            }
            var obj = JsonConvert.DeserializeObject<List<category>>(json);
            for (int i = 0; i < obj.Count; i++)
            {
                obj[i].name = youdao.GetChinese(obj[i].name);
            }
            return obj;
        }
    }
}
