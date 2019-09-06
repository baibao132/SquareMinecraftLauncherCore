using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikaDeerLauncher.BaiduAPIJson
{
    internal class TokenJson
    {
        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string access_token { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int expires_in { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string refresh_token { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string scope { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string session_key { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string session_secret { get; set; }
        }
    }
}
