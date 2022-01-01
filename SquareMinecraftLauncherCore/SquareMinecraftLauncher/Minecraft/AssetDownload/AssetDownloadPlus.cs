using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Minecraft
{
    internal class AssetDownloadPlus
    {
        public Task Threads;
        public int EndDownload = 0;
        public int DuckEndDownload = 0;
        public double Speed = 0;
        public double Progress = 0;

        public void StartDownload()
        {
            Threads = Task.Run(DownloadProgress);
        }

        private async void DownloadProgress()
        {
            downloader files = new downloader();
            files.BuildDownload("http://www.baibaoblog.cn:81/Asset/Assets.zip", Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncherDownload\Asset\Assets.zip");
            files.StartDownload();
            await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    int end = 0;
                    Speed = files.intermation.Speed;
                    Progress = files.intermation.Progress;
                    if (files.intermation.StatusId == -1)
                    {
                        end++;
                    }
                    if (files.intermation.Progress == 100)
                    {
                        end++;
                    }

                    if (end == 1)
                    {
                        EndDownload = 1;
                        return;
                    }
                    Thread.Sleep(2000);
                }
            });
        }

        public bool GetEndDownload()
        {
            return EndDownload == 1 ? true : false;
        }
    }
}
