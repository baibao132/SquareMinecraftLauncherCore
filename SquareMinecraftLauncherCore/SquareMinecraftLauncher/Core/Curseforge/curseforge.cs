using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SquareMinecraftLauncher.Minecraft;

namespace SquareMinecraftLauncher.Core.Curseforge
{
    internal class Curseforge
    {
        private string url = "https://addons-ecs.forgesvc.net";
        Download web = new Download();
        public List<CurseForgeItem> Search(string name,int id, category category)
        {
            int categoryId = 0;
            if (category != null) categoryId = category.id;
            string json = web.getHtml(url + string.Format("/api/v2/addon/search?categoryId={2}&gameId=432&index=0&pageSize=20&searchFilter={0}&sectionId={1}&sort=0", name, id,categoryId));
            var obj = JsonConvert.DeserializeObject<List<CurseForgeItem>>(json);
            for(int i = 0;i < obj.Count;i++)
            {
                obj[i].name = new youdao().GetChinese(obj[i].name);
                obj[i].summary = new youdao().GetChinese(obj[i].summary);
                for(int j = 0;j < obj[i].categories.Count;j++)
                {
                    obj[i].categories[j].name = new youdao().GetChinese(obj[i].categories[j].name);
                }
            }
            return obj;
        }

        public List<CurseForgeItem> popular(int id, category category)
        {
            int categoryId = 0;
            if (category != null) categoryId = category.id;
            string json = web.getHtml(url + string.Format("/api/v2/addon/search?categoryId={1}&gameId=432&index=0&pageSize=20&sectionId={0}&sort=0", id,categoryId));
            var obj = JsonConvert.DeserializeObject<List<CurseForgeItem>>(json);
            for (int i = 0; i < obj.Count; i++)
            {
                obj[i].name = new youdao().GetChinese(obj[i].name);
                obj[i].summary = new youdao().GetChinese(obj[i].summary);
                for (int j = 0; j < obj[i].categories.Count; j++)
                {
                    obj[i].categories[j].name = new youdao().GetChinese(obj[i].categories[j].name);
                }
            }
            return obj;
        }

        public string download(int id,string file)
        {
            return string.Format("http://mirror.baibaoblog.cn:88/file/{0}/{1}", id,file);
        }
    }
}
