using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher
{
    internal class MinecraftSkinItem
    {
        public class SKIN
        {
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
        }

        public class Textures
        {
            /// <summary>
            /// 
            /// </summary>
            public SKIN SKIN { get; set; }
        }

        public class Root
        {
            public Textures textures { get; set; }
        }
    }
}
