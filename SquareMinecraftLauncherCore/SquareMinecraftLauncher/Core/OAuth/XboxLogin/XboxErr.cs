using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core.OAuth
{
    internal class XboxErr
    {
        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string Identity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int XErr { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Redirect { get; set; }
        }
    }
}
