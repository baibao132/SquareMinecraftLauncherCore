using MinecraftServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using json;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Timers;
using File = System.IO.File;
using System.Drawing;
using SikaDeerLauncher.Core;
using mcbbs;
using System.Threading;

namespace SikaDeerLauncher.Minecraft
{
    public sealed class Tools
    {
        AI.Baidu baidu = new AI.Baidu();
        private SikaDeerLauncherCore SLC = new SikaDeerLauncherCore();
        Download web = new Download();
        internal static string DSI = "https://bmclapi2.bangbang93.com/libraries/";
        /// <summary>
        /// 下载源初始化
        /// </summary>
        /// <param name="downloadSource"></param>
        public void DownloadSourceInitialization(DownloadSource downloadSource)
        {
            baidu.Tts();
            if (downloadSource == DownloadSource.MinecraftSource)
            {
                DSI = "Minecraft";
            }
            else
            {
                DSI = null;
            }
        }
        /// <summary>
        /// 获取所有库
        /// </summary>
        /// <param name="version">版本</param>
        public MCDownload[] GetAllFile(string version)
        {
            MinecraftDownload minecraft = new MinecraftDownload();
            baidu.Tts();
            try
            {
                var rb = SLC.versionjson<json4.Root>(version);
                List<MCDownload> a = new List<MCDownload>();
                string dsi = DSI;
                if (DSI == "Minecraft")
                {
                    dsi = "https://libraries.minecraft.net/";
                }
                else
                {
                    dsi = "https://bmclapi2.bangbang93.com/libraries/";
                }
                foreach (var jo in rb.libraries)
                {

                    string LAs = null;
                    if (jo.natives != null)
                    {
                        if (jo.natives.windows != null)
                        {
                            LAs = SLC.libAnalysis(jo.name, false, jo.natives.windows);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        LAs = SLC.libAnalysis(jo.name, false, "");
                    }
                    MCDownload download = new MCDownload();
                    download.Url = dsi + LAs.Replace('\\', Convert.ToChar("/"));
                    download.name = jo.name;
                    download.mainClass = rb.mainClass;
                    string[] split= jo.name.Split(':');
                    if (jo.downloads != null && jo.downloads.artifact != null)
                    {
                        if (jo.downloads.artifact.url == "")
                        {
                            download.Url = "https://bmclapi2.bangbang93.com/libraries/" + LAs.Replace('\\', Convert.ToChar("/"));
                        }
                        else if (jo.downloads.artifact.url.IndexOf("files.minecraftforge.net") != -1 && DSI == "Minecraft")
                        {
                            string[] stra = jo.name.Split(':');
                            string sp = stra[2];
                            if (stra[2].IndexOf('-') != -1)
                            {
                                sp = stra[2].Split('-')[0];
                            }
                            LAs = stra[0].Replace('.', Convert.ToChar("\\")) + "\\" + stra[1] + "\\" + sp + "\\" + stra[1] + "-" + stra[2] + ".jar";

                            download.Url = "https://files.minecraftforge.net/maven/" + LAs.Replace('\\', Convert.ToChar("/"));
                        }
                        if (split[1] == "OptiFine")
                        {
                            download.Url = jo.downloads.artifact.url;
                        }
                        else if (split[1] == "liteloader")
                        {
                            download.Url = jo.downloads.artifact.url;
                        }
                        else if (jo.downloads.artifact.url.IndexOf("files.minecraftforge.net") != -1 && DSI != "Minecraft")
                        {
                            string[] stra = jo.name.Split(':');
                            string sp = stra[2];
                            if (stra[2].IndexOf('-') != -1)
                            {
                                sp = stra[2].Split('-')[0];
                            }
                            LAs = stra[0].Replace('.', Convert.ToChar("\\")) + "\\" + stra[1] + "\\" + sp + "\\" + stra[1] + "-" + stra[2] + ".jar";
                            download.Url = "https://bmclapi2.bangbang93.com/libraries/" + LAs.Replace('\\', Convert.ToChar("/"));
                        }
                    }
                    if (split[1] == "forge")
                    {
                        var forge = split[2].Split('-');
                        download.Url = minecraft.ForgeDownload(forge[0], forge[1]).Url;
                    }
                    else if (jo.downloads != null && jo.downloads.artifact != null && jo.natives == null)
                    {
                        download.Url = "https://bmclapi2.bangbang93.com/libraries/" + LAs.Replace('\\', Convert.ToChar("/"));
                    }
                        download.path = System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\" + LAs;
                        a.Add(download);
                }
                return SLC.screening(a.ToArray());
            }
            catch (Exception ex)
            {
                throw new SikaDeerLauncherException("版本有问题，请重新下载");
            }

        }
        /// <summary>
        /// 获取缺少库
        /// </summary>
        /// <param name="version">版本</param>
        public MCDownload[] GetMissingFile(string version)
        {
            baidu.Tts();
            MCDownload[] downloads = GetAllFile(version);
            List<MCDownload> str = new List<MCDownload>();
            foreach (var a in downloads)
            {
                string file = SLC.FileExist(a.path);
                if (file != null)
                {
                    str.Add(a);
                }


            }
            return str.ToArray();
        }

        /// <summary>
        /// 获取系统位数
        /// </summary>
        /// <returns></returns>
        public int GetOSBit()
        {
            baidu.Tts();
            bool type;
            type = Environment.Is64BitOperatingSystem;
            if (type == true)
            {
                return 64;
            }
            else
            {
                return 32;
            }
        }
        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        /// <returns></returns>
        public ServerInfo GetServerInformation(string ip, int port)
        {
            baidu.Tts();
            if (ip == "" || port == 0 || ip == null)
            {
                throw new SikaDeerLauncherException("ip或port不得为空");
            }
            try
            {
                ServerInfo info = new ServerInfo(ip, port);
                info.StartGetServerInfo();
                return info;
            }
            catch (Exception ex)
            {
                throw new SikaDeerLauncherException(ex.Message);
            }
        }
        /// <summary>
        /// 我的世界正版登录
        /// </summary>
        /// <param name="username">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public Getlogin MinecraftLogin(string username, string password)
        {
            baidu.Tts();
            if (username == "" || password == "" || username == null || password == null) throw new SikaDeerLauncherException("账号密码不得为空");
            string post = web.Post("https://authserver.mojang.com/authenticate", "{\"agent\":{\"name\":\"Minecraft\",\"version\":\"1\"},\"username\":\"" + username + "\", \"password\":\"" + password + "\", \"requestUser\":\"true\"}");
            if (post != null && post != "")
            {
                json2.Root jo = JsonConvert.DeserializeObject<json2.Root>(post);
                if (jo.errorMessage == null)
                {
                    Getlogin Getlogins = new Getlogin();
                    Getlogins.uuid = jo.selectedProfile.id;
                    Getlogins.token = jo.accessToken;
                    Getlogins.name = jo.selectedProfile.name;
                    Getlogins.twitch = "{" + jo.user.properties[0].name + ":[" + jo.user.properties[0].value + "]}";
                    return Getlogins;
                }
                else if (jo.error == "ForbiddenOperationException")
                {
                    if (jo.errorMessage == "Invalid credentials. Account migrated, use e-mail as username.")
                    {
                        throw new SikaDeerLauncherException("凭证错误");
                    }
                    else if (jo.errorMessage == "Invalid credentials. Invalid username or password.")
                    {
                        throw new SikaDeerLauncherException("密码账户错误");

                    }
                }
                throw new SikaDeerLauncherException(jo.error);

            }
            throw new SikaDeerLauncherException("请检查网络");
        }
        internal static List<mc> mcV = new List<mc>();
        bool vp = false;
        /// <summary>
        /// 取所有版本
        /// </summary>
        /// <returns></returns>
        public AllTheExistingVersion[] GetAllTheExistingVersion()
        {
            baidu.Tts();
            mcbbs.mcbbsnews a = new mcbbs.mcbbsnews();
            List<AllTheExistingVersion> aev = new List<AllTheExistingVersion>();
            if (Directory.Exists(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions"))
            {
                string[] list = Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions");
                foreach (string lists in list)
                {
                    string ap = SLC.app(lists, Convert.ToChar(@"\"), "versions");
                    if (System.IO.File.Exists(lists + @"\" + ap + ".jar"))
                    {
                        if (System.IO.File.Exists(lists + @"\" + ap + ".json"))
                        {
                            Core.Forge.ForgeY.Root jo = new Core.Forge.ForgeY.Root();
                            try
                            {
                               jo = JsonConvert.DeserializeObject<SikaDeerLauncher.Core.Forge.ForgeY.Root>(SLC.GetFile(lists + @"\" + ap + ".json"));
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                            AllTheExistingVersion ls = new AllTheExistingVersion();
                            ls.path = lists;
                            ls.version = ap;
                            if (vp == false)
                            {
                                Thread thread = new Thread(new ThreadStart(SLC.MCVersion));
                                thread.IsBackground = true;
                                thread.Start();
                                vp = true;
                            }
                            if (mcV.ToArray().Length == 0)
                            {
                                string v = SLC.GetFile(@".minecraft\version.Sika");
                                string[] a1 = v.Split('|');
                                foreach (var a2 in a1)
                                {
                                    var a3 = a2.Split('&');
                                    mc mc = new mc();
                                    mc.version = a3[0];
                                    mc.url = a3[1];
                                    mcV.Add(mc);
                                }
                            }
                            foreach (var l in mcV)
                            {
                                if (l.url == jo.downloads.client.url)
                                {
                                    ls.IdVersion = l.version;
                                    break;
                                }
                            }
                            if (ls.IdVersion != null)
                            {
                                aev.Add(ls);
                            }

                        }
                    }
                }
                return aev.ToArray();
            }
            else
            {
                throw new SikaDeerLauncherException("没有找到任何版本");
            }
        }
        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);
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
        /// <summary>
        /// 取合适内存大小
        /// </summary>
        /// <returns></returns>
        public MemoryInformation GetMemorySize()
        {
            baidu.Tts();
            MEMORY_INFO meminfo = new MEMORY_INFO(); ;
            GlobalMemoryStatus(ref meminfo);
            MemoryInformation memoryInformation = new MemoryInformation();
            memoryInformation.TotalMemory = (int)meminfo.dwTotalVirtual / 1048576;
            if (memoryInformation.TotalMemory == 0)
            {
                memoryInformation.AppropriateMemory = 512;
                return memoryInformation;
            }
            if (GetOSBit() == 64)
            {
                memoryInformation.AppropriateMemory = 1024 * memoryInformation.TotalMemory / 1024 / 2;
                return memoryInformation;
            }
            else if (memoryInformation.TotalMemory <= 1024)
            {
                memoryInformation.AppropriateMemory = 512;
                return memoryInformation;
            }
            else
            {
                memoryInformation.AppropriateMemory = 1024;
                return memoryInformation;
            }
        }

        /// <summary>
        /// 取所有natives
        /// </summary>
        /// <param name="version">版本</param>
        public MCDownload[] GetAllNatives(string version)
        {
            baidu.Tts();
            try
            {
            var rb = SLC.versionjson(version);
            List<MCDownload> a = new List<MCDownload>();
            string dsi = DSI;
            if (DSI == "Minecraft")
            {
                dsi = "https://libraries.minecraft.net/";
            }
            else
            {
                dsi = "https://bmclapi2.bangbang93.com/libraries/";
            }
            foreach (var jo in rb["libraries"])
            {
                if (jo["natives"] != null)
                {
                    if (jo["natives"]["windows"] != null)
                    {
                            string lib = SLC.libAnalysis(jo["name"].ToString(), false, jo["natives"]["windows"].ToString());
                            MCDownload download = new MCDownload();
                            download.Url = dsi + lib.Replace('\\', '/');
                            download.path = System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\" + lib;
                            a.Add(download);
                    }
                }

            }
            return a.ToArray();
            }
            catch (Exception ex)
            {
                throw new SikaDeerLauncherException("版本有问题，请重新下载");
            }
        }
        /// <summary>
        /// 获取缺少natives
        /// </summary>
        /// <param name="version">版本</param>
        public MCDownload[] GetMissingNatives(string version)
        {
            baidu.Tts();
            MCDownload[] natives = GetAllNatives(version);
            List<MCDownload> str = new List<MCDownload>();
            foreach (var a in natives)
            {
                string file = SLC.FileExist(a.path);
                if (file != null)
                {
                    str.Add(a);
                }
            }
            return str.ToArray();
        }
        /// <summary>
        /// 取所有Lib
        /// </summary>
        /// <param name="version">版本</param>
        public MCDownload[] GetAllLibrary(string version)
        {
            List<MCDownload> a = new List<MCDownload>();
            MCDownload[] File = GetAllFile(version);
            MCDownload[] Natives = GetAllNatives(version);
            for (int i = 0; i < File.Length; i++)
            {
                int t = 0;
                for (; t < Natives.Length; t++)
                {
                    if (File[i].path == Natives[t].path)
                    {
                        break;
                    }
                }
                if (t == Natives.Length)
                {
                    a.Add(File[i]);
                }
                
            }
            return a.ToArray();
        }
        /// <summary>
        /// 取缺少的Lib
        /// </summary>
        /// <param name="version">版本</param>
        public MCDownload[] GetMissingLibrary(string version)
        {
            MCDownload[] Libraries = GetAllLibrary(version);
            List<MCDownload> str = new List<MCDownload>();
            foreach (var a in Libraries)
            {
                if (SLC.FileExist(a.path) != null)
                {
                    str.Add(a);
                }
            }
            return str.ToArray();
        }
        /// <summary>
        /// 取存在的Lib
        /// </summary>
        /// <param name="version">版本</param>
        public MCDownload[] GetTheExistingLibrary(string version)
        {
            MCDownload[] All = GetAllLibrary(version);
            List<MCDownload> Exist = new List<MCDownload>();
            foreach (var a in All)
            {

                if (SLC.FileExist(a.path) == null)
                {
                    Exist.Add(a);
                }
            }
            return Exist.ToArray();
        }
        /// <summary>
        /// 取java路径
        /// </summary>
        /// <returns></returns>
        public string GetJavaPath()
        {
            string javaRuntimePath = "";

            RegistryKey localKey;
            if (Environment.Is64BitOperatingSystem)
                localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            else
                localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

            try
            {
                RegistryKey software = localKey.OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment\\1.8", false);
                javaRuntimePath = software.GetValue("JavaHome", true).ToString() + "\\bin\\javaw.exe";
            }
            catch { }
            localKey.Close();
            return javaRuntimePath;

        }
        /// <summary>
        /// 取所有的资源文件
        /// </summary>
        /// <param name="version">版本</param>
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
                throw new SikaDeerLauncherException("版本有问题，请重新下载");
            }
        }
        /// <summary>
        /// 取缺少的资源文件
        /// </summary>
        /// <param name="version">版本</param>
        public MCDownload[] GetMissingAsset(string version)
        {
            List<MCDownload> assetadd = new List<MCDownload>();
            MCDownload[] downloads = GetAllTheAsset(version);
            foreach (var ap in downloads)
            {
                string a = ap.path;
                if (SLC.FileExist(a) != null)
                {
                    assetadd.Add(ap);
                }

            }
            return assetadd.ToArray();
        }
        /// <summary>
        /// 获取外置登录信息
        /// </summary>
        /// <param name="yggdrasilURL">yggdrasil地址</param>
        /// <param name="username">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public Skin GetAuthlib_Injector(string yggdrasilURL, string username, string password)
        {
            string json = web.Post(yggdrasilURL + "/authserver/authenticate", "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}");//post访问
            BlessingSkinJson.BlessingSkin.Root BSJ = null;
            try
            {
                BSJ = JsonConvert.DeserializeObject<BlessingSkinJson.BlessingSkin.Root>(json);//解析
            }
            catch (Exception ex)
            {
                throw new SikaDeerLauncherException("yggdrasil网址有误");
            }
            if (BSJ.accessToken == null)
            {
                var BSJError = JsonConvert.DeserializeObject<BlessingSkinJson.BlessingSkinError>(json);//解析
                throw new SikaDeerLauncherException(Regex.Unescape(BSJError.errorMessage));//unicode转中文
            }
            Skin blessingSkin = new Skin();
            blessingSkin.accessToken = BSJ.accessToken;
            List<SkinName> skinNames = new List<SkinName>();
            foreach (var blessing in BSJ.availableProfiles)//遍历
            {
                SkinName name = new SkinName();
                name.Name = blessing.name;
                name.uuid = blessing.id;
                skinNames.Add(name);
            }
            blessingSkin.NameItem = skinNames.ToArray();
            return blessingSkin;
        }
        /// <summary>
        /// 取版本的forge版本
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public string GetLocalForgeVersion(string version)
        {
            var json = SLC.versionjson<json4.Root1>(version);
            foreach (var j in json.libraries)
            {
                var split = j.name.Split(':');
                if (split[0] == "net.minecraftforge")
                {
                    return split[2];
                }
            }
            return null;
        }
        /// <summary>
        /// 判断Forge更新
        /// </summary>
        /// <param name="version"></param>
        /// <returns>false是无需更新，true是需要更新</returns>
        public bool GetCompareForgeVersions(string version)
        {
            string GLFV = GetLocalForgeVersion(version);
            if (GLFV == null)
            {
                throw new SikaDeerLauncherException("没有装Forge");
            }
            var FLA = GetMaxForge(version).ForgeVersion.Split('.');
            var FLI = GLFV.Split('-')[1].Split('.');
            if (FLI.Length == 3)
            {
                if (Convert.ToInt32(FLI[0]) <= Convert.ToInt32(FLA[0]) && Convert.ToInt32(FLI[1]) <= Convert.ToInt32(FLA[1]) && Convert.ToInt32(FLI[2]) <= Convert.ToInt32(FLA[2]))
                {
                    if (FLA.Length == 4)
                    {
                        return true;
                    }
                }
            }
            else if (Convert.ToInt32(FLI[0]) <= Convert.ToInt32(FLA[0]) && Convert.ToInt32(FLI[1]) <= Convert.ToInt32(FLA[1]) && Convert.ToInt32(FLI[2]) <= Convert.ToInt32(FLA[2]) && Convert.ToInt32(FLI[3]) <= Convert.ToInt32(FLA[3]))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取Forge列表
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public ForgeList[] GetForgeList(string version)
        {
            string a = GetLocalForgeVersion(version);
            if (a != null)
            {
                version = a.Split('-')[0];
            }
            else
            {
                var ev = GetAllTheExistingVersion();
                foreach (var i in ev)
                {
                    if (i.version == version)
                    {
                        version = i.IdVersion;
                    }
                }
            }
            string html = web.getHtml("https://bmclapi2.bangbang93.com/forge/minecraft/" + version);
            if (html != "[]" && html != null)
            {
                var JA = (JArray)JsonConvert.DeserializeObject(html);
                List<ForgeList> FL = new List<ForgeList>();
                foreach (var j in JA)
                {
                    ForgeList forge = new ForgeList();
                    forge.version = j["mcversion"].ToString();
                    forge.ForgeVersion = j["version"].ToString();
                    forge.ForgeTime = j["modified"].ToString();
                    FL.Add(forge);
                }
                return FL.ToArray();
            }
            else if (html == null)
            {
                throw new SikaDeerLauncherException("访问失败");
            }
            throw new SikaDeerLauncherException("版本有误或目前没有该版本");
        }
        /// <summary>
        /// 取最高Forge版本
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns>返回ForgeList</returns>
        public ForgeList GetMaxForge(string version)
        {

            ForgeList[] FL = GetForgeList(version);
            int MaxForgeInt = 0;
            string flMax = FL[0].ForgeVersion;
            for (int i = 1; FL.Length > i; i++)
            {
                var FLI = flMax.Split('.');
                var FLA = FL[i].ForgeVersion.Split('.');
                if (FLI.Length != 3)
                {
                    if (Convert.ToInt32(FLI[0]) <= Convert.ToInt32(FLA[0]) && Convert.ToInt32(FLI[1]) <= Convert.ToInt32(FLA[1]) && Convert.ToInt32(FLI[2]) <= Convert.ToInt32(FLA[2]) && Convert.ToInt32(FLI[3]) <= Convert.ToInt32(FLA[3]))
                    {
                        MaxForgeInt = i;
                        flMax = FL[i].ForgeVersion;
                    }
                    continue;
                }
                    if (Convert.ToInt32(FLI[0]) <= Convert.ToInt32(FLA[0]) && Convert.ToInt32(FLI[1]) <= Convert.ToInt32(FLA[1]) && Convert.ToInt32(FLI[2]) <= Convert.ToInt32(FLA[2]))
                    {
                        MaxForgeInt = i;
                        flMax = FL[i].ForgeVersion;
                    }
            }
            return FL[MaxForgeInt];
        }
        /// <summary>
        /// 取所有Forge支持的版本
        /// </summary>
        /// <returns></returns>
        public string[] ForgeVersionList()
        {
            string a = web.getHtml("https://bmclapi2.bangbang93.com/forge/minecraft");
            if (a != null)
            {
                return JsonConvert.DeserializeObject<List<string>>(a).ToArray();
            }
            throw new SikaDeerLauncherException("请求失败");

        }
        /// <summary>
        /// 安装Forge
        /// </summary>
        /// <param name="ForgePath">Forge路径</param>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public bool ForgeInstallation(string ForgePath, string version)
        {
            if (SLC.FileExist(ForgePath) == null)
            {
                string error;
                Unzip unzip = new Unzip();
                if (unzip.UnZipFile(ForgePath, @"SikaDeerLauncher\Forge\", out error) == true && SLC.FileExist(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar") == null)
                {
                    StreamReader sr = null;
                    try
                    {
                        sr = new StreamReader(@"SikaDeerLauncher\Forge\install_profile.json", Encoding.Default);
                    }
                    catch (System.IO.DirectoryNotFoundException ex)
                    {
                        return false;
                    }
                    var jo = (JObject)JsonConvert.DeserializeObject(sr.ReadToEnd());
                    sr.Close();
                    bool get = false;
                    try
                    {
                        get = unzip.UnZipFile(@"SikaDeerLauncher\Forge\" + jo["install"]["filePath"].ToString(), @"SikaDeerLauncher\Forge\install\", out error);
                    }
                    catch (Exception ex)
                    {
                        StreamReader sr1 = null;
                        try
                        {
                           sr1 = new StreamReader(@"SikaDeerLauncher\Forge\version.json", Encoding.Default);
                        }
                        catch (System.IO.DirectoryNotFoundException)
                        {
                            return false;
                        }
                        jo = (JObject)JsonConvert.DeserializeObject(sr1.ReadToEnd());
                        sr1.Close();
                        Core.ForgeCore forgeCore = new Core.ForgeCore();
                        SLC.wj(@".minecraft\versions\" + version + @"\" + version + ".json", forgeCore.ForgeJson(version, @"SikaDeerLauncher\Forge\version.json"));
                        SLC.DelPathOrFile(@"SikaDeerLauncher\Forge\");
                        //target
                        return true;
                    }
                    if (get == true)
                    {
                        Core.ForgeCore forgeCore = new Core.ForgeCore();
                        SLC.wj(@".minecraft\versions\" + version + @"\" + version + ".json", forgeCore.ForgeJson(version, @"SikaDeerLauncher\Forge\install\version.json"));
                        SLC.DelPathOrFile(@"SikaDeerLauncher\Forge\");
                        //target
                        return true;
                    }

                }

            }
            return false;
        }
        /// <summary>
        /// 取Liteloader列表
        /// </summary>
        /// <returns></returns>
        public LiteloaderList[] GetLiteloaderList()
        {
            List<LiteloaderList> lists = new List<LiteloaderList>();
            string html = web.getHtml("https://bmclapi2.bangbang93.com/liteloader/list");
            if (html == null)
            {
                throw new SikaDeerLauncherException("获取失败");
            }
            var jo = (JArray)JsonConvert.DeserializeObject(html);
            foreach (var j in jo)
            {
                LiteloaderList list = new LiteloaderList();
                List<Lib> libs = new List<Lib>();
                list.version = j["version"].ToString();
                list.mcversion = j["mcversion"].ToString();
                foreach (var lib in j["build"]["libraries"])
                {
                    Lib l = new Lib();
                    l.name = lib["name"].ToString();
                    libs.Add(l);
                }
                list.lib = libs.ToArray();
                list.tweakClass = j["build"]["tweakClass"].ToString();
                lists.Add(list);
            }
            return lists.ToArray();
        }
        /// <summary>
        /// 取OptiFineList列表
        /// </summary>
        /// <param name="version">mc版本</param>
        /// <returns></returns>
        public OptiFineList[] GetOptiFineList(string version)
        {
            AllTheExistingVersion[] all = GetAllTheExistingVersion();
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
            List<OptiFineList> optiFineLists = new List<OptiFineList>();
            string html = web.getHtml("https://bmclapi2.bangbang93.com/optifine/" + version);
            if (html == null)
            {
                throw new SikaDeerLauncherException("获取失败");
            }
            else if (html == "[]")
            {
                throw new SikaDeerLauncherException("OptiFine不支持该版本");
            }
            var jo = (JArray)JsonConvert.DeserializeObject(html);
            foreach (var j in jo)
            {
                OptiFineList optiFineList = new OptiFineList();
                optiFineList.mcversion = j["mcversion"].ToString();
                optiFineList.filename = j["filename"].ToString();
                optiFineList.type = j["type"].ToString();
                optiFineList.patch = j["patch"].ToString();
                optiFineLists.Add(optiFineList);
            }
            return optiFineLists.ToArray();
        }
        /// <summary>
        /// liteloader自动安装
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public bool liteloaderInstall(string version)
        {
            LiteloaderCore liteloader = new LiteloaderCore();
            SLC.wj(@".minecraft\versions\" + version + @"\" + version + ".json", liteloader.LiteloaderJson(version));
            return true;
        }
        /// <summary>
        /// 判断Liteloader是否存在
        /// </summary>
        /// <param name="version">mc版本</param>
        /// <returns></returns>
        public bool LiteloaderExist(string version)
        {
            var json = SLC.versionjson<json4.Root1>(version);
            foreach (var j in json.libraries)
            {
                var split = j.name.Split(':');
                if (split[0] == "com.mumfrey" && split[1] == "liteloader")
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断Optifine是否存在
        /// </summary>
        /// <param name="version">mc版本</param>
        /// <returns></returns>
        public bool OptifineExist(string version)
        {
            var json = SLC.versionjson<json4.Root1>(version);
            foreach (var j in json.libraries)
            {
                var split = j.name.Split(':');
                if (split[0] == "optifine" && split[1] == "OptiFine")
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Optifine自动安装
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="patch">补丁</param>
        /// <returns></returns>
        public bool OptifineInstall(string version,string patch)
        {
            OptiFineList[] optiFines = GetOptiFineList(version);
            AllTheExistingVersion[] all = GetAllTheExistingVersion();
            string pastversion = version;
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
            OptiFineList optiFine = new OptiFineList();
            foreach (var ap in optiFines)
            {
                if (ap.mcversion == version)
                {
                    optiFine = ap;
                }
            }
            OptifineCore Optifine = new OptifineCore();
            SLC.wj(@".minecraft\versions\" + pastversion + @"\" + pastversion + ".json", Optifine.OptifineJson(pastversion,optiFine));
            return true;
        }
        //mc列表
        /// <summary>
        /// 获取mc列表
        /// </summary>
        public MCVersionList[] GetMCVersionList()
        {
            string json = web.getHtml(@"https://launchermeta.mojang.com/mc/game/version_manifest.json");
            if (json != "")
            {
                mcbbsnews mcbbs = new mcbbsnews();
                json = mcbbs.TakeTheMiddle(json, "\"versions\":", "]}");//取json数组
                JArray jobject = (JArray)JsonConvert.DeserializeObject(json + "]");//解析json数组
                List<MCVersionList> VersionLists = new List<MCVersionList>();
                foreach (var ja in jobject)//遍历
                {
                    string type = SLC.MCVersionAnalysis(ja["type"].ToString());
                    MCVersionList list = new MCVersionList();
                    list.type = type;
                    list.id = ja["id"].ToString();
                    list.releaseTime = ja["releaseTime"].ToString();
                    VersionLists.Add(list);
                }
                return VersionLists.ToArray();
            }
            else
            {
                throw new SikaDeerLauncherException("请求失败");
            }
        }
        /// <summary>
        /// 统一通行证
        /// </summary>
        /// <param name="ID">服务器ID</param>
        /// <param name="username">统一通行证用户名</param>
        /// <param name="password">统一通行证密码</param>
        /// <returns></returns>
        public UnifiedPass GetUnifiedPass(string ID,string username,string password)
        {
            UnifiedPass unified = new UnifiedPass();
            string a = web.Post("https://auth2.nide8.com:233/"+ID+ "/authserver/authenticate", "{\"agent\": {\"name\": \"Sika Deer Launcher\",\"version\": 2.23},\"username\": \""+username+"\",\"password\": \""+password+"\",\"clientToken\": \"htty\",\"requestUser\": true}");
            if (a != null)
            {
                var jo = JsonConvert.DeserializeObject<UPerror.Root>(a);
                if (jo.errorMessage == null)
                {
                    var jo1 = JsonConvert.DeserializeObject<UP.Root>(a);
                    unified.accessToken = jo1.accessToken;
                    unified.id = jo1.selectedProfile.id;
                    unified.name = jo1.selectedProfile.name;
                }
                else
                {
                    throw new SikaDeerLauncherException(Regex.Unescape(jo.errorMessage));
                }
            }
            else
            {
                throw new SikaDeerLauncherException("请求失败");
            }
            return unified;

        }
        /// <summary>
        /// 修改窗口标题
        /// </summary>
        /// <param name="Text">标题</param>
        /// <returns></returns>
        public bool ChangeTheTitle(string Text)
        {
           IntPtr ptr = windows.WinAPI.GetHandle("LWJGL");
            if (ptr == null)
            {
                return false;
            }
            windows.WinAPI.SetWindowText(ptr, Text + " SikaDeerLauncher");
            return true;
        }
        /// <summary>
        /// 取统一通行证皮肤
        /// </summary>
        /// <param name="ID">服务器ID</param>
        /// <param name="username">统一通行证用户名</param>
        /// <param name="password">统一通行证密码</param>
        public UnifiedPassesTheSkin[] GetUnifiedPassesTheSkin(string ID, string username, string password)
        {
            List<UnifiedPassesTheSkin> unifiedPassesTheSkin = new List<UnifiedPassesTheSkin>();
            UnifiedPass UP = GetUnifiedPass(ID, username, password);
            string html = web.getHtml("https://auth2.nide8.com:233/"+ID+ "/sessionserver/session/minecraft/profile/"+UP.id);
            if (html != null)
            {
                var jo = JsonConvert.DeserializeObject<UPSkin.Root>(html);
                foreach (var jo1 in jo.properties)
                {
                    byte[] Base = Encoding.Default.GetBytes(jo1.value);
                    string BS = Convert.ToBase64String(Base);
                    var jo2 = JsonConvert.DeserializeObject<UPSkinBase.Root>(BS);
                    UnifiedPassesTheSkin unifiedPasses = new UnifiedPassesTheSkin();
                    if (jo2.textures.SKIN != null)
                    {
                        unifiedPasses.Skin = jo2.textures.SKIN.url;
                    }
                    if (jo2.textures.CAPE != null)
                    {
                        unifiedPasses.Cape = jo2.textures.CAPE.url;
                    }
                    unifiedPassesTheSkin.Add(unifiedPasses);
                }
                return unifiedPassesTheSkin.ToArray();
            }
            throw new SikaDeerLauncherException("请求失败");
        }
        /// <summary>
        /// 判断Forge是否存在
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public bool ForgeExist(string version)
        {
            if (GetLocalForgeVersion(version) != null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 卸载扩展包
        /// </summary>
        /// <param name="ExpansionPack">卸载类型</param>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public void UninstallTheExpansionPack(ExpansionPack ExpansionPack,string version)
        {
            string a = version;
            MinecraftDownload download = new MinecraftDownload();
            AllTheExistingVersion[] versions = GetAllTheExistingVersion();
            version = "";
            foreach (var i in versions)
            {
                if (i.version == a)
                {
                    version = i.IdVersion;
                }
            }
            if (version == "")
            {
                throw new SikaDeerLauncherException("未找到该版本");
            }
            var json = SLC.versionjson<json4.Root1>(a);
            string[] d = new string[0];
            foreach (var j in json.libraries)
            {
                var split = j.name.Split(':');
                if (split[0] == "optifine" && split[1] == "OptiFine")
                {
                    d = split[2].Split('_');
                } 
            }
            bool lite = false;
            if (LiteloaderExist(a))
            {
                lite = true;
            }
                MCDownload mc = download.MCjsonDownload(version);
            string html = web.getHtml(mc.Url);
            switch (ExpansionPack)
            {
               case ExpansionPack.Forge :
                    if (ForgeExist(a))
                    {
                        SLC.wj(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + a + @"\" + a + ".json", html);
                        if (d.Length != 0)
                        {
                            SLC.opKeep(a, d[d.Length - 1]);
                        }
                        if (lite)
                        {
                            SLC.liKeep(a);
                        }
                    }
                    else
                    {
                        throw new SikaDeerLauncherException("没有安装Forge");
                    }
                    break;
                case ExpansionPack.Liteloader:
                    if (LiteloaderExist(a))
                    {
                        if (SLC.ForgeKeep(a, html) == false)
                        {
                            SLC.wj(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + a + @"\" + a + ".json", html);
                        }
                        if (d.Length != 0)
                        {
                            SLC.opKeep(a, d[d.Length - 1]);
                        }
                    }
                    else
                    {
                        throw new SikaDeerLauncherException("没有安装Liteloader");
                    }
                    break;
                case ExpansionPack.Optifine:
                    if (OptifineExist(a))
                    {
                        if (SLC.ForgeKeep(a, html) == false)
                        {
                            SLC.wj(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + a + @"\" + a + ".json", html);
                        }
                        if (lite)
                        {
                            SLC.liKeep(a);
                        }
                    }
                    else
                    {
                        throw new SikaDeerLauncherException("没有安装Optifine");
                    }
                    break;
            }
        }
    }
}