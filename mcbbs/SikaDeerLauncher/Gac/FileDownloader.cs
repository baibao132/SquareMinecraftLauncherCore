using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;

namespace Gac
{
    public class FileDownloader
    {
        /// <summary>
        /// 已下载文件长度
        /// </summary>
        private long downloadSize = 0;
        /// <summary>
        /// 原始文件长度
        /// </summary>
        private long fileSize = 0;
        /// <summary>
        /// 线程数
        /// </summary>
        private DownloadThread[] threads;
        /// <summary>
        /// 本地保存文件
        /// </summary>
        private string saveFile;
        /// <summary>
        /// 缓存各线程下载的长度
        /// </summary>
        public Dictionary<int, long> data = new Dictionary<int, long>();
        /// <summary>
        /// 每条线程下载的长度
        /// </summary>
        private long block;
        /// <summary>
        /// 下载路径
        /// </summary>
        private String downloadUrl;
        /// <summary>
        ///  获取线程数
        /// </summary>
        /// <returns> 获取线程数</returns>
        public int getThreadSize()
        {
            return threads.Length;
        }
        /// <summary>
        ///   获取文件大小
        /// </summary>
        /// <returns>获取文件大小</returns>
        public long getFileSize()
        {
            return fileSize;
        }
        /// <summary>
        /// 累计已下载大小
        /// </summary>
        /// <param name="size">累计已下载大小</param>
        public void append(long size)
        {
            lock (this)  //锁定同步..............线程开多了竟然没有同步起来.文件下载已经完毕了,下载总数量却不等于文件实际大小,找了半天原来这里错误的
            {
                downloadSize += size;
            }

        }
        /// <summary>
        /// 更新指定线程最后下载的位置
        /// </summary>
        /// <param name="threadId">threadId 线程id</param>
        /// <param name="pos">最后下载的位置</param>
        public void update(int threadId, long pos)
        {
            if (data.ContainsKey(threadId))
            {
                this.data[threadId] = pos;
            }
            else
            {
                this.data.Add(threadId, pos);
            }
        }

        /// <summary>
        /// 构建下载准备,获取文件大小
        /// </summary>
        /// <param name="downloadUrl">下载路径</param>
        /// <param name="fileSaveDir"> 文件保存目录</param>
        /// <param name="threadNum">下载线程数</param>
        public FileDownloader(string downloadUrl, string fileSaveDir,string filename="", int threadNum=3)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                {
                     filename = Uri.UnescapeDataString(Path.GetFileName(downloadUrl));//获取文件名称 uri 解码中文字符
                }
                //构建http 请求
                this.downloadUrl = downloadUrl;
                if (!Directory.Exists(fileSaveDir)) Directory.CreateDirectory(fileSaveDir);
                this.threads = new DownloadThread[threadNum];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(downloadUrl);
                request.Referer = downloadUrl.ToString();
                request.Method = "GET";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 2.0.1124)";
                request.ContentType = "application/octet-stream";
                request.Accept = "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                request.Timeout = 20 * 1000;
                request.AllowAutoRedirect = true;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        this.fileSize = response.ContentLength;//根据响应获取文件大小
                        if (this.fileSize <= 0) throw new Exception("获取文件大小失败");
                        
                        if (filename.Length == 0) throw new Exception("获取文件名失败");
                        this.saveFile = Path.Combine(fileSaveDir, filename); //构建保存文件 
                        //计算每条线程下载的数据长度
                        this.block = (this.fileSize % this.threads.Length) == 0 ? this.fileSize / this.threads.Length : this.fileSize / this.threads.Length + 1;
                    }
                    else
                    {
                        throw new Exception("服务器返回状态失败,StatusCode:" + response.StatusCode);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("无法连接下载地址");
            }
        }

        /// <summary>
        /// 开始下载文件
        /// </summary>
        /// <param name="listener">监听下载数量的变化,如果不需要了解实时下载的数量,可以设置为null</param>
        /// <returns>已下载文件大小</returns>
        public long download(IDownloadProgressListener listener)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                using (FileStream fstream = new FileStream(this.saveFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    if (this.fileSize > 0) fstream.SetLength(this.fileSize);
                    fstream.Close();
                }
                if (this.data.Count != this.threads.Length)
                {
                    this.data.Clear();
                    for (int i = 0; i < this.threads.Length; i++)
                    {
                        this.data.Add(i + 1, 0);//初始化每条线程已经下载的数据长度为0
                    }
                }
                for (int i = 0; i < this.threads.Length; i++)
                {//开启线程进行下载
                    long downLength = this.data[i + 1];
                    if (downLength < this.block && this.downloadSize < this.fileSize)
                    {//判断线程是否已经完成下载,否则继续下载	+

                        // Console.WriteLine("threads" + i.ToString() + ",下载块" + this.block.ToString() + "    " + this.data[i + 1].ToString() + "              " + downloadSize.ToString());
                        this.threads[i] = new DownloadThread(this, downloadUrl, this.saveFile, this.block, this.data[i + 1], i + 1);
                        this.threads[i].ThreadRun();

                    }
                    else
                    {
                        this.threads[i] = null;
                    }
                }
                bool notFinish = true;//下载未完成
                while (notFinish)
                {// 循环判断所有线程是否完成下载
                    Thread.Sleep(900);
                    notFinish = false;//假定全部线程下载完成
                    for (int i = 0; i < this.threads.Length; i++)
                    {
                        if (this.threads[i] != null && !this.threads[i].isFinish())
                        {//如果发现线程未完成下载
                            notFinish = true;//设置标志为下载没有完成
                            if (this.threads[i].getDownLength() == -1)
                            {//如果下载失败,再重新下载
                                this.threads[i] = new DownloadThread(this, downloadUrl, this.saveFile, this.block, this.data[i + 1], i + 1);
                                this.threads[i].ThreadRun();
                            }
                        }
                    }
                    if (listener != null)
                    {
                        listener.OnDownloadSize(this.downloadSize);//通知目前已经下载完成的数据长度
                        Console.WriteLine(this.downloadSize);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("下载文件失败");
            }
            return this.downloadSize;
        }
    }
}
