using System.Collections.Generic;

namespace SquareMinecraftLauncher.Core.fabricmc
{
    internal class fabricmcJson
    {
        public class Common
        {
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
        }

        public class Server
        {
            /// <summary>
            /// 
            /// </summary>
            public string _comment { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
        }

        public class Libraries
        {
            /// <summary>
            /// 
            /// </summary>
            public List<string> client { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Common> common { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Server> server { get; set; }
        }

        public class MainClass
        {
            /// <summary>
            /// 
            /// </summary>
            public string client { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string server { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public int version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Libraries libraries { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public MainClass mainClass { get; set; }
        }

    }
}
