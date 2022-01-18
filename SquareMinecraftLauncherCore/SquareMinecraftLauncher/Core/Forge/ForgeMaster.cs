using System;
using System.IO;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core.Forge
{
    internal class ForgeMaster
    {
        public static void ForgeInstallDown(string version, string java)
        {
            SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
            ForgeCore core = new ForgeCore();
            if (File.Exists(@"SquareMinecraftLauncher\Forge\version.json"))
            {
                SLC.wj(@".minecraft\versions\" + version + @"\" + version + ".json", core.ForgeJson(version, @"SquareMinecraftLauncher\Forge\version.json", java));
            }
        }

        public static async Task ForgeInstallHigh(string version, string java)
        {
            SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
            ForgeCore core = new ForgeCore();
            if (SLC.FileExist(@"SquareMinecraftLauncher\Forge\install_profile.json") == null)
            {
                await new ForgeInstallCore().ForgeInstall(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\Forge\install_profile.json", version, java);
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\Forge\maven\net\minecraftforge\forge\");
                    string ForgeVersion = dir.GetDirectories()[0].Name;
                    string path = System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\net\minecraftforge\forge\" + ForgeVersion;
                    if (!File.Exists(path + @"\forge-" + ForgeVersion + "-client.jar")) throw new SquareMinecraftLauncherException("请确认使用的jdk安装");
                    foreach (string str4 in System.IO.Directory.GetFiles(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\Forge\maven\net\minecraftforge\forge\" + ForgeVersion))
                    {
                        File.Copy(str4, path + @"\" + Path.GetFileName(str4));
                    }
                }
                catch (Exception ex) { throw new SquareMinecraftLauncherException("安装失败," + ex.Message); }
                if (File.Exists(@"SquareMinecraftLauncher\Forge\version.json"))
                {
                    SLC.wj(@".minecraft\versions\" + version + @"\" + version + ".json", core.ForgeJson(version, @"SquareMinecraftLauncher\Forge\version.json", java));
                }
            }
        }
    }
}
