using SquareMinecraftLauncher.Minecraft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core.Curseforge
{
    public interface CurseForgeInterface
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="name">搜索名</param>
        /// <returns></returns>
        Task<List<CurseForgeItem>> Search(string name);
        ///// <summary>
        ///// 搜索
        ///// </summary>
        ///// <param name="name">搜索名</param>
        ///// <param name="category">类别</param>
        ///// <returns></returns>
        //Task<List<CurseForgeItem>> Search(string name,category category);
        ///// <summary>
        ///// 热门
        ///// </summary>
        ///// <param name="category">类别</param>
        ///// <returns></returns>
        //Task<List<CurseForgeItem>> popular(category category);
        /// <summary>
        /// 热门
        /// </summary>
        /// <returns></returns>
        Task<List<CurseForgeItem>> popular();
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="LatestFilesItem">Item</param>
        /// <returns></returns>
        MCDownload download(GameVersionLatestFilesItem LatestFilesItem);
        /// <summary>
        ///安装
        /// </summary>
        /// <param name="download"></param>
        /// <returns></returns>
        Task<bool> Install(MCDownload download);
    }
}
