﻿namespace SquareMinecraftLauncher
{
    using global::SquareMinecraftLauncher.Core;
    using global::SquareMinecraftLauncher.Core.json;
    using global::SquareMinecraftLauncher.Minecraft;
    using Newtonsoft.Json;
    using System;

    public sealed class MinecraftDownload
    {
        private SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        private Download web = new Download();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LiteloaderVersion"></param>
        /// <returns></returns>
        public MCDownload DownloadLiteloader(string LiteloaderVersion)
        {
            MCDownload download = new MCDownload
            {
                path = System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncherDownload\liteloader-" + LiteloaderVersion + ".jar"
            };
            string[] textArray1 = new string[] { "https://bmclapi2.bangbang93.com/maven/com/mumfrey/liteloader/", LiteloaderVersion, "/liteloader-", LiteloaderVersion, ".jar" };
            download.Url = string.Concat(textArray1);
            return download;
        }

        public MCDownload DownloadOptifine(string version, string filename)
        {
            AllTheExistingVersion[] allTheExistingVersion = new Tools().GetAllTheExistingVersion();
            foreach (AllTheExistingVersion version2 in allTheExistingVersion)
            {
                if (version2.version == version)
                {
                    version = version2.IdVersion;
                    break;
                }
                if (version2.version == allTheExistingVersion[allTheExistingVersion.Length - 1].version)
                {
                    throw new SquareMinecraftLauncherException("未找到该版本");
                }
            }
            string[] textArray2 = new string[] { "optifine:OptiFine:", filename.Replace("preview_", "").Replace("OptiFine_", "") };
            if (filename.IndexOf("pre") > 0) filename = "preview_" + filename;
            return new MCDownload { path = System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncherDownload\" + filename, Url = "https://bmclapi2.bangbang93.com/maven/com/optifine/" + version + "/" + filename, name = string.Concat(textArray2).Replace(".jar", "") };
        }

        internal MCDownload ForgeCoreDownload(string version, string ForgeVersion)
        {
            MCDownload download = new MCDownload();
            string str = version;
            version = "";
            foreach (AllTheExistingVersion version2 in new Tools().GetAllTheExistingVersion())
            {
                if (version2.version == str)
                {
                    version = version2.IdVersion;
                    break;
                }
            }
            if (version == "")
            {
                throw new SquareMinecraftLauncherException("未找到该版本");
            }
            string str2 = "https://bmclapi2.bangbang93.com";
            if (Tools.DSI == "Minecraft")
            {
                str2 = "https://files.minecraftforge.net";
            }
            char[] separator = new char[] { '.' };
            string[] textArray3 = new string[] { System.Directory.GetCurrentDirectory(), @"\SquareMinecraftLauncherDownload\forge-", version, "-", ForgeVersion, "-universal.jar" };
            download.path = string.Concat(textArray3);
            string[] textArray4 = new string[] { str2, "/maven/net/minecraftforge/forge/", version, "-", ForgeVersion, "/forge-", version, "-", ForgeVersion, "-universal.jar" };
            download.Url = string.Concat(textArray4);
            try
            {
                new Download().CreateGetHttpResponse(download.Url);
            }
            catch (Exception)
            {
                string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\SquareMinecraftLauncherDownload\forge-", version, "-", ForgeVersion, "-", version, "-universal.jar" };
                download.path = string.Concat(textArray1);
                string[] textArray2 = new string[] { "https://bmclapi2.bangbang93.com/maven/net/minecraftforge/forge/", version, "-", ForgeVersion, "-", version, "/forge-", version, "-", ForgeVersion, "-", version, "-universal.jar" };
                download.Url = string.Concat(textArray2);
            }
            return download;
        }

        public MCDownload ForgeDownload(string version, string ForgeVersion)
        {
            MCDownload download = new MCDownload();
            string str = version;
            version = "";
            foreach (AllTheExistingVersion version2 in new Tools().GetAllTheExistingVersion())
            {
                if (version2.version == str)
                {
                    version = version2.IdVersion;
                    break;
                }
            }
            if (version == "")
            {
                throw new SquareMinecraftLauncherException("未找到该版本");
            }
            char[] separator = new char[] { '.' };
            if (Convert.ToInt32(version.Split(separator)[1]) < 9)
            {
                return this.ForgeCoreDownload(str, ForgeVersion);
            }
            string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\SquareMinecraftLauncherDownload\forge - ", version, " - ", ForgeVersion, "-installer.jar" };
            download.path = string.Concat(textArray1);
            string[] textArray2 = new string[] { "https://bmclapi2.bangbang93.com/maven/net/minecraftforge/forge/", version, "-", ForgeVersion, "/forge-", version, "-", ForgeVersion, "-installer.jar" };
            download.Url = string.Concat(textArray2);
            try
            {
                new Download().CreateGetHttpResponse(download.Url);
            }
            catch (Exception)
            {
                string[] textArray3 = new string[] { System.Directory.GetCurrentDirectory(), @"\SquareMinecraftLauncherDownload\forge-", version, "-", ForgeVersion, "-", version, "-installer.jar" };
                download.path = string.Concat(textArray3);
                string[] textArray4 = new string[] { "https://bmclapi2.bangbang93.com/maven/net/minecraftforge/forge/", version, "-", ForgeVersion, "-", version, "/forge-", version, "-", ForgeVersion, "-", version, "-installer.jar" };
                download.Url = string.Concat(textArray4);
            }
            return download;
        }

        public MCDownload JavaFileDownload()
        {
            string str;
            if (new Tools().GetOSBit() == 0x20)
            {
                str = "jre_x86.exe";
            }
            else
            {
                str = "jre_x64.exe";
            }
            return new MCDownload { Url = "https://bmclapi.bangbang93.com/java/" + str, path = System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncherDownload\" + str };
        }

        public MCDownload MCjarDownload(string version)
        {
            MCDownload download = new MCDownload();
            if (Tools.DSI == "Minecraft")
            {
                if (Tools.mcV.ToArray().Length == 0)
                {
                    char[] separator = new char[] { '|' };
                    string[] strArray = this.SLC.GetFile(@".minecraft\version.Sika").Split(separator);
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        char[] chArray2 = new char[] { '&' };
                        string[] strArray2 = strArray[i].Split(chArray2);
                        mc item = new mc
                        {
                            version = strArray2[0],
                            url = strArray2[1]
                        };
                        Tools.mcV.Add(item);
                    }
                }
                foreach (mc mc2 in Tools.mcV)
                {
                    if (mc2.version == version)
                    {
                        download.Url = mc2.url;
                        string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".jar" };
                        download.path = string.Concat(textArray1);
                    }
                }
                return download;
            }
            download.Url = "https://download.mcbbs.net/version/" + version + "/client";
            string[] textArray2 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".jar" };
            download.path = string.Concat(textArray2);
            return download;
        }

        public MCDownload MCjsonDownload(string version)
        {
            MCDownload download = new MCDownload();
            if (Tools.DSI == "Minecraft")
            {
                string str = this.web.getHtml("https://launchermeta.mojang.com/mc/game/version_manifest.json");
                if (str != null)
                {
                    foreach (mcweb.VersionsItem item in JsonConvert.DeserializeObject<mcweb.Root>(str).versions)
                    {
                        if (item.id == version)
                        {
                            download.Url = item.url;
                            string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".json" };
                            download.path = string.Concat(textArray1);
                        }
                    }
                    return download;
                }
                download.Url = "https://bmclapi2.bangbang93.com/version/" + version + "/json";
                string[] textArray2 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".json" };
                download.path = string.Concat(textArray2);
                return download;
            }
            download.Url = "https://bmclapi2.bangbang93.com/version/" + version + "/json";
            string[] textArray3 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".json" };
            download.path = string.Concat(textArray3);
            return download;
        }
    }
}

