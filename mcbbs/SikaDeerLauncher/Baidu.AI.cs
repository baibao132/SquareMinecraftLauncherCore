namespace AI
{
    using SikaDeerLauncher;
    using System;
    using System.Threading;
    using System.Windows.Forms;

    internal class Baidu
    {
        private static bool a;
        private Download web = new Download();

        internal void p()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                if (!a && (this.web.Post("http://118.31.6.246/libraries/","") != null))
                {
                    a = true;
                    Console.WriteLine("调用完成");
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

