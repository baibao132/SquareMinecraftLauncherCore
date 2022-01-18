using mcbbs;
using NSoup;
using SquareMinecraftLauncher.Core.Curseforge;
using SquareMinecraftLauncher.Minecraft;
using System;
using System.Collections.Generic;

namespace SquareMinecraftLauncher.Core
{
    internal class wiki
    {
        mcbbsnews mcbbsnews = new mcbbsnews();
        Download web = new Download();
        public string Search(string search)
        {
            string url = "https://search.mcmod.cn/s?key=" + search.Split('(')[0] + "&site=&filter=1&mold=0";
            try
            {
                NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(web.getHtml(url));
                var List = doc.GetElementsByClass("search-result-list");
                var item = NSoup.NSoupClient.Parse(List.ToString()).GetElementsByClass("result-item");
                foreach (var i in item)
                {
                    var j = NSoup.NSoupClient.Parse(i.GetElementsByClass("head").ToString());
                    var a = j.GetElementsByTag("a");
                    var title = a.Text.Replace("<em>", "").Replace("</em>", "").Replace("\"", "").Replace(" ", "");
                    if (title.IndexOf(search.Split('(')[0].Replace(" ", "")) > 0) return a[1].Attr("href").ToString();

                }
            }
            catch (Exception ex) { }
            return null;
        }

        public WiKiInformation SearchInformation(string url)
        {
            if (url == null) return null;
            WiKiInformation wiKiInformation = new WiKiInformation();
            var doc = NSoup.NSoupClient.Parse(web.getHtml(url));
            var class_text = NSoup.NSoupClient.Parse(doc.GetElementsByClass("class-text").ToString());
            var class_title = NSoupClient.Parse(class_text.ToString());
            wiKiInformation.Title = class_title.GetElementsByTag("h3").Text;
            NSoup.Select.Elements class_info = class_text.GetElementsByClass("class-info-left"); ;
            var link = NSoupClient.Parse(class_info.ToString()).GetElementsByClass("list");
            List<ExternalLinkItem> list = new List<ExternalLinkItem>();
            foreach (var item in NSoupClient.Parse(link.ToString()).GetElementsByTag("li"))
            {
                var item_link = NSoupClient.Parse(item.ToString()).GetElementsByTag("script").ToString();
                string Link = item_link.Replace("{title:\"这是一个站外链接\",content:\"<p>此链接会跳转到:</p><p><strong>", "|").Replace("</strong>", "|").Split('|')[1];
                string Title = NSoupClient.Parse(item.ToString()).GetElementsByTag("span").Attr("title").ToString();
                list.Add(new ExternalLinkItem { url = Link, Title = Title });
            }
            wiKiInformation.ExternalLinks = list.ToArray();
            var mcver = NSoupClient.Parse(NSoupClient.Parse(class_info.ToString()).GetElementsByTag("li").ToString()).GetElementsByTag("ul");
            List<string> version = new List<string>();
            foreach (var i in mcver)
            {
                var mc = NSoupClient.Parse(i.GetElementsByClass("text-danger").ToString()).GetElementsByTag("a");
                bool tf = false;
                foreach (var j in mc)
                {
                    if (!string.IsNullOrEmpty(j.Text()))
                    {
                        version.Add(j.Text());
                        tf = true;
                    }
                }
                if (tf) break;
            }
            wiKiInformation.SupportVersion = version.ToArray();
            var text_area = NSoupClient.Parse(NSoupClient.Parse(NSoupClient.Parse(NSoupClient.Parse(doc.GetElementsByClass("center").ToString()).GetElementsByTag("ul").ToString()).GetElementsByTag("li").ToString()).GetElementsByClass("common-text").ToString()).GetElementsByTag("p");
            string text = "";
            List<string> Img = new List<string>();
            foreach (var i in text_area)
            {
                text += i.Text() + "\n";
                var imgUrl = i.GetElementsByTag("Img").Attr("src").ToString();
                if (!string.IsNullOrEmpty(imgUrl)) Img.Add("https://" + imgUrl);
            }
            wiKiInformation.introduce = text;
            wiKiInformation.ImgUrl = Img.ToArray();
            var class_relation_list = doc.GetElementsByClass("class-relation-list");
            try
            {
                var relation = NSoupClient.Parse(NSoupClient.Parse(class_relation_list[0].ToString()).GetElementsByClass("relation")[0].ToString()).GetElementsByTag("li");

                string test = relation.ToString();
                if (test.IndexOf("前置") < 0)
                {
                    wiKiInformation.PreMod = null;
                    return wiKiInformation;
                }
                List<PreModItem> relation_list = new List<PreModItem>();
                for (var i = 1; i < relation.Count; i++)
                {
                    var mod = relation[i].GetElementsByTag("a");
                    var modtitle = mod.Attr("data-original-title").ToString();
                    var wikiLink = "https://www.mcmod.cn/" + mod.Attr("href").ToString();
                    relation_list.Add(new PreModItem { ModName = modtitle, WikiUrl = wikiLink });
                }
                wiKiInformation.PreMod = relation_list.ToArray();
            }
            catch (Exception ex) { }
            return wiKiInformation;
        }
    }
}
