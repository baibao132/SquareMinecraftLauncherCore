using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core.OAuth
{
    internal class XboxToken
    {
        public class XuiItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string uhs { get; set; }
        }

        public class DisplayClaims
        {
            /// <summary>
            /// 
            /// </summary>
            public List<XuiItem> xui { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string IssueInstant { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string NotAfter { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Token { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public DisplayClaims DisplayClaims { get; set; }
        }
    }
}
