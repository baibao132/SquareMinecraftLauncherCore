namespace SikaDeerLauncher.Core
{
    using Gac;
    using System;
    using System.IO;

    internal class GacDownload
    {
        internal int Complete;
        private DownLoadFile dlf = new DownLoadFile();
        internal int Failure;
        internal static int id;
        private bool s;

        internal void Download(string path, string url)
        {
            if (!this.s)
            {
                this.dlf.doSendMsg += new DownLoadFile.dlgSendMsg(this.SendMsgHander);
                this.s = true;
            }
            this.dlf.AddDown(url, path.Replace(Path.GetFileName(path), ""), Path.GetFileName(path), id);
            this.dlf.StartDown(3);
        }

        private void SendMsgHander(DownMsg msg)
        {
            DownStatus tag = msg.Tag;
            if (tag != DownStatus.End)
            {
                if (tag != DownStatus.Error)
                {
                    return;
                }
            }
            else
            {
                Console.WriteLine(this.Complete);
                this.Complete++;
                return;
            }
            this.Failure++;
        }
    }
}

