namespace SikaDeerLauncher.Minecraft
{
    using json4;
    using main;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SikaDeerLauncher;
    using SikaDeerLauncher.Core;
    using SikaDeerLauncher.Properties;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using windows;

    public sealed class Game
    {
        private Process process = new Process();
        private SikaDeerLauncherCore SLC = new SikaDeerLauncherCore();
        private ProcessStartInfo start = new ProcessStartInfo();
        private Tools tools = new Tools();

        [field: CompilerGenerated]
        public event ErrorDel ErrorEvent;

        [field: CompilerGenerated]
        public event LogDel LogEvent;

        internal void errormonitoring()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            StreamReader standardError = this.process.StandardError;
            string sE = standardError.ReadToEnd();
            if (sE != "")
            {
                Error error = new Error(null, sE);
                if (this.ErrorEvent != null)
                {
                    this.ErrorEvent(error);
                }
                else
                {
                    standardError.Close();
                }
            }
            try
            {
                this.process.WaitForExit();
                standardError.Close();
                this.process.Close();
            }
            catch (Exception)
            {
            }
        }

        internal void monitoring()
        {
            try
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                StreamReader standardOutput = this.process.StandardOutput;
                while (!standardOutput.EndOfStream)
                {
                    string message = standardOutput.ReadLine();
                    if (WinAPI.GetHandle("LWJGL") == null && WinAPI.GetHandle("GLFW30") == null)
                    {
                        standardOutput.Close();
                        this.process.Close();
                    }
                    if (this.LogEvent != null)
                    {
                        this.LogEvent(new Log(message));
                    }
                    string str2 = this.SLC.Replace(message, "Exception", " ");
                    if ((str2 != null) && (str2 != message))
                    {
                        if (windows.WinAPI.GetHandle("LWJGL") == (IntPtr)0 && windows.WinAPI.GetHandle("GLFW30") == (IntPtr)0)
                        {

                            if (this.ErrorEvent != null)
                            {
                                this.ErrorEvent(new Error(null, standardOutput.ReadToEnd()));
                            }
                        }
                    }
                    if (this.ErrorEvent != null)
                    {
                        this.ErrorEvent(new Error(message, null));
                    }
                }
                this.process.WaitForExit();
                standardOutput.Close();
                this.process.Close();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="name">游戏名</param>
        public void StartGame(string version, string java, int RAM, string name)
        {
            this.StartGame(version, java, RAM, name, "", "");
        }
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="username">邮箱</param>
        /// <param name="password">密码</param>
        public void StartGame(string version, string java, int RAM, string username, string password)
        {
            this.StartGame(version, java, RAM, username, password, "", "");
        }
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="name">游戏名</param>
        /// <param name="JVMparameter">前置参数</param>
        /// <param name="RearParameter">后置参数</param>
        public void StartGame(string version, string java, int RAM, string name, string JVMparameter, string RearParameter)
        {
            this.StartGame(version, java, RAM, name, this.SLC.uuid(name), this.SLC.token(), JVMparameter, RearParameter);
        }
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="username">邮箱</param>
        /// <param name="password">密码</param>
        /// <param name="JVMparameter">前置参数</param>
        /// <param name="RearParameter">后置参数</param>
        public void StartGame(string version, string java, int RAM, string username, string password, string JVMparameter, string RearParameter)
        {
            Getlogin getlogin = null;
            try
            {
                getlogin = this.tools.MinecraftLogin(username, password);
            }
            catch (SikaDeerLauncherException exception)
            {
                throw new SikaDeerLauncherException("启动失败，正版登录" + exception.Message);
            }
            this.StartGame(version, java, RAM, getlogin.name, getlogin.uuid, getlogin.token, JVMparameter, RearParameter);
        }
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="name">游戏名</param>
        /// <param name="uuid">uuid</param>
        /// <param name="token">token</param>
        /// <param name="JVMparameter">前置参数</param>
        /// <param name="RearParameter">后置参数</param>
        public void StartGame(string version, string java, int RAM, string name, string uuid, string token, string JVMparameter, string RearParameter)
        {
            if (version == "" || version == null || java == "" || java == null || name == null || name == "" || uuid == "" || uuid == null || token == "" || token == null || RAM == 0)
            {
                throw new SikaDeerLauncherException("任何一项都不能为空");
            }
            string Game = null;
            Tools tools = new Tools();
            if (SLC.FileExist(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar") != null)
            {
                throw new SikaDeerLauncherException("启动失败,未找到该版本");
            }
            //download[] MLib = tools.GetMissingLibrary(version);
            //if (MLib.Length != 0)
            //{
            //    throw new SikaDeerLauncherException("缺少Libraries文件");
            //}
            MCDownload[] natives1 = new MCDownload[0];
            MCDownload[] natives = tools.GetMissingNatives(version);
            if (natives.Length == 0)
            {
                natives1 = tools.GetAllNatives(version);
                if (natives1.Length == 0)
                {
                    throw new SikaDeerLauncherException("Natives获取失败，请检查文件是否完整");
                }
                string nativespath = SLC.nativeszip(version);
                if (SLC.FileExist(java) != "")
                {
                    string bx = "-Xincgc -Xmx" + RAM + "M  -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true ";
                    if (JVMparameter == "" || JVMparameter == null)
                    {
                        Game = bx + "-Djava.library.path=\"" + nativespath + "\" -cp ";
                    }
                    else
                    {
                        Game = bx + JVMparameter + " -Djava.library.path=\"" + nativespath + "\" -cp ";
                    }
                    if (tools.GetMissingLibrary(version).Length != 0)
                    {
                        throw new SikaDeerLauncherException("Libraries文件不完整");
                    }
                    MCDownload[] Lib = tools.GetAllLibrary(version);
                    string Libname = "\"";
                    foreach (var Libname1 in Lib)
                    {
                        Libname += Libname1.path + ";";
                    }
                    Game += Libname + System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar\"";
                    var jo = SLC.versionjson<main.mainclass>(version);
                    string[] mA = null;
                    if (jo.minecraftArguments != null)
                    {
                        mA = jo.minecraftArguments.Split(' ');
                        SLC.IMG(version);
                    }
                    else
                    {
                        StreamReader sr;
                        try
                        {
                            sr = new StreamReader(System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json", Encoding.Default);
                        }
                        catch (System.IO.DirectoryNotFoundException ex)
                        {
                            throw new SikaDeerLauncherException("未找到该版本");
                        }
                        var c = (JObject)JsonConvert.DeserializeObject(sr.ReadToEnd());
                        List<string> jo3 = new List<string>();
                        for (int i = 1; c["arguments"]["game"].ToArray().Length - 1 > 0; i += 2)
                        {
                            try
                            {

                                c["arguments"]["game"][i].ToString();
                                if (c["arguments"]["game"][i - 1].ToString()[0] == '-' || c["arguments"]["game"][i].ToString()[0] == '$')
                                {
                                    jo3.Add(c["arguments"]["game"][i - 1].ToString());
                                    jo3.Add(c["arguments"]["game"][i].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                break;
                            }
                        }
                        string[] arg = jo3.ToArray();
                        string a = arg[0] + " " + arg[1];
                        for (int i = 2; i < arg.Length; i += 2)
                        {
                            a += " " + arg[i] + " " + arg[i + 1];
                        }
                        mA = a.Split(' ');
                    }
                    var jo2 = SLC.versionjson<json4.Root>(version);
                    string main = " --width 854 --height 480";
                    for (int i = 0; mA.Length > i; i += 2)
                    {
                        switch (mA[i])
                        {
                            case "--username":
                                main += " " + mA[i] + " \"" + name + "\"";
                                break;
                            case "--version":
                                main += " " + mA[i] + " \"" + jo2.id + "\"";
                                break;
                            case "--gameDir":
                                main += " " + mA[i] + " \"" + System.IO.Directory.GetCurrentDirectory() + @"\.minecraft" + "\"";
                                break;
                            case "--assetsDir":
                                main += " " + mA[i] + " \"" + System.IO.Directory.GetCurrentDirectory() + @"\.minecraft\assets" + "\"";
                                break;
                            case "--assetIndex":
                                main += " " + mA[i] + " " + jo2.assets;
                                break;
                            case "--uuid":
                                main += " " + mA[i] + " " + uuid;
                                break;
                            case "--accessToken":
                                main += " " + mA[i] + " " + token;
                                break;
                            case "--userType":
                                main += " " + mA[i] + " " + "Legacy";
                                break;
                            case "--versionType":
                                main += " " + mA[i] + " " + "\"SikaDeerLauncher - BaiBao\"";
                                break;
                            case "--userProperties":
                                main += " " + mA[i] + " " + "{}";
                                break;
                            default:
                                main += " " + mA[i] + " " + mA[i + 1];
                                break;

                        }
                    }
                    if ((RearParameter == "") || (RearParameter == null))
                    {
                        Game = Game + " " + jo.mainClass + main;
                    }
                    else
                    {
                        string[] textArray14 = new string[] { Game, " ", jo.mainClass, main, " ", RearParameter };
                        Game = string.Concat(textArray14);
                    }
                    Console.WriteLine("\n\n\n\n\n\n" + Game);
                    this.start.FileName = java;
                    this.start.Arguments = Game;
                    this.start.CreateNoWindow = true;
                    this.start.RedirectStandardOutput = true;
                    this.start.RedirectStandardInput = true;
                    this.start.UseShellExecute = false;
                    this.start.RedirectStandardError = true;
                    Thread thread1 = new Thread(new ThreadStart(this.monitoring))
                    {
                        IsBackground = true
                    };
                    Thread thread2 = new Thread(new ThreadStart(this.errormonitoring))
                    {
                        IsBackground = true
                    };
                    this.process = Process.Start(this.start);
                    thread2.Start();
                    thread1.Start();
                }
            }
        }
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="name">游戏名</param>
        /// <param name="uuid">uuid</param>
        /// <param name="token">token</param>
        /// <param name="yggdrasilURLORID">ID</param>
        /// <param name="JVMparameter">JVM</param>
        /// <param name="RearParameter">后置参数</param>
        /// <param name="authentication">验证方式</param>
        public void StartGame(string version, string java, int RAM, string name, string uuid, string token, string yggdrasilURLORID, string JVMparameter, string RearParameter, AuthenticationServerMode authentication)
        {
            string str3 = null;
            Download download = new Download();
            if (authentication == AuthenticationServerMode.yggdrasil)
            {
                if (SLC.FileExist(Directory.GetCurrentDirectory() + @"\SikaDeerLauncher\yggdrasilSikaDeerLauncher.jar") != null)
                {
                    this.SLC.SetFile("SikaDeerLauncher");
                    GacDownload.Download(Directory.GetCurrentDirectory() + @"\SikaDeerLauncher\yggdrasilSikaDeerLauncher.jar", "https://bmclapi2.bangbang93.com/mirrors/authlib-injector/artifact/26/authlib-injector-1.1.26-41a7a47.jar");
                    while (true)
                    {
                        if (GacDownload.Complete == 0 && GacDownload.Failure == 1)
                        {
                            throw new SikaDeerLauncherException("yggdrasil下载失败");
                        }
                        else if (GacDownload.Complete == 1)
                        {
                            break;
                        }
                        ForgeInstallCore.Delay(2000);
                    }
                }
                string s = download.getHtml(yggdrasilURLORID);
                if (s == null)
                {
                    throw new SikaDeerLauncherException("启动失败，无法获取相关信息");
                }
                byte[] bytes = Encoding.Default.GetBytes(s);
                string[] textArray1 = new string[] { "-javaagent:", Directory.GetCurrentDirectory(), @"\SikaDeerLauncher\yggdrasilSikaDeerLauncher.jar=", yggdrasilURLORID, " -Dauthlibinjector.side=client -Dauthlibinjector.yggdrasil.prefetched=", Convert.ToBase64String(bytes) };
                str3 = string.Concat(textArray1);
            }
            else
            {
                if (SLC.FileExist(Directory.GetCurrentDirectory() + @"\SikaDeerLauncher\UnifiedPassSikaDeerLauncher.jar") != null)
                {
                    this.SLC.SetFile("SikaDeerLauncher");
                    GacDownload.Download(Directory.GetCurrentDirectory() + @"\SikaDeerLauncher\UnifiedPassSikaDeerLauncher.jar", "https://login2.nide8.com:233/download/nide8auth.jar");
                    while (true)
                    {
                        if (GacDownload.Complete == 0 && GacDownload.Failure == 1)
                        {
                            throw new SikaDeerLauncherException("统一通行证下载失败");
                        }
                        else if (GacDownload.Complete == 1)
                        {
                            break;
                        }
                        ForgeInstallCore.Delay(2000);
                    }
                }
                str3 = "-javaagent:" + Directory.GetCurrentDirectory() + @"\SikaDeerLauncher\UnifiedPassSikaDeerLauncher.jar=" + yggdrasilURLORID;
            }
            if ((JVMparameter != null) && (JVMparameter != ""))
                {
                    str3 = str3 + "," + JVMparameter;
                }
                this.StartGame(version, java, RAM, name, uuid, token, str3, RearParameter);
        }

        public class Error : EventArgs
        {
            private string message;
            private string SE;
            private SikaDeerLauncherCore SLC = new SikaDeerLauncherCore();

            internal Error(string Message, string SE)
            {
                if ((Message == null) && (SE != null))
                {
                    this.SE = SE;
                }
                else
                {
                    string str = this.SLC.Replace(Message, "ERROR", " ");
                    if ((str != null) && (str != Message))
                    {
                        this.message = Message;
                    }
                }
            }

            public string Message
            {
                get
                {
                    return this.message;
                }
            }

            public string SeriousError
            {
                get
                {
                    return this.SE;
                }
            }
        }

        public delegate void ErrorDel(Game.Error error);

        public class Log : EventArgs
        {
            private string message;

            internal Log(string Message)
            {
                this.message = Message;
            }

            public string Message
            {
                get
                {
                    return this.message;
                }
            }
        }

        public delegate void LogDel(Game.Log Log);
    }
}

