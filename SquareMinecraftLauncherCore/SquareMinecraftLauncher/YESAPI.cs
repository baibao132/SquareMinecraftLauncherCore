namespace AI
{
    using SquareMinecraftLauncher;
    using System;
    using System.Threading;
    using System.Windows.Forms;

    internal class YESAPI
    {
        private static bool a;
        private Download web = new Download();

        internal void p()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                if (!a)
                {
                    string b = web.Post("http://hn2.api.okayapi.com/?s=App.Statistics.GetDailyRequest&start_date=" + DateTime.Now.ToString("yyyy-MM-dd") + "&end_date=" + DateTime.Now.ToString("yyyy-MM-dd") + "&app_key=BA9F2CD7DE34B7B063EB382BA5F346F1", "");
                    if (b != null)
                    {
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

