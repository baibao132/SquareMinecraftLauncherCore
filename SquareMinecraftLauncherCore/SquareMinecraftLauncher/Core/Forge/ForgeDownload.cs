using Gac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core
{
    internal class ForgeDownload
    {
            Thread[] Threads = new Thread[0];
            Minecraft.MCDownload[] download = new Minecraft.MCDownload[0];
            int EndDownload = 0;
            public ForgeDownload(int thread, Minecraft.MCDownload[] download)
            {
                Threads = new Thread[thread];
                this.download = download;
            }

            public ForgeDownload(Minecraft.MCDownload[] download)
            {
                Threads = new Thread[3];
                this.download = download;
            }

            int ADindex = 0;
            private Minecraft.MCDownload AssignedDownload()
            {
                if (ADindex == download.Length) return null;
                ADindex++;
                return download[ADindex - 1];
            }

            public void StartDownload()
            {
                for (int i = 0; i < Threads.Length; i++)
                {
                    Threads[i] = new Thread(DownloadProgress);
                    Threads[i].IsBackground = true;
                    Threads[i].Start();//启动线程
                }
            }
        public int error = 0;
            private async void DownloadProgress()
            {
                List<FileDownloader> files = new List<FileDownloader>();
                for (int i = 0; i < 3; i++)
                {
                    Minecraft.MCDownload download = AssignedDownload();//分配下载任务
                    try
                    {
                        if (download != null)
                        {
                            FileDownloader fileDownloader = new FileDownloader(download.Url, download.path.Replace(System.IO.Path.GetFileName(download.path), ""), System.IO.Path.GetFileName(download.path)+".Square");//增加下载
                            fileDownloader.download(null);
                            files.Add(fileDownloader);
                        }

                    }
                    catch (Exception ex)//当出现下载失败时，忽略该文件
                    {
                        error++;
                        Console.WriteLine(ex.Message);
                    }
                }
                if (files.Count == 0) return;
                await Task.Factory.StartNew(() =>
                {
                    while (true)//循环检测当前线程files.Count个下载任务是否下载完毕
                    {
                        int end = 0;
                        for (int i = 0; i < files.Count; i++)
                        {
                            if (files[i].download(null) == files[i].getFileSize())
                            {
                                end++;
                            }
                            Console.WriteLine(files[i].download(null) + " from " + files[i].getFileSize());
                        }
                        Console.WriteLine(EndDownload);

                        if (end == files.Count)//完成则递归当前函数
                        {
                            EndDownload += files.Count;
                            DownloadProgress();//递归
                            return;
                        }
                        Thread.Sleep(1000);
                    }
                });
            }

            public bool GetEndDownload()
            {
                return EndDownload == download.Length ? true : false;
            }
    }
}
