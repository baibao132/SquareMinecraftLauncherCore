using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SikaDeerLauncher.Minecraft
{
    public sealed class Game
    {
        Tools tools = new Tools();
        SikaDeerLauncher.Core.SikaDeerLauncherCore SLC = new Core.SikaDeerLauncherCore();
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
                    MCDownload[] Lib = tools.GetTheExistingLibrary(version);
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
                        for (int i = 2; i < arg.Length; i +=2)
                        {
                            a += " " + arg[i] + " " + arg[i + 1];
                        }
                        mA = a.Split(' ');
                    }
                    var jo2 = SLC.versionjson<json4.Root>(version);
                    string main = null;
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
                    if (RearParameter == "" || RearParameter == null)
                    {
                        Game += " " + jo.mainClass + main;
                    }
                    else
                    {
                        Game += " " + jo.mainClass + main + " " + RearParameter;
                    }
                    Console.WriteLine("\n\n\n\n\n\n" + Game);
                    ProcessStartInfo start = new ProcessStartInfo(java);
                    start.Arguments = Game;//设置命令参数
                    start.CreateNoWindow = true;//不显示dos命令行窗口
                    start.RedirectStandardOutput = true;//
                    start.RedirectStandardInput = true;//
                    start.UseShellExecute = false;
                    process = Process.Start(start);
                    LogEvent += new LogDel(errorORLog);
                    Thread thread = new Thread(new ThreadStart(monitoring));
                    thread.IsBackground = true;
                    thread.Start();
                }
                else
                {
                    throw new SikaDeerLauncherException("java路径不对");
                }
            }
            else
            {
                throw new SikaDeerLauncherException("缺少Natives");
            }
        }
        Process process = new Process();
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java路径</param>
        /// <param name="username">邮箱</param>
        /// <param name="password">密码</param>
        /// <param name="RAM">内存</param>
        /// <param name="JVMparameter">JVM参数</param>
        /// <param name="RearParameter">后置参数</param>
        public void StartGame(string version, string java, int RAM, string username, string password, string JVMparameter, string RearParameter)
        {
            Getlogin getlogin = null;
            try
            {
                getlogin = tools.MinecraftLogin(username, password);
            }
            catch (SikaDeerLauncherException ex)
            {
                throw new SikaDeerLauncherException("启动失败，正版登录" + ex.Message);
            }
            StartGame(version, java, RAM, getlogin.name, getlogin.uuid, getlogin.token, JVMparameter, RearParameter);
        }
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java路径</param>
        /// <param name="name">游戏名</param>
        /// <param name="RAM">内存</param>
        /// <param name="JVMparameter">JVM参数</param>
        /// <param name="RearParameter">后置参数</param>
        public void StartGame(string version, string java, int RAM, string name, string JVMparameter, string RearParameter)
        {
            StartGame(version, java, RAM, name, SLC.uuid(name), SLC.token(), JVMparameter, RearParameter);
        }
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java路径</param>
        /// <param name="name">游戏名</param>
        /// <param name="RAM">内存</param>
        public void StartGame(string version, string java, int RAM, string name)
        {
            StartGame(version, java, RAM, name, "", "");
        }
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java路径</param>
        /// <param name="username">邮箱</param>
        /// <param name="password">密码</param>
        /// <param name="RAM">内存</param>
        public void StartGame(string version, string java, int RAM, string username, string password)
        {
            StartGame(version, java, RAM, username, password, "", "");
        }
        /// <summary>
        /// （外置登录）启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java路径</param>
        /// <param name="name">游戏名</param>
        /// <param name="uuid">uuid</param>
        /// <param name="token">token</param>
        /// <param name="RAM">内存</param>
        /// <param name="JVMparameter">JVM参数</param>
        /// <param name="RearParameter">后置参数</param>
        /// <param name="yggdrasilURLORID">yggdrasil地址或统一通行证ID</param>
        public void StartGame(string version, string java, int RAM, string name, string uuid, string token, string yggdrasilURLORID, string JVMparameter, string RearParameter, AuthenticationServerMode authentication)
        {
            Download download = new Download();
            if (authentication == AuthenticationServerMode.yggdrasil)
            {
                SLC.SetFile(@"SikaDeerLauncher");
                byte[] res = new byte[Properties.Resources.SDL.Length];
                Properties.Resources.SDL.CopyTo(res, 0);
                FileStream fs = new FileStream(@"SikaDeerLauncher\yggdrasilSikaDeerLauncher.jar", FileMode.Create, FileAccess.Write);
                fs.Write(res, 0, res.Length);
                fs.Close();
                string url = download.getHtml(yggdrasilURLORID);
                if (url == null)
                {
                    throw new SikaDeerLauncherException("启动失败，无法获取相关信息");
                }
                byte[] bytes = Encoding.Default.GetBytes(url);
                string jvm = "-javaagent:" + System.IO.Directory.GetCurrentDirectory() + @"\SikaDeerLauncher\yggdrasilSikaDeerLauncher.jar=" + yggdrasilURLORID + " -Dauthlibinjector.side=client -Dauthlibinjector.yggdrasil.prefetched=" + Convert.ToBase64String(bytes);
                if (JVMparameter != null && JVMparameter != "")
                {
                    jvm += "," + JVMparameter;
                }
                StartGame(version, java, RAM, name, uuid, token, jvm, RearParameter);
            }
            else
            {
                SLC.SetFile(@"SikaDeerLauncher");
                byte[] res = new byte[Properties.Resources.nide8auth.Length];
                Properties.Resources.nide8auth.CopyTo(res, 0);
                FileStream fs = new FileStream(@"SikaDeerLauncher\UnifiedPassSikaDeerLauncher.jar", FileMode.Create, FileAccess.Write);
                fs.Write(res, 0, res.Length);
                fs.Close();
                string jvm = "-javaagent:" + System.IO.Directory.GetCurrentDirectory() + @"\SikaDeerLauncher\UnifiedPassSikaDeerLauncher.jar=" + yggdrasilURLORID;
                if (JVMparameter != null && JVMparameter != "")
                {
                    jvm += "," + JVMparameter;
                }
                StartGame(version, java, RAM, name, uuid, token, jvm, RearParameter);
            }
        }
        public delegate void LogDel(Log Log);
        /// <summary>
        /// Log事件
        /// </summary>
        public event LogDel LogEvent;
        public delegate void ErrorDel(Error error);
        /// <summary>
        /// 退出事件
        /// </summary>
        public event ErrorDel ErrorEvent;
        public class Log : EventArgs
        {
            private string message;
            internal Log(string Message)
            {
                this.message = Message;
            }
            public string Message
            {
                get { return message; }
            }

        }
        internal void monitoring()
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            StreamReader reader = process.StandardOutput;//截取输出流
            string line = reader.ReadLine();//每次读取一行
            while (!reader.EndOfStream)
            {
                Log error = new Log(reader.ReadLine());
                if (LogEvent != null)
                {
                    LogEvent(error);
                }
            }
            process.WaitForExit();//等待程序执行完退出进程
            process.Close();//关闭进程
            reader.Close();//关闭流
        }
        public class Error : EventArgs
        {
            SikaDeerLauncher.Core.SikaDeerLauncherCore SLC = new Core.SikaDeerLauncherCore();
            private string message;
            internal Error(string Message)
            {
                string a = SLC.Replace(Message, "[Client thread/ERROR]", " ");
                if (a != null && a != Message)
                {
                    this.message = Message;
                }
            }
            public string Message
            {
                get { return message; }
            }
        }
        internal void errorORLog(Log log)
        {
            if (ErrorEvent != null)
            {
                Error e = new Error(log.Message);
                ErrorEvent(e);
            }
        }
    }
}
