using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftServer.Json
{
    internal class json19
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

        public class SampleItem
        {
            /// <summary>
            /// §7§l §m    ] §e§l 生§b§l存§d§l都§6§l市 §7§l§m [    
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
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

        public class ExtraItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string text { get; set; }
        }

        public class Description
        {
            /// <summary>
            /// 
            /// </summary>
            public List<ExtraItem> extra { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string text { get; set; }
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
            /// <summary>
            /// 
            /// </summary>
            public Version version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Players players { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Description description { get; set; }
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
