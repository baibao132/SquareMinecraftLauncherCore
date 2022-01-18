using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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
