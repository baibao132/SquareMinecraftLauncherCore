using SquareMinecraftLauncher.Core.Curseforge;
using System;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Minecraft
{
    public class WorldCurseForge : CurseForgeInterface
    {
        Curseforge Curseforge = new Curseforge();
        public MCDownload download(GameVersionLatestFilesItem LatestFilesItem)
        {
            string url = Curseforge.download(LatestFilesItem.projectFileId, LatestFilesItem.projectFileName);
            string path = Directory.GetCurrentDirectory() + @"\.minecraft\saves\" + LatestFilesItem.projectFileName;
            MCDownload download = new MCDownload();
            download.Url = url;
            download.path = path;
            return download;
        }

        public async Task<CurseForgeItem[]> popular()
        {
            return await popular(null);
        }

        public async Task<CurseForgeItem[]> Search(string name)
        {
            return await Search(name, null);
        }
        public async Task<CurseForgeItem[]> popular(category category)
        {
            CurseForgeItem[] forgeItems = new CurseForgeItem[0];
            await Task.Factory.StartNew(() =>
            {
                forgeItems = Curseforge.popular(17, category);
            });
            return forgeItems;
        }

        public async Task<CurseForgeItem[]> Search(string name, category category)
        {
            CurseForgeItem[] forgeItems = new CurseForgeItem[0];
            await Task.Factory.StartNew(() =>
            {
                forgeItems = Curseforge.Search(name, 17, category);
            });
            return forgeItems;
        }

        public async Task<bool> Install(MCDownload download)
        {
            bool s = false;
            await Task.Factory.StartNew(() =>
            {
                string error = "";
                if (new Unzip().UnZipFile(download.path, Directory.GetCurrentDirectory() + Directory.GetCurrentDirectory() + @"\.minecraft\saves\", out error))
                {
                    s = true;
                }
            });
            return s;
        }
    }
}
