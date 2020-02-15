using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikaDeerLauncher.MinecraftServer.Json
{
    internal class jsonForge
    {
        public class SampleItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// §a欢迎来到 §a§l仙§b§l梦§6§l境
            /// </summary>
            public string name { get; set; }
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
            public List<SampleItem> sample { get; set; }
        }

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

        public class ModListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string modid { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string version { get; set; }
        }

        public class Modinfo
        {
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<ModListItem> modList { get; set; }
        }

        public class Root
        {
    public string description { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Players players { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Version version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string favicon { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Modinfo modinfo { get; set; }
        }
    }
}
