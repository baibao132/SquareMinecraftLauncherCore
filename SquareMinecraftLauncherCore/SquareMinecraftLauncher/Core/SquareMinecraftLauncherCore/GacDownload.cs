namespace SquareMinecraftLauncher.Core
{
    using Gac;
    using System;
    using System.IO;

    internal class GacDownload
    {
        internal int Complete;
        internal DownLoadFile dlf = new DownLoadFile();
        internal int Failure;
        internal static int id;
        private bool s;

        internal void Download(string path, string url)
        {
            if (!s)
            {
                dlf.doSendMsg += new DownLoadFile.dlgSendMsg(SendMsgHander);
                s = true;
            }
            dlf.AddDown(url, path.Replace(Path.GetFileName(path), ""), Path.GetFileName(path), id);
            dlf.StartDown(10);
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
                Console.WriteLine(Complete);
                Complete++;
                return;
            }
            Failure++;
        }
    }
}

