namespace SquareMinecraftLauncher.Core
{
    using global::SquareMinecraftLauncher.Core.Forge;
    using global::SquareMinecraftLauncher.Minecraft;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Linq;

    internal class ForgeCore
    {
        private MinecraftDownload download = new MinecraftDownload();
        private SquareMinecraftLauncher.Download Download = new SquareMinecraftLauncher.Download();
        private SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        private SquareMinecraftLauncher.Minecraft.Tools Tools = new SquareMinecraftLauncher.Minecraft.Tools();

        public string ForgeJson(string version, string ForgePath)
        {
            string file = null;
            AllTheExistingVersion[] allTheExistingVersion = this.Tools.GetAllTheExistingVersion();
            foreach (AllTheExistingVersion version2 in allTheExistingVersion)
            {
                if (version2.version == version)
                {
                    string idVersion = version2.IdVersion;
                    break;
                }
                if (version2 == allTheExistingVersion[allTheExistingVersion.Length - 1])
                {
                    throw new SquareMinecraftLauncherException("未找到该版本");
                }
            }
            if (this.Tools.ForgeExist(version))
            {
                this.Tools.UninstallTheExpansionPack(ExpansionPack.Forge, version);
            }
            file = this.SLC.GetFile(Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json");
            ForgeJsonEarly.Root root = JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(file);
            ForgeY.Root versionText = JsonConvert.DeserializeObject<ForgeY.Root>(file);
            string str2 = null;
            if (root.minecraftArguments != null)
            {
                ForgeJsonEarly.Root root3 = JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(this.SLC.GetFile(ForgePath));
                str2 = str2 + "{" + this.ForgeJsonY(versionText, JsonConvert.DeserializeObject<ForgeY.Root>(this.SLC.GetFile(ForgePath)));
                if (this.Tools.OptifineExist(version))
                {
                    root3.minecraftArguments = root3.minecraftArguments + " --tweakClass optifine.OptiFineForgeTweaker";
                }
                if (this.Tools.LiteloaderExist(version))
                {
                    root3.minecraftArguments = root3.minecraftArguments + " --tweakClass com.mumfrey.liteloader.launch.LiteLoaderTweaker";
                }
                return (str2 + ",\"minecraftArguments\": \"" + root3.minecraftArguments + "\"}");
            }
            JObject obj2 = (JObject) JsonConvert.DeserializeObject(file);
            JObject obj3 = (JObject) JsonConvert.DeserializeObject(this.SLC.GetFile(ForgePath));
            str2 = str2 + "{\"arguments\": {\"game\": [";
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
            if (this.Tools.OptifineExist(version))
            {
                str2 = str2 + "\"--tweakClass\",\"optifine.OptiFineForgeTweaker\",";
            }
            if (this.Tools.LiteloaderExist(version))
            {
                str2 = str2 + " \"--tweakClass\",\"com.mumfrey.liteloader.launch.LiteLoaderTweaker\",";
            }
            for (int j = 0; (obj3["arguments"]["game"].ToArray().Length - 1) > 0; j++)
            {
                try
                {
                    obj3["arguments"]["game"][j].ToString();
                    str2 = str2 + "\"" + obj3["arguments"]["game"][j].ToString() + "\",";
                }
                catch (Exception)
                {
                    str2 = str2.Substring(0, str2.Length - 1) + "]},";
                    break;
                }
            }
            return (str2 + this.ForgeJsonY(versionText, JsonConvert.DeserializeObject<ForgeY.Root>(this.SLC.GetFile(ForgePath))) + "}");
        }

        public string ForgeJsonY(ForgeY.Root versionText, ForgeY.Root ForgeText)
        {
            string[] textArray1 = new string[] { "\"assetIndex\": {\"id\": \"", versionText.assetIndex.id, "\",\"size\":", versionText.assetIndex.size, ",\"url\": \"", versionText.assetIndex.url, "\"},\"assets\": \"", versionText.assets, "\",\"downloads\": {\"client\": {\"url\":\"", versionText.downloads.client.url, "\"}},\"id\": \"", versionText.id, "\",\"libraries\": [" };
            string str = string.Concat(textArray1);
            foreach (ForgeY.LibrariesItem item in ForgeText.libraries)
            {
                if (item.downloads == null)
                {
                    ForgeY.Downloads downloads = new ForgeY.Downloads();
                    ForgeY.Artifact artifact = new ForgeY.Artifact();
                    downloads.artifact = artifact;
                    item.downloads = downloads;
                }
                else if (item.downloads.artifact == null)
                {
                    ForgeY.Artifact artifact2 = new ForgeY.Artifact();
                    item.downloads.artifact = artifact2;
                }
                if (item.downloads.artifact.url.IndexOf("files.minecraftforge.net") < 0)
                {
                    item.downloads.artifact.url = "";
                    versionText.libraries.Add(item);
                    continue;
                }
                item.downloads.artifact.url = "http://files.minecraftforge.net/maven/";
                versionText.libraries.Add(item);
            }
            for (int i = 0; versionText.libraries.ToArray().Length > i; i++)
            {
                str = str + "{\"name\":\"" + versionText.libraries[i].name + "\",";
                if (((versionText.libraries[i].downloads == null) || (versionText.libraries[i].downloads.artifact == null)) && (versionText.libraries[i].url == null))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                else if ((versionText.libraries[i].downloads != null) && (versionText.libraries[i].downloads.artifact != null))
                {
                    str = str + "\"downloads\":{\"artifact\":{\"url\":\"" + versionText.libraries[i].downloads.artifact.url + "\"}}";
                }
                else if (versionText.libraries[i].url != null)
                {
                    str = str + "\"downloads\":{\"artifact\":{\"url\":\"" + versionText.libraries[i].url + "\"}}";
                }
                if (versionText.libraries[i].natives != null)
                {
                    str = str + ",\"natives\": {";
                    string str2 = null;
                    if (versionText.libraries[i].natives.linux != null)
                    {
                        if (str2 != null)
                        {
                            str2 = str2 + ",";
                        }
                        str2 = str2 + "\"linux\": \"natives - linux\"";
                    }
                    if (versionText.libraries[i].natives.osx != null)
                    {
                        if (str2 != null)
                        {
                            str2 = str2 + ",";
                        }
                        str2 = str2 + "\"osx\": \"natives - osx\"";
                    }
                    if (versionText.libraries[i].natives.windows != null)
                    {
                        if (str2 != null)
                        {
                            str2 = str2 + ",";
                        }
                        str2 = str2 + "\"windows\": \"" + versionText.libraries[i].natives.windows + "\"";
                    }
                    str = str + str2 + "}},";
                }
                else
                {
                    str = str + "},";
                }
                if (i == (versionText.libraries.ToArray().Length - 1))
                {
                    char[] chArray1 = str.ToCharArray();
                    chArray1[chArray1.Length - 1] = ']';
                    str = null;
                    foreach (char ch in chArray1)
                    {
                        str = str + ch.ToString();
                    }
                }
            }
            return (str + ",\"mainClass\": \"" + ForgeText.mainClass + "\"");
        }

        internal string ForgeKeep(string FileText, ForgeY.Root ForgePath, string versionjson, string version)
        {
            ForgeJsonEarly.Root root = JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(FileText);
            JsonConvert.DeserializeObject<ForgeY.Root>(FileText);
            string str = null;
            if (root.minecraftArguments != null)
            {
                JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(versionjson);
                str = str + "{";
                this.SLC.wj(Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json", versionjson);
                str = str + this.ForgeJsonY(JsonConvert.DeserializeObject<ForgeY.Root>(versionjson), ForgePath);
                char[] separator = new char[] { ' ' };
                string[] strArray = root.minecraftArguments.Split(separator);
                string str2 = "";
                for (int j = 1; strArray.Length > j; j += 2)
                {
                    if ((((strArray[j - 1][0] == '-') && (strArray[j] != "com.mumfrey.liteloader.launch.LiteLoaderTweaker")) && (strArray[j] != "optifine.OptiFineForgeTweaker")) || (strArray[j][0] == '$'))
                    {
                        if (j != (strArray.Length - 1))
                        {
                            string[] textArray4 = new string[] { str2, strArray[j - 1], " ", strArray[j], " " };
                            str2 = string.Concat(textArray4);
                        }
                        else
                        {
                            str2 = str2 + strArray[j - 1] + " " + strArray[j];
                        }
                    }
                }
                if (str2[str2.Length - 1] == ' ')
                {
                    str2 = str2.Substring(0, str2.Length - 1);
                }
                return (str + ",\"minecraftArguments\": \"" + str2 + "\"}");
            }
            JObject obj2 = (JObject) JsonConvert.DeserializeObject(FileText);
            str = str + "{\"arguments\": {\"game\": [";
            for (int i = 1; (obj2["arguments"]["game"].ToArray<JToken>().Length - 1) > 0; i += 2)
            {
                try
                {
                    obj2["arguments"]["game"][i].ToString();
                    if ((((obj2["arguments"]["game"][i - 1].ToString()[0] == '-') && (obj2["arguments"]["game"][i].ToString() != "com.mumfrey.liteloader.launch.LiteLoaderTweaker")) && (obj2["arguments"]["game"][i].ToString() != "optifine.OptiFineForgeTweaker")) || (obj2["arguments"]["game"][i - 1].ToString()[0] == '$'))
                    {
                        string[] textArray1 = new string[] { str, "\"", obj2["arguments"]["game"][i - 1].ToString(), "\",\"", obj2["arguments"]["game"][i].ToString(), "\"," };
                        str = string.Concat(textArray1);
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            str = str.Substring(0, str.Length - 1) + "]},";
            this.SLC.wj(Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json", versionjson);
            return (str + this.ForgeJsonY(JsonConvert.DeserializeObject<ForgeY.Root>(versionjson), ForgePath) + "}");
        }
    }
}

