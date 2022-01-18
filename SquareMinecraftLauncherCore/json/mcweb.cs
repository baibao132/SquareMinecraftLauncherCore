using System.Collections.Generic;

namespace SquareMinecraftLauncher.Core.json
{
    internal class mcweb
    {
        public class VersionsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string time { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string releaseTime { get; set; }
        }

        public class Root
        {
            public List<VersionsItem> versions { get; set; }
        }
    }
}
