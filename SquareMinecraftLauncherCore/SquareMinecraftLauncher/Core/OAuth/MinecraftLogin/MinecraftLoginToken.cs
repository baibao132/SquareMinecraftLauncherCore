using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core.OAuth
{
    public class MinecraftLoginToken
    {
        /// <summary>
        /// 游戏名
        /// </summary>
        public string name { get; internal set; }
        /// <summary>
        /// uuid
        /// </summary>
        public string uuid { get; internal set; }
        /// <summary>
        /// 皮肤Url
        /// </summary>
        public string SkinUrl { get; internal set; }
    }
}
