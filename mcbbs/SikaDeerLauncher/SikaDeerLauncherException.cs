namespace SikaDeerLauncher
{
    using SikaDeerLauncher.Core;
    using System;

    public sealed class SikaDeerLauncherException : Exception
    {
        public SikaDeerLauncherException(string message) : base(message)
        {
            SikaDeerLauncherCore core = new SikaDeerLauncherCore();
            core.SetFile("SikaDeerLauncher");
            string file = null;
            if (core.FileExist(@"SikaDeerLauncher\Error.Log") == null)
            {
                file = core.GetFile(@"SikaDeerLauncher\Error.Log");
                core.wj(@"SikaDeerLauncher\Error.Log", file + "\n[" + DateTime.Now.ToString() + "] " + message);
            }
            else
            {
                core.wj(@"SikaDeerLauncher\Error.Log", "[" + DateTime.Now.ToString() + "] " + message);
            }
        }
    }
}

