using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core.OAuth
{
    public class Token
    {
        /// <summary>
        /// access_token
        /// </summary>
        public string access_token { get; internal set; }
        /// <summary>
        /// 用于重新获取Minecraft Token
        /// </summary>
        public string refresh_token { get; internal set; }
    }
}
