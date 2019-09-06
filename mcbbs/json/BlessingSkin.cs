using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlessingSkinJson
{
    internal class BlessingSkin
    {
        public class AvailableProfilesItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string accessToken { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string clientToken { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<AvailableProfilesItem> availableProfiles { get; set; }
        }
    }
}
