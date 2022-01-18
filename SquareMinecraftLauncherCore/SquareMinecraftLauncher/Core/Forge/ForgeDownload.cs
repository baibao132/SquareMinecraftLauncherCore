using SquareMinecraftLauncher.Minecraft;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core
{
    internal class ForgeDownload
    {
        Task[] Threads = new Task[0];
        MCDownload[] download = new MCDownload[0];
        public int EndDownload = 0;
        public int DuckEndDownload = 0;
        public double Speed = 0;
        public double Progress = 0;
        public int error = 0;
        public ForgeDownload(int thread, MCDownload[] downloads)
        {
            Threads = new Task[thread];
            download = downloads;
        }

        public ForgeDownload(MCDownload[] downloads)
        {
            Threads = new Task[3];
            download = downloads;
        }

        int ADindex = 0;
        private MCDownload AssignedDownload()
        {
            if (ADindex >= download.Length) return null;
            ADindex++;
            try
            {
                return download[ADindex - 1];
            }
            catch (Exception) { }
            return null;
        }

        public void StartDownload()
        {
            DuckEndDownload = 1;
            Thread thread = new Thread(DownloadProgressPlus);
            thread.Start();
        }

        private void DownloadProgressPlus()
        {
            Console.WriteLine("pass");
            DuckEndDownload = 0;
            for (int i = 0; i < Threads.Length; i++)
            {
                Threads[i] = Task.Run(DownloadProgress);
                // Threads[i].Start();//启动线程
            }
        }

        private async void DownloadProgress()
        {
            List<downloader> files = new List<downloader>();
            for (int i = 0; i < 10; i++)
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
                            error++;
                        }
                        if (files[i].intermation.Progress == 100)
                        {
                            end++;
                        }
                    }

                    if (end >= files.Count)
                    {
                        DuckEndDownload += files.Count;
                        Speed = 1;
                        Progress += 100 / (double)(download.Length) * files.Count;
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
