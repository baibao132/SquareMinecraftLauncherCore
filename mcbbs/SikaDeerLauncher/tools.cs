namespace SikaDeerLauncher.Minecraft
{
    using AI;
    using BlessingSkinJson;
    using global::MinecraftServer.Server;
    using json2;
    using json4;
    using mcbbs;
    using Microsoft.Win32;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SikaDeerLauncher;
    using SikaDeerLauncher.Core;
    using SikaDeerLauncher.Core.Forge;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using windows;
    using File = System.IO.File;

    public sealed class Tools
    {
        private Baidu baidu = new Baidu();
        internal static string DSI = "https://bmclapi2.bangbang93.com/libraries/";
        internal static List<mc> mcV = new List<mc>();
        private SikaDeerLauncherCore SLC = new SikaDeerLauncherCore();
        private bool vp;
        private Download web = new Download();

        public bool ChangeTheTitle(string Text)
        {
            WinAPI.SetWindowText(WinAPI.GetHandle("LWJGL"), Text + " SikaDeerLauncher");
            return true;
        }

        public void DownloadSourceInitialization(DownloadSource downloadSource)
        {
            this.baidu.Tts();
            if (downloadSource == DownloadSource.MinecraftSource)
            {
                DSI = "Minecraft";
            }
            else
            {
                DSI = null;
            }
        }
        public bool ForgeExist(string version)
        {
            return(GetLocalForgeVersion(version) != null);
        }
        public bool ForgeExist(string version,ref string ForgeVersion)
        {
            ForgeVersion = GetLocalForgeVersion(version);
            if (ForgeVersion != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ForgeInstallation(string ForgePath, string version, string java)
        {
            string str;
            if ((this.SLC.FileExist(ForgePath) == null) && new Unzip().UnZipFile(ForgePath, @"SikaDeerLauncher\Forge\", out str))
            {
                string[] textArray1 = new string[] { Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".jar" };
                if (this.SLC.FileExist(string.Concat(textArray1)) == null)
                {
                    ForgeCore core = new ForgeCore();
                    this.SLC.wj(@".minecraft\versions\" + version + @"\" + version + ".json", core.ForgeJson(version, @"SikaDeerLauncher\Forge\version.json"));
                    if (this.SLC.FileExist(@"SikaDeerLauncher\Forge\install_profile.json") == null)
                    {
                        new ForgeInstallCore().ForgeInstall(@"SikaDeerLauncher\Forge\install_profile.json", version, java);
                        char[] separator = new char[] { '\\' };
                        string[] strArray = ((JObject) JsonConvert.DeserializeObject(this.SLC.GetFile(Directory.GetCurrentDirectory() + @"\SikaDeerLauncher\Forge\install_profile.json")))["path"].ToString().Replace(':', '\\').Split(separator);
                        string[] textArray3 = new string[] { strArray[0].Replace('.', '\\'), @"\", strArray[1], @"\", strArray[2] };
                        string str2 = string.Concat(textArray3);
                        string path = Directory.GetCurrentDirectory() + @"\.minecraft\libraries\" + str2;
                        this.SLC.path(path);
                        foreach (string str4 in Directory.GetFiles(@"SikaDeerLauncher\Forge\maven\" + str2))
                        {
                            try
                            {
                                File.Copy(str4, path + @"\" + Path.GetFileName(str4));
                            }
                            catch
                            {
                            }
                        }
                    }
                    this.SLC.DelPathOrFile(@"SikaDeerLauncher\Forge\");
                    return true;
                }
            }
            return false;
        }

        public string[] ForgeVersionList()
        {
            string str = this.web.getHtml("https://bmclapi2.bangbang93.com/forge/minecraft");
            if (str == null)
            {
                throw new SikaDeerLauncherException("请求失败");
            }
            return JsonConvert.DeserializeObject<List<string>>(str).ToArray();
        }

        public MCDownload[] GetAllFile(string version)
        {
            MCDownload[] downloadArray;
            MinecraftDownload download = new MinecraftDownload();
            this.baidu.Tts();
            try
            {
                var root = this.SLC.versionjson<json4.Root>(version);
                List<MCDownload> list = new List<MCDownload>();
                string dSI = DSI;
                if (DSI == "Minecraft")
                {
                    dSI = "https://libraries.minecraft.net/";
                }
                else
                {
                    dSI = "https://bmclapi2.bangbang93.com/libraries/";
                }
                foreach (LibrariesItem item in root.libraries)
                {
                    string str2 = null;
                    if (item.natives != null)
                    {
                        if (item.natives.windows == null)
                        {
                            continue;
                        }
                        str2 = this.SLC.libAnalysis(item.name, false, item.natives.windows);
                    }
                    else
                    {
                        str2 = this.SLC.libAnalysis(item.name, false, "");
                    }
                    MCDownload download2 = new MCDownload {
                        name = item.name,
                        mainClass = root.mainClass
                    };
                    char[] separator = new char[] { ':' };
                    string[] strArray = item.name.Split(separator);
                    download2.Url = dSI + str2.Replace('\\', Convert.ToChar("/"));
                    download2.path = Directory.GetCurrentDirectory() + @"\.minecraft\libraries\" + str2;
                    if ((item.downloads != null) && (item.downloads.artifact != null))
                    {
                        if ((item.downloads.artifact.url.IndexOf("files.minecraftforge.net") != -1))
                        {
                            char[] chArray2 = new char[] { ':' };
                            string[] strArray2 = item.name.Split(chArray2);
                            string str3 = strArray2[2];
                            if (strArray2[2].IndexOf('-') != -1)
                            {
                                char[] chArray3 = new char[] { '-' };
                                str3 = strArray2[2].Split(chArray3)[0];
                            }
                            string[] textArray1 = new string[] { strArray2[0].Replace('.', Convert.ToChar(@"\")), @"\", strArray2[1], @"\", str3, @"\", strArray2[1], "-", strArray2[2], ".jar" };
                            str2 = string.Concat(textArray1);
                            download2.Url = "https://files.minecraftforge.net/maven/" + str2.Replace('\\', Convert.ToChar("/"));
                        }
                        if (strArray[1] == "OptiFine")
                        {
                            download2.Url = item.downloads.artifact.url;
                        }
                        else if (strArray[1] == "liteloader")
                        {
                            download2.Url = item.downloads.artifact.url;
                        }
                    }
                    if (strArray[1] == "forge")
                    {
                        char[] chArray4 = new char[] { '-' };
                        string[] strArray3 = strArray[2].Split(chArray4);
                        download2.Url = download.ForgeCoreDownload(strArray3[0], strArray3[1]).Url;
                        list.Add(download2);
                        download2.name = "";
                    }
                    list.Add(download2);
                }
                downloadArray = this.SLC.screening(list.ToArray());
            }
            catch (Exception)
            {
                throw new SikaDeerLauncherException("版本有问题，请重新下载");
            }
            return downloadArray;
        }

        public MCDownload[] GetAllLibrary(string version)
        {
            List<MCDownload> list = new List<MCDownload>();
            MCDownload[] allFile = this.GetAllFile(version);
            MCDownload[] allNatives = this.GetAllNatives(version);
            for (int i = 0; i < allFile.Length; i++)
            {
                int index = 0;
                while (index < allNatives.Length)
                {
                    if (allFile[i].path == allNatives[index].path)
                    {
                        break;
                    }
                    index++;
                }
                if (index == allNatives.Length)
                {
                    list.Add(allFile[i]);
                }
            }
            return list.ToArray();
        }

        public MCDownload[] GetAllNatives(string version)
        {
            MCDownload[] downloadArray;
            this.baidu.Tts();
            try
            {
                List<MCDownload> list = new List<MCDownload>();
                string dSI = DSI;
                if (DSI == "Minecraft")
                {
                    dSI = "https://libraries.minecraft.net/";
                }
                else
                {
                    dSI = "https://bmclapi2.bangbang93.com/libraries/";
                }
                foreach (JToken token in this.SLC.versionjson(version)["libraries"])
                {
                    if ((token["natives"] != null) && (token["natives"]["windows"] != null))
                    {
                        string str2 = this.SLC.libAnalysis(token["name"].ToString(), false, token["natives"]["windows"].ToString());
                        MCDownload item = new MCDownload {
                            Url = dSI + str2.Replace('\\', '/'),
                            path = Directory.GetCurrentDirectory() + @"\.minecraft\libraries\" + str2
                        };
                        list.Add(item);
                    }
                }
                downloadArray = list.ToArray();
            }
            catch (Exception)
            {
                throw new SikaDeerLauncherException("版本有问题，请重新下载");
            }
            return downloadArray;
        }

        public MCDownload[] GetAllTheAsset(string version)
        {
            try
            {
                var jo = SLC.versionjson<json4.Root>(version);

                string json = web.getHtml(jo.AssetIndex.url);
                mcbbs.mcbbsnews mcbbs = new mcbbs.mcbbsnews();
                string[] str = new string[0];
                List<MCDownload> str2 = new List<MCDownload>();
                var j = JObject.Parse(json).Value<JObject>("objects");
                JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
                string jstr;
                string dsi = "http://resources.download.minecraft.net";
                foreach (var o in j.Properties())
                {
                    jstr = o.Name;
                    MCDownload assets = new MCDownload();
                    var hash = json1["objects"][o.Name]["hash"].ToString();
                    assets.path = System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\assets\objects\" + hash[0] + hash[1] + "\\" + hash;
                    assets.Url = dsi + @"/" + hash[0] + hash[1] + "/" + hash;
                    str2.Add(assets);
                }
                return str2.ToArray();
            }
            catch (Exception ex)
            {
                throw new SikaDeerLauncherException("无法连接网络");
            }
        }

        public AllTheExistingVersion[] GetAllTheExistingVersion()
        {
            this.baidu.Tts();
            new mcbbsnews();
            List<AllTheExistingVersion> list = new List<AllTheExistingVersion>();
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\.minecraft\versions"))
            {
                throw new SikaDeerLauncherException("没有找到任何版本");
            }
            foreach (string str in Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\.minecraft\versions"))
            {
                string str2 = this.SLC.app(str, Convert.ToChar(@"\"), "versions");
                if (File.Exists(str + @"\" + str2 + ".jar") && File.Exists(str + @"\" + str2 + ".json"))
                {
                    ForgeY.Root root = new ForgeY.Root();
                    try
                    {
                        root = JsonConvert.DeserializeObject<ForgeY.Root>(this.SLC.GetFile(str + @"\" + str2 + ".json"));
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    AllTheExistingVersion item = new AllTheExistingVersion {
                        path = str,
                        version = str2
                    };
                    if (!this.vp)
                    {
                        new Thread(new ThreadStart(this.SLC.MCVersion)) { IsBackground = true }.Start();
                        this.vp = true;
                    }
                    if (mcV.ToArray().Length == 0)
                    {
                        ForgeInstallCore.Delay(2000);
                        char[] separator = new char[] { '|' };
                        string[] strArray2 = this.SLC.GetFile(@".minecraft\version.Sika").Split(separator);
                        for (int i = 0; i < strArray2.Length; i++)
                        {
                            char[] chArray2 = new char[] { '&' };
                            string[] strArray3 = strArray2[i].Split(chArray2);
                            mc mc = new mc {
                                version = strArray3[0],
                                url = strArray3[1]
                            };
                            mcV.Add(mc);
                        }
                    }
                    if (root == null)
                    {
                        throw new SikaDeerLauncherException("无任何版本");
                    }
                    try
                    {
                        foreach (mc mc2 in mcV)
                        {
                            if (mc2.url == root.downloads.client.url)
                            {
                                item.IdVersion = mc2.version;
                                break;
                            }
                        }
                        if (item.IdVersion != null)
                        {
                            list.Add(item);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return list.ToArray();
        }

        public Skin GetAuthlib_Injector(string yggdrasilURL, string username, string password)
        {
            string str = this.web.Post(yggdrasilURL + "/authserver/authenticate", "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}");
            BlessingSkin.Root root = new BlessingSkin.Root();
            try
            {
                root = JsonConvert.DeserializeObject<BlessingSkin.Root>(str);
            }
            catch (Exception)
            {
                throw new SikaDeerLauncherException("yggdrasil网址有误");
            }
            if (root == null)
            {
                throw new SikaDeerLauncherException("网络有问题");
            }
            if (root.accessToken == null)
            {
                throw new SikaDeerLauncherException(Regex.Unescape(JsonConvert.DeserializeObject<BlessingSkinError>(str).errorMessage));
            }
            Skin skin = new Skin {
                accessToken = root.accessToken
            };
            List<SkinName> list = new List<SkinName>();
            foreach (BlessingSkin.AvailableProfilesItem item in root.availableProfiles)
            {
                SkinName name = new SkinName {
                    Name = item.name,
                    uuid = item.id
                };
                list.Add(name);
            }
            skin.NameItem = list.ToArray();
            return skin;
        }

        public bool GetCompareForgeVersions(string version)
        {
            string localForgeVersion = this.GetLocalForgeVersion(version);
            if (localForgeVersion == null)
            {
                throw new SikaDeerLauncherException("没有装Forge");
            }
            char[] separator = new char[] { '.' };
            string[] strArray = this.GetMaxForge(version).ForgeVersion.Split(separator);
            char[] chArray2 = new char[] { '-' };
            char[] chArray3 = new char[] { '.' };
            string[] strArray2 = localForgeVersion.Split(chArray2)[1].Split(chArray3);
            if (strArray2.Length == 3)
            {
                if (((Convert.ToInt32(strArray2[0]) <= Convert.ToInt32(strArray[0])) && (Convert.ToInt32(strArray2[1]) <= Convert.ToInt32(strArray[1]))) && ((Convert.ToInt32(strArray2[2]) <= Convert.ToInt32(strArray[2])) && (strArray.Length == 4)))
                {
                    return true;
                }
            }
            else if (((Convert.ToInt32(strArray2[0]) <= Convert.ToInt32(strArray[0])) && (Convert.ToInt32(strArray2[1]) <= Convert.ToInt32(strArray[1]))) && ((Convert.ToInt32(strArray2[2]) <= Convert.ToInt32(strArray[2])) && (Convert.ToInt32(strArray2[3]) <= Convert.ToInt32(strArray[3]))))
            {
                return true;
            }
            return false;
        }

        public ForgeList[] GetForgeList(string version)
        {
            string localForgeVersion = this.GetLocalForgeVersion(version);
            if (localForgeVersion != null)
            {
                char[] separator = new char[] { '-' };
                version = localForgeVersion.Split(separator)[0];
            }
            else
            {
                foreach (AllTheExistingVersion version2 in this.GetAllTheExistingVersion())
                {
                    if (version2.version == version)
                    {
                        version = version2.IdVersion;
                    }
                }
            }
            string str2 = this.web.getHtml("https://bmclapi2.bangbang93.com/forge/minecraft/" + version);
            if ((str2 != "[]") && (str2 != null))
            {
                List<ForgeList> list = new List<ForgeList>();
                foreach (JToken token in (JArray) JsonConvert.DeserializeObject(str2))
                {
                    ForgeList item = new ForgeList {
                        version = token["mcversion"].ToString(),
                        ForgeVersion = token["version"].ToString(),
                        ForgeTime = token["modified"].ToString()
                    };
                    list.Add(item);
                }
                return list.ToArray();
            }
            if (str2 == null)
            {
                throw new SikaDeerLauncherException("访问失败");
            }
            throw new SikaDeerLauncherException("版本有误或目前没有该版本");
        }

        public string GetJavaPath()
        {
            RegistryKey key;
            string str = "";
            if (Environment.Is64BitOperatingSystem)
            {
                key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            }
            else
            {
                key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            }
            try
            {
                str = key.OpenSubKey(@"SOFTWARE\JavaSoft\Java Runtime Environment\1.8", false).GetValue("JavaHome", true).ToString() + @"\bin\javaw.exe";
            }
            catch
            {
            }
            key.Close();
            if (str == "")
            {
                int length = Environment.SystemDirectory.Length;
                string str2 = Environment.SystemDirectory.Remove(3, length - 3);
                string[] directories = new string[0];
                if (this.GetOSBit() == 0x40)
                {
                    try
                    {
                        directories = Directory.GetDirectories(str2 + @"Program Files\Java\");
                    }
                    catch
                    {
                    }
                    if (directories.Length != 0)
                    {
                        if (this.SLC.FileExist(directories[0] + @"\bin\javaw.exe") == null)
                        {
                            str = directories[0] + @"\bin\javaw.exe";
                        }
                        return str;
                    }
                }
                if (directories.Length == 0)
                {
                    try
                    {
                        directories = Directory.GetDirectories(str2 + @"Program Files (x86)\Java\");
                    }
                    catch
                    {
                    }
                }
                if ((directories.Length != 0) && (this.SLC.FileExist(directories[0] + @"\bin\javaw.exe") == null))
                {
                    str = directories[0] + @"\bin\javaw.exe";
                }
            }
            return str;
        }

        public LiteloaderList[] GetLiteloaderList()
        {
            List<LiteloaderList> list = new List<LiteloaderList>();
            string text1 = this.web.getHtml("https://bmclapi2.bangbang93.com/liteloader/list");
            if (text1 == null)
            {
                throw new SikaDeerLauncherException("获取失败");
            }
            foreach (JToken token in (JArray) JsonConvert.DeserializeObject(text1))
            {
                LiteloaderList item = new LiteloaderList();
                List<Lib> list3 = new List<Lib>();
                item.version = token["version"].ToString();
                item.mcversion = token["mcversion"].ToString();
                foreach (JToken token2 in token["build"]["libraries"])
                {
                    Lib lib = new Lib {
                        name = token2["name"].ToString()
                    };
                    list3.Add(lib);
                }
                item.lib = list3.ToArray();
                item.tweakClass = token["build"]["tweakClass"].ToString();
                list.Add(item);
            }
            return list.ToArray();
        }

        public string GetLocalForgeVersion(string version)
        {
            using (List<LibrariesItem>.Enumerator enumerator = this.SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);
                    if (strArray[0] == "net.minecraftforge")
                    {
                        return strArray[2];
                    }
                }
            }
            return null;
        }

        public ForgeList GetMaxForge(string version)
        {
            ForgeList[] forgeList = this.GetForgeList(version);
            int index = 0;
            string forgeVersion = forgeList[0].ForgeVersion;
            for (int i = 1; forgeList.Length > i; i++)
            {
                char[] separator = new char[] { '.' };
                string[] strArray = forgeVersion.Split(separator);
                char[] chArray2 = new char[] { '.' };
                string[] strArray2 = forgeList[i].ForgeVersion.Split(chArray2);
                if (strArray.Length != 3)
                {
                    if (((Convert.ToInt32(strArray[0]) <= Convert.ToInt32(strArray2[0])) && (Convert.ToInt32(strArray[1]) <= Convert.ToInt32(strArray2[1]))) && ((Convert.ToInt32(strArray[2]) <= Convert.ToInt32(strArray2[2])) && (Convert.ToInt32(strArray[3]) <= Convert.ToInt32(strArray2[3]))))
                    {
                        index = i;
                        forgeVersion = forgeList[i].ForgeVersion;
                    }
                }
                else if (((Convert.ToInt32(strArray[0]) <= Convert.ToInt32(strArray2[0])) && (Convert.ToInt32(strArray[1]) <= Convert.ToInt32(strArray2[1]))) && (Convert.ToInt32(strArray[2]) <= Convert.ToInt32(strArray2[2])))
                {
                    index = i;
                    forgeVersion = forgeList[i].ForgeVersion;
                }
            }
            return forgeList[index];
        }
        /// <summary>
        /// 取MC列表
        /// </summary>
        /// <returns></returns>
        public MCVersionList[] GetMCVersionList()
        {
            string text = this.web.getHtml("http://118.31.6.246/libraries/mc/game/version_manifest.json");
            if (text == null)
            {
                throw new SikaDeerLauncherException("请求失败");
            }
            List<MCVersionList> list = new List<MCVersionList>();
            foreach (JToken token in (JArray) JsonConvert.DeserializeObject(new mcbbsnews().TakeTheMiddle(text, "\"versions\":", "]}") + "]"))
            {
                string str2 = this.SLC.MCVersionAnalysis(token["type"].ToString());
                MCVersionList item = new MCVersionList {
                    type = str2,
                    id = token["id"].ToString(),
                    releaseTime = token["releaseTime"].ToString()
                };
                list.Add(item);
            }
            return list.ToArray();
        }
        /// <summary>
        /// 取合适内存
        /// </summary>
        /// <returns></returns>
        public MemoryInformation GetMemorySize()
        {
            this.baidu.Tts();
            MEMORY_INFO meminfo = new MEMORY_INFO();
            GlobalMemoryStatus(ref meminfo);
            MemoryInformation information = new MemoryInformation {
                TotalMemory = (int) (meminfo.dwTotalVirtual / 0x100000)
            };
            if (information.TotalMemory == 0)
            {
                information.AppropriateMemory = 512;
                return information;
            }
            if (this.GetOSBit() == 64)
            {
                if (GetJavaPath().IndexOf("x86") >= 0)
                {
                    information.AppropriateMemory = 512;
                    return information;
                }
                information.AppropriateMemory = ((1024 * information.TotalMemory) / 1024) / 2;
                return information;
            }
            if (information.TotalMemory <= 1024)
            {
                information.AppropriateMemory = 512;
                return information;
            }
            information.AppropriateMemory = 1024;
            return information;
        }

        public MCDownload[] GetMissingAsset(string version)
        {
            List<MCDownload> list = new List<MCDownload>();
            foreach (MCDownload download in this.GetAllTheAsset(version))
            {
                string path = download.path;
                if (this.SLC.FileExist(path) != null)
                {
                    list.Add(download);
                }
            }
            return list.ToArray();
        }

        public MCDownload[] GetMissingFile(string version)
        {
            this.baidu.Tts();
            List<MCDownload> list = new List<MCDownload>();
            foreach (MCDownload download in this.GetAllFile(version))
            {
                if (this.SLC.FileExist(download.path) != null)
                {
                    list.Add(download);
                }
            }
            return list.ToArray();
        }

        public MCDownload[] GetMissingLibrary(string version)
        {
            List<MCDownload> list = new List<MCDownload>();
            foreach (MCDownload download in this.GetAllLibrary(version))
            {
                if (this.SLC.FileExist(download.path) != null)
                {
                    list.Add(download);
                }
            }
            return list.ToArray();
        }

        public MCDownload[] GetMissingNatives(string version)
        {
            this.baidu.Tts();
            List<MCDownload> list = new List<MCDownload>();
            foreach (MCDownload download in this.GetAllNatives(version))
            {
                if (this.SLC.FileExist(download.path) != null)
                {
                    list.Add(download);
                }
            }
            return list.ToArray();
        }

        public OptiFineList[] GetOptiFineList(string version)
        {
            AllTheExistingVersion[] allTheExistingVersion = this.GetAllTheExistingVersion();
            foreach (AllTheExistingVersion version2 in allTheExistingVersion)
            {
                if (version2.version == version)
                {
                    version = version2.IdVersion;
                    break;
                }
                if (version2.version == allTheExistingVersion[allTheExistingVersion.Length - 1].version)
                {
                    throw new SikaDeerLauncherException("未找到该版本");
                }
            }
            List<OptiFineList> list = new List<OptiFineList>();
            string str = this.web.getHtml("https://bmclapi2.bangbang93.com/optifine/" + version);
            switch (str)
            {
                case null:
                    throw new SikaDeerLauncherException("获取失败");

                case "[]":
                    throw new SikaDeerLauncherException("OptiFine不支持该版本");
            }
            foreach (JToken token in (JArray) JsonConvert.DeserializeObject(str))
            {
                OptiFineList item = new OptiFineList {
                    mcversion = token["mcversion"].ToString(),
                    filename = token["filename"].ToString(),
                    type = token["type"].ToString(),
                    patch = token["patch"].ToString()
                };
                list.Add(item);
            }
            return list.ToArray();
        }

        public int GetOSBit()
        {
            this.baidu.Tts();
            if (Environment.Is64BitOperatingSystem)
            {
                return 0x40;
            }
            return 0x20;
        }

        public ServerInfo GetServerInformation(string ip, int port)
        {
            ServerInfo info;
            this.baidu.Tts();
            if (((ip == "") || (port == 0)) || (ip == null))
            {
                throw new SikaDeerLauncherException("ip或port不得为空");
            }
            try
            {
                ServerInfo info1 = new ServerInfo(ip, port);
                info1.StartGetServerInfo();
                info = info1;
            }
            catch (Exception exception1)
            {
                throw new SikaDeerLauncherException(exception1.Message);
            }
            return info;
        }

        public MCDownload[] GetTheExistingLibrary(string version)
        {
            List<MCDownload> list = new List<MCDownload>();
            foreach (MCDownload download in this.GetAllLibrary(version))
            {
                if (this.SLC.FileExist(download.path) == null)
                {
                    list.Add(download);
                }
            }
            return list.ToArray();
        }

        public UnifiedPass GetUnifiedPass(string ID, string username, string password)
        {
            UnifiedPass pass = new UnifiedPass();
            string str = this.web.Post("https://auth2.nide8.com:233/" + ID + "/authserver/authenticate", "{\"agent\": {\"name\": \"Sika Deer Launcher\",\"version\": 2.23},\"username\": \"" + username + "\",\"password\": \"" + password + "\",\"clientToken\": \"htty\",\"requestUser\": true}");
            if (str == null)
            {
                throw new SikaDeerLauncherException("请求失败");
            }
            UPerror.Root root = JsonConvert.DeserializeObject<UPerror.Root>(str);
            if (root.errorMessage != null)
            {
                throw new SikaDeerLauncherException(Regex.Unescape(root.errorMessage));
            }
            UP.Root root2 = JsonConvert.DeserializeObject<UP.Root>(str);
            pass.accessToken = root2.accessToken;
            pass.id = root2.selectedProfile.id;
            pass.name = root2.selectedProfile.name;
            return pass;
        }

        public UnifiedPassesTheSkin[] GetUnifiedPassesTheSkin(string ID, string username, string password)
        {
            List<UnifiedPassesTheSkin> list = new List<UnifiedPassesTheSkin>();
            UnifiedPass pass = this.GetUnifiedPass(ID, username, password);
            string str = this.web.getHtml("https://auth2.nide8.com:233/" + ID + "/sessionserver/session/minecraft/profile/" + pass.id);
            if (str == null)
            {
                throw new SikaDeerLauncherException("请求失败");
            }
            foreach (UPSkin.PropertiesItem item in JsonConvert.DeserializeObject<UPSkin.Root>(str).properties)
            {
                UPSkinBase.Root root = JsonConvert.DeserializeObject<UPSkinBase.Root>(Convert.ToBase64String(Encoding.Default.GetBytes(item.value)));
                UnifiedPassesTheSkin skin = new UnifiedPassesTheSkin();
                if (root.textures.SKIN != null)
                {
                    skin.Skin = root.textures.SKIN.url;
                }
                if (root.textures.CAPE != null)
                {
                    skin.Cape = root.textures.CAPE.url;
                }
                list.Add(skin);
            }
            return list.ToArray();
        }

        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);
        public bool LiteloaderExist(string version)
        {
            using (List<LibrariesItem>.Enumerator enumerator = this.SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);
                    if ((strArray[0] == "com.mumfrey") && (strArray[1] == "liteloader"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool LiteloaderExist(string version,ref string LiteloaderVersion)
        {
            using (List<LibrariesItem>.Enumerator enumerator = this.SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);
                    if ((strArray[0] == "com.mumfrey") && (strArray[1] == "liteloader"))
                    {
                        LiteloaderVersion = strArray[2];
                        return true;
                    }
                }
            }
            return false;
        }

        public bool liteloaderInstall(string version)
        {
            LiteloaderCore core = new LiteloaderCore();
            this.SLC.wj(@".minecraft\versions\" + version + @"\" + version + ".json", core.LiteloaderJson(version));
            return true;
        }

        public Getlogin MinecraftLogin(string username, string password)
        {
            this.baidu.Tts();
            if (((username == "") || (password == "")) || ((username == null) || (password == null)))
            {
                throw new SikaDeerLauncherException("账号密码不得为空");
            }
            string str = this.web.Post("https://authserver.mojang.com/authenticate", "{\"agent\":{\"name\":\"Minecraft\",\"version\":\"1\"},\"username\":\"" + username + "\", \"password\":\"" + password + "\", \"requestUser\":\"true\"}");
            if ((str == null) || !(str != ""))
            {
                throw new SikaDeerLauncherException("请检查网络");
            }
            Console.WriteLine(str);
            var root = JsonConvert.DeserializeObject<json2.Root>(str);
            if (root.errorMessage == null)
            {
                Getlogin getlogin = new Getlogin {
                    uuid = root.selectedProfile.id,
                    token = root.accessToken,
                    name = root.selectedProfile.name
                };
                Console.WriteLine(getlogin.token);
                string str1 = this.web.Post("https://authserver.mojang.com/authenticate", getlogin.token, "Authorization");
                Console.WriteLine(str1);
                  string[] textArray2 = new string[] { "{", root.user.properties[0].name, ":[", root.user.properties[0].value, "]}" };
                getlogin.twitch = string.Concat(textArray2);
                return getlogin;
            }
            if (root.error == "ForbiddenOperationException")
            {
                if (root.errorMessage == "Invalid credentials. Account migrated, use e-mail as username.")
                {
                    throw new SikaDeerLauncherException("凭证错误");
                }
                if (root.errorMessage == "Invalid credentials. Invalid username or password.")
                {
                    throw new SikaDeerLauncherException("密码账户错误");
                }
            }
            throw new SikaDeerLauncherException(root.error);
        }

        public bool OptifineExist(string version)
        {
            using (List<LibrariesItem>.Enumerator enumerator = this.SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);
                    if ((strArray[0] == "optifine") && (strArray[1] == "OptiFine"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool OptifineExist(string version,ref string OptifineVersion)
        {
            using (List<LibrariesItem>.Enumerator enumerator = this.SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);
                    if ((strArray[0] == "optifine") && (strArray[1] == "OptiFine"))
                    {
                        OptifineVersion = strArray[2];
                        return true;
                    }
                }
            }
            return false;
        }

        public bool OptifineInstall(string version, string patch)
        {
            OptiFineList[] optiFineList = this.GetOptiFineList(version);
            AllTheExistingVersion[] allTheExistingVersion = this.GetAllTheExistingVersion();
            string str = version;
            foreach (AllTheExistingVersion version2 in allTheExistingVersion)
            {
                if (version2.version == version)
                {
                    version = version2.IdVersion;
                    break;
                }
                if (version2.version == allTheExistingVersion[allTheExistingVersion.Length - 1].version)
                {
                    throw new SikaDeerLauncherException("未找到该版本");
                }
            }
            OptiFineList list = new OptiFineList();
            foreach (OptiFineList list2 in optiFineList)
            {
                if (list2.mcversion == version)
                {
                    list = list2;
                }
            }
            OptifineCore core = new OptifineCore();
            this.SLC.wj(@".minecraft\versions\" + str + @"\" + str + ".json", core.OptifineJson(str, list));
            return true;
        }

        public void UninstallTheExpansionPack(ExpansionPack ExpansionPack, string version)
        {
            string str = version;
            MinecraftDownload download = new MinecraftDownload();
            version = "";
            foreach (AllTheExistingVersion version2 in this.GetAllTheExistingVersion())
            {
                if (version2.version == str)
                {
                    version = version2.IdVersion;
                }
            }
            if (version == "")
            {
                throw new SikaDeerLauncherException("未找到该版本");
            }
            string[] strArray = new string[0];
            using (List<LibrariesItem>.Enumerator enumerator = this.SLC.versionjson<Root1>(str).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray2 = enumerator.Current.name.Split(separator);
                    if ((strArray2[0] == "optifine") && (strArray2[1] == "OptiFine"))
                    {
                        char[] chArray2 = new char[] { '_' };
                        strArray = strArray2[2].Split(chArray2);
                    }
                }
            }
            bool flag = false;
            if (this.LiteloaderExist(str))
            {
                flag = true;
            }
            MCDownload download2 = download.MCjsonDownload(version);
            string text = this.web.getHtml(download2.Url);
            switch (ExpansionPack)
            {
                case ExpansionPack.Forge:
                    if (!this.ForgeExist(str))
                    {
                        throw new SikaDeerLauncherException("没有安装Forge");
                    }
                    this.SLC.wj(Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + str + @"\" + str + ".json", text);
                    if (strArray.Length != 0)
                    {
                        this.SLC.opKeep(str, strArray[strArray.Length - 1]);
                    }
                    if (!flag)
                    {
                        break;
                    }
                    this.SLC.liKeep(str);
                    return;

                case ExpansionPack.Liteloader:
                    if (!this.LiteloaderExist(str))
                    {
                        throw new SikaDeerLauncherException("没有安装Liteloader");
                    }
                    if (!this.SLC.ForgeKeep(str, text))
                    {
                        this.SLC.wj(Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + str + @"\" + str + ".json", text);
                    }
                    if (strArray.Length == 0)
                    {
                        break;
                    }
                    this.SLC.opKeep(str, strArray[strArray.Length - 1]);
                    return;

                case ExpansionPack.Optifine:
                    if (!this.OptifineExist(str))
                    {
                        throw new SikaDeerLauncherException("没有安装Optifine");
                    }
                    if (!this.SLC.ForgeKeep(str, text))
                    {
                        this.SLC.wj(Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + str + @"\" + str + ".json", text);
                    }
                    if (!flag)
                    {
                        break;
                    }
                    this.SLC.liKeep(str);
                    return;

                default:
                    return;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }
    }
}

