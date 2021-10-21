using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            internal int projectFileId { get; set; }
            /// <summary>
            /// 文件名
            /// </summary>
            public string projectFileName { get; set; }
        }

        public class CurseForgeItem
    {
            internal int id { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 图标及详情图片
            /// </summary>
            public List<AttachmentsItem> attachments { get; set; }
        /// <summary>
        /// curseforge页面
        /// </summary>
        public string websiteUrl { get; set; }
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
        }
}
