using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncherWPF
{
    public class DIYJson
    {
        public class DIYLoginItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string DIYname { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string s { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string L { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public List<DIYLoginItem> DIYLogin { get; set; }
        }
    }
}
