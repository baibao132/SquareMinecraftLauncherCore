using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Minecraft
{
    public class AssetDownload
    {
        Process process = new Process();
        int AllFile = 0;
        int FinishFile = 0;
        AssetDownloadCore gacDownload = null;
        public async Task BuildAssetDownload(int NumThreads, string version)
        {
            if (DownloadProgressChanged == null) throw new SquareMinecraftLauncherException("未对事件实例化，不排除未将事件代码放置改代码前");
            Tools tools = new Tools();
            gacDownload = new AssetDownloadCore(NumThreads, version);
            gacDownload.StartDownload();
            Thread thread = new Thread(Process_OutputDataReceived);
            thread.Start();
            await Task.Run(() => { while (!gacDownload.GetEndDownload()) Thread.Sleep(3000); });
        }

        [field: CompilerGenerated]
        public event DownloadProgressChangedEvent DownloadProgressChanged;
        public delegate void DownloadProgressChangedEvent(DownloadIntermation Log);

        private void Process_OutputDataReceived()
        {
            DownloadIntermation intermation = new DownloadIntermation();
            while (!gacDownload.GetEndDownload())
            {
                intermation.FinishFile = gacDownload.EndDownload;
                intermation.AllFile = AllFile;
                intermation.Progress = gacDownload.Progress;
                intermation.Speed = gacDownload.Speed;
                DownloadProgressChanged(intermation);
                Console.WriteLine(gacDownload.EndDownload + "|" + Math.Round(gacDownload.Progress, 1) + "|" + Math.Round(gacDownload.Speed, 1));
                Thread.Sleep(1000);
            }
            intermation.FinishFile = FinishFile;
            intermation.AllFile = AllFile;
            intermation.Progress = 100;
            intermation.Speed = 2;
            DownloadProgressChanged(intermation);
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
            public double Progress { get; internal set; }
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
