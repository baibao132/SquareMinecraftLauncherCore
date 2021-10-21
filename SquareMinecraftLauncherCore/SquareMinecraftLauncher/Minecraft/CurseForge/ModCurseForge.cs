﻿using SquareMinecraftLauncher.Core.Curseforge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Minecraft
{
    public class ModCurseForge : CurseForgeInterface
    {
        Curseforge Curseforge = new Curseforge();
        public MCDownload download(GameVersionLatestFilesItem LatestFilesItem)
        {
            string url = Curseforge.download(LatestFilesItem.projectFileId);
            string path = Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\" + LatestFilesItem.projectFileName;
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

        public async Task<bool> Install(MCDownload download)
        {
            await Task.Factory.StartNew(() =>
            {
                    System.IO.File.Move(download.path, Directory.GetCurrentDirectory() + @"\.minecraft\mods\" + System.IO.Path.GetFileName(download.path));
            });
            return true;
        }

        public async Task<List<CurseForgeItem>> Search(string name, category category)
        {
            List<CurseForgeItem> forgeItems = new List<CurseForgeItem>();
            await Task.Factory.StartNew(() =>
            {
                forgeItems = Curseforge.Search(name, 6,category);
            });
            return forgeItems;
        }

        public async Task<List<CurseForgeItem>> popular(category category)
        {
            List<CurseForgeItem> forgeItems = new List<CurseForgeItem>();
            await Task.Factory.StartNew(() =>
            {
                forgeItems = Curseforge.popular(6,category);
            });
            return forgeItems;
        }
    }
}
