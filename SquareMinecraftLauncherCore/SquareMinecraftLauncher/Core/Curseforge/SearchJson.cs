using System.Collections.Generic;

namespace SquareMinecraftLauncher.Core.Curseforge
{
    public class AttachmentsItem
    {
        /// <summary>
        /// 图片或详情图
        /// </summary>
        public string thumbnailUrl { get; set; }
    }


    public class CategoriesItem
    {
        /// <summary>
        /// 类别名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 类别图片
        /// </summary>
        public string url { get; set; }
    }

    public class GameVersionLatestFilesItem
    {
        /// <summary>
        /// 游戏版本
        /// </summary>
        public string gameVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int projectFileId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string projectFileName { get; set; }
    }

    public class PreModItem
    {
        /// <summary>
        /// mod名称
        /// </summary>
        public string ModName { get; internal set; }
        /// <summary>
        /// mod百科网址
        /// </summary>
        public string WikiUrl { get; internal set; }

    }

    public class ExternalLinkItem
    {
        /// <summary>
        /// 网站名称
        /// </summary>
        public string Title { get; internal set; }
        /// <summary>
        /// 网址
        /// </summary>
        public string url { get; internal set; }
    }

    public class CurseForgeItem
    {
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 图标及详情图片
        /// </summary>
        public List<AttachmentsItem> attachments { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string summary { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public List<CategoriesItem> categories { get; set; }
        /// <summary>
        /// 文件详情
        /// </summary>
        public List<GameVersionLatestFilesItem> gameVersionLatestFiles { get; set; }
        /// <summary>
        /// 外部链接
        /// </summary>
        public ExternalLinkItem[] ExternalLinks { get; internal set; }
        /// <summary>
        /// 前置mod
        /// </summary>
        public PreModItem[] PreMod { get; internal set; }
    }
}
