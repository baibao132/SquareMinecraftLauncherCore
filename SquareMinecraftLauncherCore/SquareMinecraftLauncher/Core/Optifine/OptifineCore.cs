namespace SquareMinecraftLauncher.Core
{
    using global::SquareMinecraftLauncher.Core.Forge;
    using global::SquareMinecraftLauncher.Minecraft;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SquareMinecraftLauncher;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal class OptifineCore
    {
        private MinecraftDownload Minecraft = new MinecraftDownload();
        private SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        private Tools tools = new Tools();

        internal async Task<string> liteloaderJsonY(ForgeY.Root versionText, string type, string patch, string version, string filename, string javaPath)
        {
            string[] textArray1 = new string[] { "\"assetIndex\": {\"id\": \"", versionText.assetIndex.id, "\",\"size\":", versionText.assetIndex.size, ",\"url\": \"", versionText.assetIndex.url, "\"},\"assets\": \"", versionText.assets, "\",\"downloads\": {\"client\": {\"url\":\"", versionText.downloads.client.url, "\"}},\"id\": \"", versionText.id, "\",\"libraries\": [" };
            string str = string.Concat(textArray1);

            ForgeY.Artifact artifact = new ForgeY.Artifact();
            ForgeY.Downloads downloads = new ForgeY.Downloads();
            var DO = Minecraft.DownloadOptifine(version, filename);
            ForgeY.LibrariesItem item = new ForgeY.LibrariesItem();
            item.name = DO.name;
            artifact.url = DO.Url;
            downloads.artifact = artifact;
            item.downloads = downloads;
            versionText.libraries.Add(item);
            ForgeDownload forgeDownload = new ForgeDownload(new MCDownload[] { new MCDownload() { Url = DO.Url, path = DO.path } });
            forgeDownload.StartDownload();
            await Task.Run(() =>
            {
                while (!forgeDownload.GetEndDownload())
                {
                    Thread.Sleep(2000);
                }
            });

            if (forgeDownload.error != 0) throw new SquareMinecraftLauncherException("安装失败，下载optifine失败");
            Unzip unzip = new Unzip();
            string err;
            unzip.UnZipFile(DO.path, System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\OptiFine\", out err);
            if (File.Exists(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\OptiFine\launchwrapper-of.txt"))
            {
                string launch = File.ReadAllText(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\OptiFine\launchwrapper-of.txt");
                SLC.path(System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\optifine\launchwrapper-of\" + launch);
                File.Copy(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\OptiFine\launchwrapper-of-" + launch + ".jar", System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\optifine\launchwrapper-of\" + launch + @"\launchwrapper-of-" + launch + ".jar");
                ForgeY.LibrariesItem item2 = new ForgeY.LibrariesItem();
                ForgeY.Artifact artifact2 = new ForgeY.Artifact();
                ForgeY.Downloads downloads2 = new ForgeY.Downloads();
                item2.name = "optifine:launchwrapper-of:" + launch;
                artifact2.url = DO.Url;
                downloads2.artifact = artifact2;
                item2.downloads = downloads2;
                versionText.libraries.Add(item2);
            }
            else
            {
                if (!tools.ForgeExist(version))
                {
                    ForgeY.LibrariesItem item2 = new ForgeY.LibrariesItem();
                    ForgeY.Artifact artifact2 = new ForgeY.Artifact();
                    ForgeY.Downloads downloads2 = new ForgeY.Downloads();
                    item2.name = "net.minecraft:launchwrapper:1.12";
                    artifact2.url = "http://files.minecraftforge.net/maven/";
                    downloads2.artifact = artifact2;
                    item2.downloads = downloads2;
                    versionText.libraries.Add(item2);
                }
            }
            if (File.Exists(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\OptiFine\optifine\Patcher.class"))
            {
                SLC.path(System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\optifine\OptiFine\" + DO.name.Split(':')[2]);
                string arg = "-cp \"" + DO.path + "\" optifine.Patcher " + System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar " + DO.path + " " + System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\optifine\OptiFine\" + DO.name.Split(':')[2] + @"\" + DO.name.Split(':')[2] + ".jar";
                Process process = Process.Start(javaPath, arg);
                await Task.Run(() => { process.WaitForExit(); });
                process.Close();
            }
            else
            {
                throw new SquareMinecraftLauncherException("安装失败，OptiFine存在问题");
            }
            SLC.DelPathOrFile(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\OptiFine\");
            for (int i = 0; versionText.libraries.ToArray().Length > i; i++)
            {
                str = str + "{\"name\":\"" + versionText.libraries[i].name + "\",";
                if ((versionText.libraries[i].downloads == null) || (versionText.libraries[i].downloads.artifact == null))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                else
                {
                    str = str + "\"downloads\":{\"artifact\":{\"url\":\"" + versionText.libraries[i].downloads.artifact.url + "\"}}";
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
            return (str + ",\"mainClass\": \"net.minecraft.launchwrapper.Launch\"");
        }

        internal async Task<string> OptifineJson(string version, OptiFineList optiFineList, string javaPath)
        {
            if (this.tools.OptifineExist(version))
            {
                throw new SquareMinecraftLauncherException("已经安装过了,无需再次安装");
            }
            string file = this.SLC.GetFile(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json");
            ForgeJsonEarly.Root root = JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(file);
            ForgeY.Root versionText = JsonConvert.DeserializeObject<ForgeY.Root>(file);
            string str2 = null;
            if (root.minecraftArguments == null)
            {
                JObject obj2 = (JObject)JsonConvert.DeserializeObject(file);
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
                try
                {
                    await tools.GetCompareForgeVersions(version);
                    str2 = str2 + "\"--tweakClass\",";
                    str2 = str2 + "\"optifine.OptiFineForgeTweaker\"]},";
                }
                catch (SquareMinecraftLauncherException)
                {
                    str2 = str2 + "\"--tweakClass\"," + "\"optifine.OptiFineTweaker\"]},";
                }
                return (str2 + await liteloaderJsonY(versionText, optiFineList.type, optiFineList.patch, version, optiFineList.filename, javaPath) + "}");
            }
            str2 = str2 + "{" + await liteloaderJsonY(versionText, optiFineList.type, optiFineList.patch, version, optiFineList.filename, javaPath);
            ForgeJsonEarly.Root root3 = JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(file);
            try
            {
                await tools.GetCompareForgeVersions(version);
                return (str2 + ",\"minecraftArguments\": \"" + root3.minecraftArguments + " --tweakClass optifine.OptiFineForgeTweaker\"}");
            }
            catch (SquareMinecraftLauncherException)
            {
                return (str2 + ",\"minecraftArguments\": \"" + root3.minecraftArguments + " --tweakClass optifine.OptiFineTweaker\"}");
            }
        }
    }
}

