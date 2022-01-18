using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Minecraft
{
    internal class AssetDownloadCore
    {
        Task[] Threads = new Task[0];
        MCDownload[] download = new MCDownload[0];
        public int EndDownload = 0;
        public int DuckEndDownload = 0;
        public double Speed = 0;
        public double Progress = 0;
        Tools tools = new Tools();
        string Version = "";
        AssetDownloadPlus assetDownloadPlus = new AssetDownloadPlus();
        public AssetDownloadCore(int thread, string version)
        {
            Threads = new Task[thread];
            Version = version;
        }

        public AssetDownloadCore(string version)
        {
            Threads = new Task[3];
            Version = version;
        }

        int ADindex = 0;
        private MCDownload AssignedDownload()
        {
            if (ADindex == download.Length) return null;
            ADindex++;
            return download[ADindex - 1];
        }

        public void StartDownload()
        {
            DuckEndDownload = 1;
            Thread thread = new Thread(DownloadProgressPlus);
            thread.Start();
        }

        private async void DownloadProgressPlus()
        {
            this.download = tools.GetMissingAsset(Version);
            if (download.Length > 900)
            {
                assetDownloadPlus.StartDownload();
                await Task.Factory.StartNew(() =>
                {
                    while (!assetDownloadPlus.GetEndDownload())
                    {
                        Speed = assetDownloadPlus.Speed;
                        Progress = assetDownloadPlus.Progress / 2;
                        Thread.Sleep(2000);
                    }
                });
                Unzip unzip = new Unzip();
                string Out = "";
                unzip.UnZipFile(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncherDownload\Asset\Assets.zip", System.Directory.GetCurrentDirectory() + @"\.minecraft\assets\", out Out);
                Thread.Sleep(4000);
            }
            Console.WriteLine("pass");
            tools.DownloadSourceInitialization(DownloadSource.MCBBSSource);
            Progress = 50;
            DuckEndDownload = 0;
            download = tools.GetMissingAsset(Version);
            EndDownload = tools.GetAllTheAsset(Version).Length - download.Length;
            for (int i = 0; i < Threads.Length; i++)
            {
                Threads[i] = Task.Run(DownloadProgress);
                // Threads[i].Start();//启动线程
            }
        }

        private async void DownloadProgress()
        {
            List<downloader> files = new List<downloader>();
            for (int i = 0; i < 5; i++)
            {
                MCDownload download = AssignedDownload();
                if (download != null)
                {
                    downloader fileDownloader = new downloader();
                    fileDownloader.BuildDownload(download.Url, download.path);
                    fileDownloader.StartDownload();
                    files.Add(fileDownloader);
                }
            }
            if (files.Count == 0)
            {
                Progress = 100;
                DuckEndDownload = download.Length;
                return;
            }
            await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    int end = 0;
                    for (int i = 0; i < files.Count; i++)
                    {
                        Speed += files[i].intermation.Speed;
                        if (files[i].intermation.StatusId == -1)
                        {
                            end++;
                        }
                        if (files[i].intermation.Progress == 100)
                        {
                            end++;
                        }
                    }

                    if (end >= files.Count)
                    {
                        DuckEndDownload += files.Count;
                        EndDownload += files.Count;
                        Speed = 1;
                        Progress += 50 / (double)(download.Length) * files.Count;
                        DownloadProgress();//递归

                        return;
                    }
                    Thread.Sleep(200);
                }
            });
        }

        public bool GetEndDownload()
        {
            return DuckEndDownload == download.Length ? true : false;
        }
    }
}
