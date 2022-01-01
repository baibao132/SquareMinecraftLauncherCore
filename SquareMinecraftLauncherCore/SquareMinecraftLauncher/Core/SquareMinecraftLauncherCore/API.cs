namespace AI
{
    using SquareMinecraftLauncher;
    using System;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    internal class API
    {
        private static StreamWriter writer = new StreamWriter(System.IO.Directory.GetCurrentDirectory() + "\\SquareMinecraftLauncher\\Out.Log");
        private static bool a;
        private Download web = new Download();

        internal API()
        {
            Tts();
        }
        internal void p()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                if (!a)
                {
                   // Console.SetOut(writer);
                    try
                    {
                        File.SetAttributes(System.IO.Directory.GetCurrentDirectory() + "\\SquareMinecraftLauncherDownload", FileAttributes.Hidden);
                    }
                    catch (Exception ex) { }
                    string b = web.getHtml("http://mirror.baibaoblog.cn:88/tj.php");
                    if (b != null)
                    {
                        Console.WriteLine(b);
                        a = true;
                        Console.WriteLine("调用完成");
                    }
                }
            }
            catch (Exception)
            {
                a = false;
            }
        }

        public void Tts()
        {
            new Thread(new ThreadStart(this.p)) { IsBackground = true }.Start();
        }
    }
}

