using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher
{
    internal class MinecraftSkin
    {
        public class PropertiesItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public List<PropertiesItem> properties { get; set; }
        }
    }
}
