using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikaDeerLauncher.MinecraftServer.Json
{
    internal class Jsoncj
    {
        public class Version
        {
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int protocol { get; set; }
        }

        public class Players
        {
            /// <summary>
            /// 
            /// </summary>
            public int max { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int online { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> sample { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public Version version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Players players { get; set; }
    public string description { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string favicon { get; set; }
        }
    }
}
