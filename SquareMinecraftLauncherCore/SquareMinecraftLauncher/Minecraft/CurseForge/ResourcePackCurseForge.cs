using SquareMinecraftLauncher.Core.Curseforge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Minecraft
{
    public class ResourcePackCurseForge : CurseForgeInterface
    {
        Curseforge Curseforge = new Curseforge();
        public MCDownload download(GameVersionLatestFilesItem LatestFilesItem)
        {
            string url = Curseforge.download(LatestFilesItem.projectFileId, LatestFilesItem.projectFileName);
            string path = Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncherDownload\" + LatestFilesItem.projectFileName;
            MCDownload download = new MCDownload();
            download.Url = url;
            download.path = path;
            return download;
        }

        public async Task<List<CurseForgeItem>> popular()
        {
            return await popular(null);
        }

        public async Task<List<CurseForgeItem>> Search(string name)
        {
            return await Search(name, null);
        }

        public async Task<List<CurseForgeItem>> popular(category category)
        {
            List<CurseForgeItem> forgeItems = new List<CurseForgeItem>();
            await Task.Factory.StartNew(() =>
            {
                forgeItems = Curseforge.popular(12,category);
            });
            return forgeItems;
        }

        public async Task<List<CurseForgeItem>> Search(string name, category category)
        {
            List<CurseForgeItem> forgeItems = new List<CurseForgeItem>();
            await Task.Factory.StartNew(() =>
            {
                forgeItems = Curseforge.Search(name, 12,category);
            });
            return forgeItems;
        }

        public async Task<bool> Install(MCDownload download)
        {
            await Task.Factory.StartNew(() =>
            {
                 System.IO.Directory.Move(download.path, Directory.GetCurrentDirectory() + @"\.minecraft\resourcepacks");

            });
            return true;
        }
    }
}
