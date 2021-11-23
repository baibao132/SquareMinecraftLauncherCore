namespace AI
{
    using SquareMinecraftLauncher;
    using System;
    using System.Threading;
    using System.Windows.Forms;

    internal class API
    {
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

