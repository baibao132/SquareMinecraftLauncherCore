using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SquareMinecraftLauncher.Core.OAuth;
using SquareMinecraftLauncher.Minecraft.MCServerPing;
using SquareMinecraftLauncher.Core.Curseforge;
using SquareMinecraftLauncher.Minecraft;
using System.Threading.Tasks;
using System.Threading;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1Async()
        {
            AssetDownload assetDownload = new AssetDownload();
            assetDownload.DownloadProgressChanged += AssetDownload_DownloadProgressChanged;
            await assetDownload.BuildAssetDownload(4, "1.17.1");

            
        }

        private void AssetDownload_DownloadProgressChanged(AssetDownload.DownloadIntermation Log)
        {
            Console.WriteLine(Log.Progress);
        }
    }
}
