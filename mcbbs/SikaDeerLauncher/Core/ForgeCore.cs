using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SikaDeerLauncher.Core
{
    internal class ForgeCore
    {
        MinecraftDownload download = new MinecraftDownload();
        Download Download = new Download();
        SikaDeerLauncher.Minecraft.Tools Tools = new Minecraft.Tools();
        SikaDeerLauncherCore SLC = new SikaDeerLauncherCore();
        public string ForgeJson(string version,string ForgePath)
        {
            string FileText = null;
            string mc = null;
            Minecraft.AllTheExistingVersion[] all = Tools.GetAllTheExistingVersion();
            foreach (var s in all)
            {
                if (s.version == version)
                {
                    mc = s.IdVersion;
                    break;
                }
                else if (s == all[all.Length - 1])
                {
                    throw new SikaDeerLauncherException("未找到该版本");
                }
            }
            try
            {
                if (Tools.GetCompareForgeVersions(version))
                {
                    Tools.UninstallTheExpansionPack(Minecraft.ExpansionPack.Forge,version);
                    FileText = SLC.GetFile(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json");
                }
                else
                {
                    throw new SikaDeerLauncherException("无需更新");
                }
            }
            catch (SikaDeerLauncherException ex)
            {
                FileText = SLC.GetFile(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json");
            }
            var a = JsonConvert.DeserializeObject<Forge.ForgeJsonEarly.Root>(FileText);
            var b = JsonConvert.DeserializeObject<Forge.ForgeY.Root>(FileText);
            string arg = null;
            if (a.minecraftArguments == null)
            {
                var c = (JObject)JsonConvert.DeserializeObject(FileText);
                var p = (JObject)JsonConvert.DeserializeObject(SLC.GetFile(ForgePath));
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
                for (int i = 0; p["arguments"]["game"].ToArray().Length - 1 > 0; i++)
                {
                    try
                    {
                        p["arguments"]["game"][i].ToString();
                        arg += "\"" + p["arguments"]["game"][i].ToString() + "\",";
                    }
                    catch (Exception ex)
                    {
                        arg = arg.Substring(0, arg.Length - 1) + "]},";
                        break;
                    }
                }
                arg = SLC.Replace(arg, "optifine.OptiFineTweaker", "optifine.OptiFineForgeTweaker");
               arg += ForgeJsonY(b, JsonConvert.DeserializeObject<Forge.ForgeY.Root>(SLC.GetFile(ForgePath)));
               arg += "}";
            }
            else
            {
                var p = JsonConvert.DeserializeObject<Forge.ForgeJsonEarly.Root>(SLC.GetFile(ForgePath));
                arg += "{";
                arg += ForgeJsonY(b, JsonConvert.DeserializeObject<Forge.ForgeY.Root>(SLC.GetFile(ForgePath)));
                p.minecraftArguments = SLC.Replace(p.minecraftArguments, "optifine.OptiFineTweaker", "optifine.OptiFineForgeTweaker");
                arg += ",\"minecraftArguments\": \"" + p.minecraftArguments+"\"}";
            }
            return arg;
        }
        public string ForgeJsonY(Forge.ForgeY.Root versionText,Forge.ForgeY.Root ForgeText)
        {
            string arg = "\"assetIndex\": {\"id\": \""+versionText.assetIndex.id+ "\",\"size\":"+versionText.assetIndex.size+",\"url\": \"" + versionText.assetIndex.url+ "\"},\"assets\": \""+versionText.assets+ "\",\"downloads\": {\"client\": {\"url\":\""+versionText.downloads.client.url+"\"}},\"id\": \"" + versionText.id+ "\",\"libraries\": [";
            foreach (var a in ForgeText.libraries)
            {
                if (a.downloads == null)
                {
                    Forge.ForgeY.Downloads down = new Forge.ForgeY.Downloads();
                    Forge.ForgeY.Artifact artifact = new Forge.ForgeY.Artifact();
                    down.artifact = artifact;
                    a.downloads = down;
                }
                else if (a.downloads.artifact == null)
                {
                    Forge.ForgeY.Artifact artifact = new Forge.ForgeY.Artifact();
                    a.downloads.artifact = artifact;
                }
                a.downloads.artifact.url = "http://files.minecraftforge.net/maven/";
                versionText.libraries.Add(a);
            }
            for (int i = 0; versionText.libraries.ToArray().Length > i; i++)
            {
                arg += "{\"name\":\"" + versionText.libraries[i].name + "\",";
                if ((versionText.libraries[i].downloads == null || versionText.libraries[i].downloads.artifact == null) && versionText.libraries[i].url == null)
                {
                    arg = arg.Substring(0, arg.Length - 1);
                }
                else if (versionText.libraries[i].downloads != null && versionText.libraries[i].downloads.artifact != null)
                {
                    arg += "\"downloads\":{\"artifact\":{\"url\":\"" + versionText.libraries[i].downloads.artifact.url + "\"}}";
                }
                else if (versionText.libraries[i].url != null)
                {
                    arg += "\"downloads\":{\"artifact\":{\"url\":\"" + versionText.libraries[i].url + "\"}}";
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
                        natives += "\"windows\": \""+versionText.libraries[i].natives.windows+"\"";
                    }
                    arg += natives+"}},";
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
            arg += ",\"mainClass\": \""+ForgeText.mainClass+"\"";
            return arg;
        }
        internal string ForgeKeep(string FileText,Forge.ForgeY.Root ForgePath,string versionjson,string version)
        {
            var a = JsonConvert.DeserializeObject<Forge.ForgeJsonEarly.Root>(FileText);
            var b = JsonConvert.DeserializeObject<Forge.ForgeY.Root>(FileText);
            string arg = null;
            if (a.minecraftArguments == null)
            {
                var c = (JObject)JsonConvert.DeserializeObject(FileText);
                arg += "{\"arguments\": {\"game\": [";
                for (int i = 1; c["arguments"]["game"].ToArray().Length - 1 > 0; i += 2)
                {
                    try
                    {

                        c["arguments"]["game"][i].ToString();
                        if (c["arguments"]["game"][i - 1].ToString()[0] == '-' && (c["arguments"]["game"][i].ToString() != "com.mumfrey.liteloader.launch.LiteLoaderTweaker" && c["arguments"]["game"][i].ToString() != "optifine.OptiFineForgeTweaker") || c["arguments"]["game"][i - 1].ToString()[0] == '$')
                        {
                            arg += "\"" + c["arguments"]["game"][i - 1].ToString()+"\",\""+ c["arguments"]["game"][i].ToString() + "\",";
                        }

                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
                arg = arg.Substring(0, arg.Length - 1) + "]},";
                SLC.wj(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json", versionjson);
                arg += ForgeJsonY(JsonConvert.DeserializeObject<Forge.ForgeY.Root>(versionjson), ForgePath);
                arg += "}";
            }
            else
            {
                var p = JsonConvert.DeserializeObject<Forge.ForgeJsonEarly.Root>(versionjson);
                arg += "{";
                SLC.wj(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json", versionjson);
                arg += ForgeJsonY(JsonConvert.DeserializeObject<Forge.ForgeY.Root>(versionjson), ForgePath);
                var newjson = a.minecraftArguments.Split(' ');
                string marg = "";
                for (var i = 1; newjson.Length > i;i += 2)
                {
                    if (newjson[i - 1][0] == '-' && (newjson[i] != "com.mumfrey.liteloader.launch.LiteLoaderTweaker" && newjson[i] != "optifine.OptiFineForgeTweaker")|| newjson[i][0] == '$')
                    {
                        if (i != newjson.Length - 1)
                        {
                            marg += newjson[i - 1] + " " + newjson[i] + " ";
                        }
                        else
                        {
                            marg += newjson[i - 1] + " " + newjson[i];
                        }
                    }

                }
                if (marg[marg.Length - 1] == ' ')
                {
                    marg = marg.Substring(0, marg.Length - 1);
                }
                arg += ",\"minecraftArguments\": \"" + marg + "\"}";
            }
            return arg;
        }
    }
}
