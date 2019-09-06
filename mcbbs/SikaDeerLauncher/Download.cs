using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using mcbbs;
using SikaDeerLauncher.Minecraft;
using Microsoft.VisualBasic.Devices;

namespace SikaDeerLauncher
{
    public sealed class MinecraftDownload
    {
        Download web = new Download();
        //java下载
        /// <summary>
        /// java下载
        /// </summary>
        /// <returns></returns>
        public MCDownload JavaFileDownload()
        {
            string javanumder;
            SikaDeerLauncher.Minecraft.Tools a = new Minecraft.Tools();
            if (a.GetOSBit() == 32)
            {
                javanumder = "jre_x86.exe";
            }
            else
            {
                javanumder = "jre_x64.exe";
            }
            MCDownload GetJavaDownload = new MCDownload();
            GetJavaDownload.Url = @"https://bmclapi.bangbang93.com/java/" + javanumder;
            GetJavaDownload.path = System.IO.Directory.GetCurrentDirectory() + @"\SikaDeerLauncherDownload\" + javanumder;
            return GetJavaDownload;
        }

        //mc本体下载
        SikaDeerLauncher.Core.SikaDeerLauncherCore SLC = new Core.SikaDeerLauncherCore();
        /// <summary>
        /// mc本体下载
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public MCDownload MCjarDownload(string version)
        {
            MCDownload download = new MCDownload();
            if (Tools.DSI == "Minecraft")
            {
                if (Tools.mcV.ToArray().Length == 0)
                {
                    string v = SLC.GetFile(@".minecraft\version.Sika");
                    string[] a1 = v.Split('|');
                    foreach (var a2 in a1)
                    {
                        var a3 = a2.Split('&');
                        mc mc = new mc();
                        mc.version = a3[0];
                        mc.url = a3[1];
                        Tools.mcV.Add(mc);
                    }
                }
                foreach (var l in Tools.mcV)
                {
                    if (l.version == version)
                    {
                        download.Url = l.url;
                        download.path = System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar";
                    }
                }
            }
            else
            {
                download.Url = @"https://bmclapi2.bangbang93.com/version/" + version + "/client";
                download.path = System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar";
            }
            return download;
        }
        /// <summary>
        /// MCjson下载
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public MCDownload MCjsonDownload(string version)
        {
            MCDownload download = new MCDownload();
            if (Tools.DSI == "Minecraft")
            {
                string json = web.getHtml(@"https://launchermeta.mojang.com/mc/game/version_manifest.json");
                if (json != "")
                {
                    var jo = JsonConvert.DeserializeObject<Core.json.mcweb.Root>(json);

                    foreach (var jo1 in jo.versions)
                    {
                        if (jo1.id == version)
                        {
                            download.Url = jo1.url;
                            download.path = System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json";

                        }
                    }
                }
                else
                {
                    download.Url = @"https://bmclapi2.bangbang93.com/version/" + version + "/json";
                    download.path = System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json";
                }
            }
            else
            {
                download.Url = @"https://bmclapi2.bangbang93.com/version/" + version + "/json";
                download.path = System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json";
            }
            return download;
        }
        /// <summary>
        /// 取Forge下载地址
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="ForgeVersion">Forge版本</param>
        /// <returns></returns>
        public MCDownload ForgeDownload(string version,string ForgeVersion)
        {
            Tools tools = new Tools();
            MCDownload download = new MCDownload();
            if (version.Length >= 5)
            {
                download.path = System.IO.Directory.GetCurrentDirectory()+@"\SikaDeerLauncherDownload\" +"forge - " + version + " - " + ForgeVersion  + " - installer.jar";
                download.Url = "https://bmclapi2.bangbang93.com/maven/net/minecraftforge/forge/" + version + "-" + ForgeVersion  + "/forge-" + version + "-" + ForgeVersion  + "-installer.jar";
                return download;
            }
            download.path = System.IO.Directory.GetCurrentDirectory()+@"\SikaDeerLauncherDownload\" +"forge-" + version + "-" + ForgeVersion + "-" + version + "-installer.jar";
            download.Url = "https://bmclapi2.bangbang93.com/maven/net/minecraftforge/forge/"+version+"-"+ForgeVersion+"-"+version+"/forge-"+version+"-"+ForgeVersion+"-"+version+"-installer.jar";
            return download;
        }
        /// <summary>
        /// 取Liteloader下载
        /// </summary>
        /// <param name="version">Liteloader版本</param>
        /// <returns></returns>
        public MCDownload DownloadLiteloader(string version)
        {
            Tools tools = new Tools();
            AllTheExistingVersion[] all = tools.GetAllTheExistingVersion();
            foreach (var a in all)
            {
                if (a.version == version)
                {
                    version = a.IdVersion;
                    break;
                }
                else if(a.version == all[all.Length-1].version)
                {
                    throw new SikaDeerLauncherException("未找到该版本");
                }
            }
            MCDownload download = new MCDownload();
            download.path = System.IO.Directory.GetCurrentDirectory() + @"\SikaDeerLauncherDownload\" + "liteloader-" + version + ".jar";
            download.Url = "https://bmclapi2.bangbang93.com/maven/com/mumfrey/liteloader/" + version + "/liteloader-" + version + ".jar";
            return download;
        }
        /// <summary>
        /// 取Optifine下载
        /// </summary>
        /// <param name="version">mc版本</param>
        /// <param name="filename">Optifine文件名</param>
        /// <returns></returns>
        public MCDownload DownloadOptifine(string version,string filename)
        {
            Tools tools = new Tools();
            AllTheExistingVersion[] all = tools.GetAllTheExistingVersion();
            foreach (var a in all)
            {
                if (a.version == version)
                {
                    version = a.IdVersion;
                    break;
                }
                else if (a.version == all[all.Length - 1].version)
                {
                    throw new SikaDeerLauncherException("未找到该版本");
                }
            }
            MCDownload download = new MCDownload();
            download.path = System.IO.Directory.GetCurrentDirectory() + @"\SikaDeerLauncherDownload\" + filename;
            download.Url = "https://bmclapi2.bangbang93.com/maven/com/optifine/" + version + "/"+filename;
            return download;
        }
    }

}
