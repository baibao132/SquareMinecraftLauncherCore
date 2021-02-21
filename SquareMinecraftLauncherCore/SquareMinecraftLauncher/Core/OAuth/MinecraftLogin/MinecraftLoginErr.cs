using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core.OAuth
{
    internal class MinecraftLoginErr
    {
        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string path { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string errorType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string error { get; set; }
        }
    }
}
