using SquareMinecraftLauncher.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SquareMinecraftLauncher.Minecraft
{
    public class AssetDownload
    {
        Process process = new Process();
        int _NumThreads; 
        string _version;
        bool _disposed = false;
        int AllFile = 0;
        int FinishFile = 0;
        public async Task BuildAssetDownload(int NumThreads,string version)
        {
            _NumThreads = NumThreads;
            _version = version;
            if (!_disposed)
            {
                AllFile = new Tools().GetAllTheAsset(version).Length;
                _disposed = true;
            }
            if (FinishFile == AllFile)
            {
                return;
            }
            string Path = System.IO.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\Asset\";
            if (File.Exists(Path + @"\ConsoleApp15.exe"))
            {
                process.StartInfo.FileName = "cmd.exe";
                string Arguments = Path + @"ConsoleApp15.exe " + string.Format("{0} {1} {2}", version, NumThreads, System.Directory.GetCurrentDirectory());
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.CreateNoWindow = false;
                process.Start();
                process.StandardInput.WriteLine(Arguments + "&exit");
                process.StandardInput.AutoFlush = true;
                process.StandardInput.Close();
                Thread thread = new Thread(Process_OutputDataReceived);
                thread.Start();
                await Task.Run(() =>
                {
                    while (true)
                    {
                        if (FinishFile == AllFile)
                        {
                            return;
                        }
                        Thread.Sleep(2000);
                    }
                } );
            }

            GacDownload gac = new GacDownload();
            gac.Download(Path + "yq.zip", "http://www.baibaoblog.cn:81/Asset/Asset.zip");
            while (gac.Complete != 1) Thread.Sleep(500);
            if(File.Exists(Path + "yq.zip"))
            {
                Unzip unzip = new Unzip();
                string a = "";
                unzip.UnZipFile(Path + "yq.zip", Path, out  a);
                BuildAssetDownload(NumThreads,version);
            }

        }

        [field: CompilerGenerated]
        public event DownloadProgressChangedEvent DownloadProgressChanged;
        public delegate void DownloadProgressChangedEvent(DownloadIntermation Log);

        private async void Process_OutputDataReceived()
        {
            StreamReader streamReader = process.StandardOutput ;
            while (!streamReader.EndOfStream)
            {
                string read = await streamReader.ReadLineAsync();
                DownloadIntermation intermation = new DownloadIntermation();
                if (read == "Error")
                {
                    BuildAssetDownload(_NumThreads, _version);
                    return;
                }
                else
                {
                    string[] str = read.Split('|');
                    if (str.Length == 1) continue;
                    intermation.FinishFile = Convert.ToInt32(str[0]);
                    FinishFile = Convert.ToInt32(str[0]); 
                    intermation.AllFile = AllFile;
                    intermation.Progress = (int)(Convert.ToDouble(intermation.FinishFile) * (double)(100) / Convert.ToDouble(AllFile));
                    intermation.Speed = Convert.ToDouble(str[2]);
                    DownloadProgressChanged(intermation);

                }
            }
        }

        public class DownloadIntermation
        {
            /// <summary>
            /// 下载速度
            /// </summary>
            public double Speed { get; internal set; }
            /// <summary>
            /// 进度
            /// </summary>
            public int Progress { get; internal set; }
            /// <summary>
            /// 完成文件
            /// </summary>
            public int FinishFile { get; internal set; }
            /// <summary>
            /// 所有文件
            /// </summary>
            public int AllFile { get; internal set; }
        }
    }
}
