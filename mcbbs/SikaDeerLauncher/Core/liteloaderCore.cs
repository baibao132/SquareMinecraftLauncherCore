using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SikaDeerLauncher.Minecraft;
namespace SikaDeerLauncher.Core
{
    internal class LiteloaderCore
    {
        Tools tools = new Tools();
        SikaDeerLauncherCore SLC = new SikaDeerLauncherCore();
        MinecraftDownload Minecraft = new MinecraftDownload();
        internal string LiteloaderJson(string version)
        {
            if (tools.LiteloaderExist(version) == true)
            {
                throw new SikaDeerLauncherException("已经安装过了，无需再次安装");
            }
            AllTheExistingVersion[] all = tools.GetAllTheExistingVersion();
            string mcversion = null;
            foreach (var s in all)
            {
                if (s.version == version)
                {
                    mcversion = s.IdVersion;
                }
            }
            LiteloaderList[] liteloaderLists = tools.GetLiteloaderList();
            LiteloaderList liteloaderList = new LiteloaderList();
            foreach (var ap in liteloaderLists)
            {
                if (ap.mcversion == mcversion)
                {
                    liteloaderList = ap;
                    break;
                }
                else if(ap.mcversion == liteloaderLists[liteloaderLists.Length-1].mcversion)
                {
                    throw new SikaDeerLauncherException("该版本不支持安装");
                }
            }
            string FileText = SLC.GetFile(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version+ @"\" + version + ".json");
            //var a = JsonConvert.DeserializeObject<Forge.ForgeJson.Root>(FileText);
            var b = JsonConvert.DeserializeObject<Forge.ForgeY.Root>(FileText);
            string arg = null;
            //if (a.arguments != null)
            //{
            //    arg += "{\"arguments\": {\"game\": [";
            //    for (int i = 0; a.arguments.game.ToArray().Length - 1 > 0; i++)
            //    {
            //        arg += "\"" + a.arguments.game[i] + "\",";
            //    }
            //}
            //else
            //{
                arg += "{";


            //}
            arg += liteloaderJsonY(b,liteloaderList,version);
            var p = JsonConvert.DeserializeObject<Forge.ForgeJsonEarly.Root>(FileText);
            arg += ",\"minecraftArguments\": \"" + p.minecraftArguments+" --tweakClass "+liteloaderList.tweakClass + "\"}";
            return arg;
        }
        public string liteloaderJsonY(Forge.ForgeY.Root versionText, LiteloaderList libraries,string version)
        {
            string arg = "\"assetIndex\": {\"id\": \"" + versionText.assetIndex.id + "\",\"size\":" + versionText.assetIndex.size + ",\"url\": \"" + versionText.assetIndex.url + "\"},\"assets\": \"" + versionText.assets + "\",\"downloads\": {\"client\": {\"url\":\"" + versionText.downloads.client.url + "\"}},\"id\": \"" + versionText.id + "\",\"libraries\": [";
            foreach (var a in libraries.lib)
            {
                Forge.ForgeY.LibrariesItem item = new Forge.ForgeY.LibrariesItem();
                item.name = a.name;
                Forge.ForgeY.Downloads down = new Forge.ForgeY.Downloads();
                down.artifact = new Forge.ForgeY.Artifact();
                down.artifact.url = " ";
                item.downloads = down;
                versionText.libraries.Add(item);
            }
            Forge.ForgeY.LibrariesItem item1 = new Forge.ForgeY.LibrariesItem();
            item1.name = "com.mumfrey:liteloader:"+libraries.version;
            Forge.ForgeY.Downloads down1 = new Forge.ForgeY.Downloads();
            down1.artifact = new Forge.ForgeY.Artifact();
            down1.artifact.url = Minecraft.DownloadLiteloader(version).Url;
            item1.downloads = down1;
            versionText.libraries.Add(item1);
            for (int i = 0; versionText.libraries.ToArray().Length > i; i++)
            {
                arg += "{\"name\":\"" + versionText.libraries[i].name + "\",";
                if (versionText.libraries[i].downloads == null || versionText.libraries[i].downloads.artifact == null)
                {
                    arg = arg.Substring(0, arg.Length - 1);
                }
                else
                {
                    arg += "\"downloads\":{\"artifact\":{\"url\":\"" + versionText.libraries[i].downloads.artifact.url + "\"}}";
                }
                if (versionText.libraries[i].natives != null)
                {
                    arg += ",\"natives\": {";
                    string natives = null;
                    if (versionText.libraries[i].natives.linux != null)
                    {
                        if (natives != null)
                        {
                            natives += ",";
                        }
                        natives += "\"linux\": \"natives - linux\"";
                    }
                    if (versionText.libraries[i].natives.osx != null)
                    {
                        if (natives != null)
                        {
                            natives += ",";
                        }
                        natives += "\"osx\": \"natives - osx\"";
                    }
                    if (versionText.libraries[i].natives.windows != null)
                    {
                        if (natives != null)
                        {
                            natives += ",";
                        }
                        natives += "\"windows\": \"" + versionText.libraries[i].natives.windows + "\"";
                    }
                    arg += natives + "}},";
                }
                else
                {
                    arg += "},";
                }
                if (i == versionText.libraries.ToArray().Length - 1)
                {
                    char[] ca = arg.ToCharArray();
                    ca[ca.Length - 1] = ']';
                    arg = null;
                    foreach (var mychar in ca)
                    {
                        arg += mychar;
                    }
                }
            }
            arg += ",\"mainClass\": \"" + versionText.mainClass + "\"";
            return arg;
        }
    }
}
