using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core.Forge
{
    internal class ForgeJson
    {
        public class Arguments
        {
            /// <summary>
            /// 
            /// </summary>
            public List<string> game { get; set; }
        }
        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public Arguments arguments { get; set; }
        }
    }
}
