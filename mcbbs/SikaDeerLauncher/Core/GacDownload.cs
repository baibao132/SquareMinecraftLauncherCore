namespace SikaDeerLauncher.Core
{
    using Gac;
    using System;
    using System.IO;

    internal class GacDownload
    {
        internal static int Complete;
        internal static DownLoadFile dlf = new DownLoadFile();
        internal static int Failure;
        internal static int id;
        private static bool s;

        internal static void Download(string path, string url)
        {
            if (!s)
            {
                dlf.doSendMsg += new DownLoadFile.dlgSendMsg(SendMsgHander);
                s = true;
            }
            dlf.AddDown(url, path.Replace(Path.GetFileName(path), ""), Path.GetFileName(path), id);
            dlf.StartDown(3);
        }

        private static void SendMsgHander(DownMsg msg)
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

