using Downloader;
using System;
using System.ComponentModel;
using System.Net;
using System.Reflection;

namespace SquareMinecraftLauncher.Minecraft
{
    internal class downloader
    {
        DownloadService _downloader;
        public DownloadIntermation intermation = new DownloadIntermation();
        public downloader()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 5;
            var downloadOpt = new DownloadConfiguration()
            {
                BufferBlockSize = 10240, // usually, hosts support max to 8000 bytes, default values is 8000
                ChunkCount = 1, // file parts to download, default value is 1
                MaximumBytesPerSecond = 10240 * 10240, // download speed limited to 1MB/s, default values is zero or unlimited
                MaxTryAgainOnFailover = int.MaxValue, // the maximum number of times to fail
                OnTheFlyDownload = false, // caching in-memory or not? default values is true
                ParallelDownload = false, // download parts of file as parallel or not. Default value is false
                TempDirectory = "C:\\temp", // Set the temp path for buffering chunk files, the default path is Path.GetTempPath()
                Timeout = 1000, // timeout (millisecond) per stream block reader, default values is 1000
                RequestConfiguration = // config and customize request headers
                {
                    Accept = "*/*",
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    CookieContainer =  new CookieContainer(), // Add your cookies
                    Headers = new WebHeaderCollection(), // Add your custom headers
                    KeepAlive = false,
                    ProtocolVersion = HttpVersion.Version11, // Default value is HTTP 1.1
                    UseDefaultCredentials = false,
                    UserAgent = $"DownloaderSample/{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}"
                }
            };
            _downloader = new DownloadService(downloadOpt);
            // Provide any information about download progress, like progress percentage of sum of chunks, total speed, average speed, total received bytes and received bytes array to live streaming.
            _downloader.DownloadProgressChanged += OnDownloadProgressChanged;
            // Download completed event that can include occurred errors or cancelled or download completed successfully.
            _downloader.DownloadFileCompleted += OnDownloadFileCompleted;
        }

        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                intermation.Status = e.Error.Message;
                intermation.StatusId = -1;
            }
        }

        private void OnDownloadProgressChanged(object sender, Downloader.DownloadProgressChangedEventArgs e)
        {
            intermation.Speed = Math.Round(e.BytesPerSecondSpeed / 512 / 1024, 1);
            intermation.Progress = (int)e.ProgressPercentage;
        }

        public async void StartDownload()
        {
            await _downloader.DownloadFileTaskAsync(Url, FilePath);
            intermation.Status = "下载中";
            intermation.StatusId = 1;
        }
        string Url, FilePath;
        public void BuildDownload(string Url, string FilePath)
        {
            this.Url = Url;
            this.FilePath = FilePath;
            intermation.StatusId = 0;
        }
    }

    public class DownloadIntermation
    {

        public string Status { get; internal set; }
        public int StatusId { get; internal set; }
        public double Speed { get; internal set; }
        public int Progress { get; internal set; }
        public string FileName { get; internal set; }
    }
}
