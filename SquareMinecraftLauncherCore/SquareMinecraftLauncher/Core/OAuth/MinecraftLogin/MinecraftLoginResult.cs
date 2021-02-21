using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core.OAuth
{
    internal class MinecraftLoginResult
    {
        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string username { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> roles { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string access_token { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string token_type { get; set; }
            /// <summary>
        }
    } 
}
