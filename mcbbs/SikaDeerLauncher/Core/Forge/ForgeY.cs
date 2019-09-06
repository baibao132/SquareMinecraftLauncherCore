using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikaDeerLauncher.Core.Forge
{
    class ForgeY
    {
        public class AssetIndex
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            public string size { get; set; }
        }

        public class Natives
        {
            /// <summary>
            /// 
            /// </summary>
            public string linux { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string osx { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string windows { get; set; }
        }

        public class LibrariesItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Natives natives { get; set; }
            public Downloads downloads { get; set; }
            public string url { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public AssetIndex assetIndex { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string assets { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<LibrariesItem> libraries { get; set; }
            public string mainClass { get; set; }
            public Downloadsy downloads { get; set; }
        }
        public class Client
        {
            /// <summary>
            /// 
            /// </summary>
            public string sha1 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int size { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
        }
        public class Downloadsy
        {
            /// <summary>
            /// 
            /// </summary>
            public Client client { get; set; }
        }
        public class Artifact
        {
            public string url { get; set; }
        }

        public class Downloads
        {
            /// <summary>
            /// 
            /// </summary>
            public Artifact artifact { get; set; }
        }
    }
}
