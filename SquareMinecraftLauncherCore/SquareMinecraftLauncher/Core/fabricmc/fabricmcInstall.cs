using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SquareMinecraftLauncher.Core.Forge;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SquareMinecraftLauncher.Core.fabricmc
{
    internal class fabricmcInstall
    {
        List<ForgeY.LibrariesItem> libItem = new List<ForgeY.LibrariesItem>();
        Download web = new Download();
        SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        public void GetLoaderVersionJson(string loaderVersion, string version, string Idversion)
        {
            var json = web.getHtml("https://maven.fabricmc.net/net/fabricmc/fabric-loader/" + loaderVersion + "/fabric-loader-" + loaderVersion + ".json");
            if (json != null)
            {
                var jo = JsonConvert.DeserializeObject<fabricmcJson.Root>(json);
                foreach (var i in jo.libraries.common)//����fabicmcLibraries
                {
                    ForgeY.LibrariesItem lib = new ForgeY.LibrariesItem();
                    lib.name = i.name;
                    if (i.url != null)
                    {
                        ForgeY.Downloads downloads = new ForgeY.Downloads();
                        ForgeY.Artifact Artifact = new ForgeY.Artifact();
                        downloads.artifact = Artifact;
                        if (i.url == "http://repo.maven.apache.org/maven2/")
                        {
                            i.url = "https://repo.maven.apache.org/maven2/";
                        }
                        Artifact.url = i.url;
                        lib.downloads = downloads;
                    }
                    libItem.Add(lib);
                }
                /*/����fabricmc����/*/
                ForgeY.LibrariesItem lib1 = new ForgeY.LibrariesItem();
                lib1.name = "net.fabricmc:fabric-loader:" + loaderVersion;
                ForgeY.Downloads downloads1 = new ForgeY.Downloads();
                ForgeY.Artifact Artifact1 = new ForgeY.Artifact();
                downloads1.artifact = Artifact1;
                Artifact1.url = "https://maven.fabricmc.net/";
                lib1.downloads = downloads1;
                libItem.Add(lib1);
                ForgeY.LibrariesItem lib2 = new ForgeY.LibrariesItem();
                lib2.name = "net.fabricmc:intermediary:" + Idversion;
                ForgeY.Downloads downloads2 = new ForgeY.Downloads();
                ForgeY.Artifact Artifact2 = new ForgeY.Artifact();
                downloads2.artifact = Artifact2;
                Artifact2.url = "https://maven.fabricmc.net/";
                lib2.downloads = downloads2;
                libItem.Add(lib2);
                /*/����/*/
                var json1 = SLC.GetFile(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json");
                var jo1 = JsonConvert.DeserializeObject<ForgeY.Root>(json1);
                foreach (var i in jo1.libraries)//���Ӱ汾libraries
                {
                    ForgeY.LibrariesItem lib = new ForgeY.LibrariesItem();
                    lib.name = i.name;
                    lib.downloads = i.downloads;
                    lib.url = i.url;
                    lib.natives = i.natives;
                    libItem.Add(lib);
                }
                string mainclass = jo.mainClass.client;//fabricmcMainClass
                string Arguments = ArgumentsJson(json1); //����µ�Arguments
                Arguments = Arguments.Substring(0, Arguments.Length - 1);
                SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json", new NewJson().newJson(libItem, Arguments, mainclass, jo1));
                return;
            }
            throw new SquareMinecraftLauncherException("����ʧ��");
        }
        internal string ArgumentsJson(string json)
        {
            var obj2 = (JObject)JsonConvert.DeserializeObject(json);
            string str2 = null;
            for (int i = 0; (obj2["arguments"]["game"].ToArray<JToken>().Length - 1) > 0; i++)
            {
                try
                {
                    obj2["arguments"]["game"][i].ToString();
                    if ((obj2["arguments"]["game"][i].ToString()[0] == '-') || (obj2["arguments"]["game"][i].ToString()[0] == '$'))
                    {
                        str2 = str2 + "\"" + obj2["arguments"]["game"][i].ToString() + "\",";
                        continue;
                    }
                    if (obj2["arguments"]["game"][i - 1].ToString()[0] == '-')
                    {
                        str2 = str2 + "\"" + obj2["arguments"]["game"][i].ToString() + "\",";
                        continue;
                    }
                }
                catch (Exception)
                {
                }
                break;
            }
            return str2;
        }
    }
}