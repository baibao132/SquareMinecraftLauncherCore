using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SikaDeerLauncher.Minecraft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikaDeerLauncher.Core
{
    internal class OptifineCore
    {
        Tools tools = new Tools();
        SikaDeerLauncherCore SLC = new SikaDeerLauncherCore();
        MinecraftDownload Minecraft = new MinecraftDownload();
        internal string OptifineJson(string version,OptiFineList optiFineList)
        {
            if (tools.OptifineExist(version))
            {
                throw new SikaDeerLauncherException("已经安装过了,无需再次安装");
            }
            string FileText = SLC.GetFile(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json");
            var a = JsonConvert.DeserializeObject<Forge.ForgeJsonEarly.Root>(FileText);
            var b = JsonConvert.DeserializeObject<Forge.ForgeY.Root>(FileText);
            string arg = null;
            if (a.minecraftArguments == null)
            {
                var c = (JObject)JsonConvert.DeserializeObject(FileText);
                arg += "{\"arguments\": {\"game\": [";
                for (int i = 0; c["arguments"]["game"].ToArray().Length - 1 > 0; i++)
                {
                    try
                    {

                        c["arguments"]["game"][i].ToString();
                        if (c["arguments"]["game"][i].ToString()[0] == '-' || c["arguments"]["game"][i].ToString()[0] == '$')
                        {
                            arg += "\"" + c["arguments"]["game"][i].ToString() + "\",";
                        }
                        else
                        {
                            if (c["arguments"]["game"][i - 1].ToString()[0] == '-')
                            {
                                arg += "\"" + c["arguments"]["game"][i].ToString() + "\",";
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
                try
                {
                    tools.GetCompareForgeVersions(version);
                    arg += "\"--tweakClass\",";
                    arg += "\"optifine.OptiFineForgeTweaker\"]},";
                }
                catch(SikaDeerLauncherException ex)
                {
                    arg += "\"--tweakClass\",";
                    arg += "\"optifine.OptiFineTweaker\"]},";
                }
                arg += liteloaderJsonY(b, optiFineList.type, optiFineList.patch, version, optiFineList.filename);
                arg += "}";
            }
            else
            {
                arg += "{";
                arg += liteloaderJsonY(b,optiFineList.type,optiFineList.patch,version,optiFineList.filename);
                var p = JsonConvert.DeserializeObject<Forge.ForgeJsonEarly.Root>(FileText);
                try
                {
                    tools.GetCompareForgeVersions(version);
                    arg += ",\"minecraftArguments\": \"" + p.minecraftArguments + " --tweakClass optifine.OptiFineForgeTweaker\"}";
                }
                catch(SikaDeerLauncherException ex)
                {
                    arg += ",\"minecraftArguments\": \"" + p.minecraftArguments + " --tweakClass optifine.OptiFineTweaker\"}";
                }
            }
            return arg;
        }
            internal string liteloaderJsonY(Forge.ForgeY.Root versionText, string type, string patch,string version,string filename)
            {
                string arg = "\"assetIndex\": {\"id\": \"" + versionText.assetIndex.id + "\",\"size\":" + versionText.assetIndex.size + ",\"url\": \"" + versionText.assetIndex.url + "\"},\"assets\": \"" + versionText.assets + "\",\"downloads\": {\"client\": {\"url\":\"" + versionText.downloads.client.url + "\"}},\"id\": \"" + versionText.id + "\",\"libraries\": [";
                Forge.ForgeY.LibrariesItem item1 = new Forge.ForgeY.LibrariesItem();
                item1.name = "optifine:OptiFine:" + versionText.id + "_"+type+"_"+patch;
                Forge.ForgeY.Artifact artifact = new Forge.ForgeY.Artifact();
                Forge.ForgeY.Downloads down = new Forge.ForgeY.Downloads();
                artifact.url = Minecraft.DownloadOptifine(version,filename).Url;
                down.artifact = artifact;
                item1.downloads = down;
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
