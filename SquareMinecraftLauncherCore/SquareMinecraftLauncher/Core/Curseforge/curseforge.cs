using Newtonsoft.Json;
using SquareMinecraftLauncher.Minecraft;
using System.Collections.Generic;

namespace SquareMinecraftLauncher.Core.Curseforge
{
    internal class Curseforge
    {
        private string url = "https://addons-ecs.forgesvc.net";
        Download web = new Download();
        wiki wiki = new wiki();
        public CurseForgeItem[] Search(string name, int id, category category)
        {
            int categoryId = 0;
            if (category != null) categoryId = category.id;
            string json = web.getHtml(url + string.Format("/api/v2/addon/search?categoryId={2}&gameId=432&index=0&pageSize=20&searchFilter={0}&sectionId={1}&sort=0", name, id, categoryId));
            var obj = JsonConvert.DeserializeObject<List<CurseForgeItem>>(json);
            List<WiKiInformation> list = new List<WiKiInformation>();
            for (int i = 0; i < obj.Count; i++)
            {
                list.Add(wiki.SearchInformation(wiki.Search(obj[i].name)));
                obj[i].name = new youdao().GetChinese(obj[i].name);
                obj[i].summary = new youdao().GetChinese(obj[i].summary);
            }
            return bulid(list, obj).ToArray();
        }

        public CurseForgeItem[] popular(int id, category category)
        {
            int categoryId = 0;
            if (category != null) categoryId = category.id;
            string json = web.getHtml(url + string.Format("/api/v2/addon/search?categoryId={1}&gameId=432&index=0&pageSize=20&sectionId={0}&sort=0", id, categoryId));
            var obj = JsonConvert.DeserializeObject<List<CurseForgeItem>>(json);
            List<WiKiInformation> list = new List<WiKiInformation>();
            for (int i = 0; i < obj.Count; i++)
            {
                list.Add(wiki.SearchInformation(wiki.Search(obj[i].name)));
                obj[i].name = new youdao().GetChinese(obj[i].name);
                obj[i].summary = new youdao().GetChinese(obj[i].summary);
            }
            return bulid(list, obj).ToArray();
        }

        public string download(int id, string file)
        {
            return string.Format("http://mirror.baibaoblog.cn:88/file/{0}/{1}", id, file);
        }

        internal List<CurseForgeItem> bulid(List<WiKiInformation> wiKis, List<CurseForgeItem> curseForgeItems)
        {
            for (int i = 0; i < wiKis.Count; i++)
            {
                if (wiKis[i] == null) continue;
                if (!string.IsNullOrEmpty(wiKis[i].Title)) curseForgeItems[i].name = wiKis[i].Title;
                if (!string.IsNullOrEmpty(wiKis[i].introduce)) curseForgeItems[i].summary = wiKis[i].introduce;
                if (wiKis[i].ExternalLinks != null) curseForgeItems[i].ExternalLinks = wiKis[i].ExternalLinks;
                if (wiKis[i].PreMod != null) curseForgeItems[i].PreMod = wiKis[i].PreMod;
            }
            return curseForgeItems;
        }
    }
}
