using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SquareMinecraftLauncher.Core.ydjson;
using Newtonsoft.Json.Linq;

namespace SquareMinecraftLauncher.Core
{
    internal class ydjson
    {
        public class WebItem
        {
            /// <summary>
            /// 
            /// </summary>
            public List<string> value { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string key { get; set; }
        }

        public class Root
        {
            public JArray translation { get; set; }
        }
    }
}
